using Microsoft.Extensions.Options;
using RecipeProcessing.Infrastructure.Interfaces;
using RecipeProcessing.Infrastructure.Models;
using StackExchange.Redis;

namespace RecipeProcessing.Infrastructure.Queues;

internal class RedisQueueService(IConnectionMultiplexer redis, IOptions<RedisQueueOptions> options) : IQueueService
{
    private const string StreamName = "image-processing-stream";
    private const string GroupName = "image-process-group";
    
    private readonly string _consumerName = options.Value.ConsumerName;
    
    public async Task AddImageProcessingTaskAsync(string filePath, string imageHash, string mimeType)
    {
        var database = await EnsureRedisInitialisedAsync(StreamName, GroupName);

        await database.StreamAddAsync(StreamName,
            [
                new NameValueEntry(nameof(filePath), filePath),
                new NameValueEntry(nameof(imageHash), imageHash),
                new NameValueEntry(nameof(mimeType), mimeType),
            ],
            maxLength: 500,
            useApproximateMaxLength: true
        );
    }

    public async IAsyncEnumerable<ImageProcessingTask> GetPendingImageTasksAsync()
    {
        var database = await EnsureRedisInitialisedAsync(StreamName, GroupName);

        var entries = await database.StreamReadGroupAsync(
            key: StreamName,
            groupName: GroupName,
            consumerName: _consumerName,
            position: ">",
            count: 1
        );

        foreach (var entry in entries)
        {
            yield return new ImageProcessingTask()
            {
                FilePath = entry.Values.FirstOrDefault(v => v.Name == nameof(ImageProcessingTask.FilePath)).Value
                    .ToString(),
                ImageHash = entry.Values.FirstOrDefault(v => v.Name == nameof(ImageProcessingTask.ImageHash)).Value
                    .ToString(),
                MimeType = entry.Values.FirstOrDefault(v => v.Name == nameof(ImageProcessingTask.MimeType)).Value
                    .ToString(),
                StreamEntryId = entry.Id.ToString()
            };
        }
    }

    public async Task AcknowledgeProcessedTaskAsync(string streamEntryId)
    {
        var database = await EnsureRedisInitialisedAsync(StreamName, GroupName);
        await database.StreamAcknowledgeAsync(StreamName, GroupName, streamEntryId);
    }

    private async Task<IDatabase> EnsureRedisInitialisedAsync(string streamName, string groupName)
    {
        var database = redis.GetDatabase();

        //Apparently the only threadsafe way to check if a consumer group exists
        try
        {
            await database.StreamCreateConsumerGroupAsync(streamName, groupName, "0-0");
        }
        catch (RedisServerException ex) when (ex.Message.Contains("BUSYGROUP"))
        {
            // Group already exists, no further action needed
            //Log here
        }

        return database;
    }
}