using RecipeProcessing.Infrastructure.Interfaces;
using StackExchange.Redis;

namespace RecipeProcessing.Infrastructure.Queues;

public class RedisStreamQueueService(IConnectionMultiplexer redis) : IQueueService
{
    private const string StreamName = "image-processing-stream";
    private const string GroupName = "image-process-group";

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

    public async Task<StreamEntry[]> GetPendingImageTasksAsync(string consumerName)
    {
        var database = await EnsureRedisInitialisedAsync(StreamName, GroupName);

        var entries = await database.StreamReadGroupAsync(
            key: StreamName,
            groupName: GroupName,
            consumerName: consumerName,
            position: ">",
            count: 1
        );

        return entries;
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