using Microsoft.Extensions.Options;
using RecipeProcessing.Infrastructure.Interfaces;
using RecipeProcessing.Infrastructure.Models;
using StackExchange.Redis;

namespace RecipeProcessing.Infrastructure.Queues;

internal class RedisQueueService : IQueueService
{
    private const string StreamName = "image-processing-stream";
    private const string GroupName = "image-process-group";

    private readonly IDatabase _database;
    private readonly string _consumerName;

    public RedisQueueService(IConnectionMultiplexer redis, IOptions<RedisQueueOptions> options)
    {
        _database = redis.GetDatabase();
        _consumerName = options.Value.ConsumerName;
        
        InitializeStreams();
    }

    public async Task AddImageProcessingTaskAsync(string filePath, string imageHash, string mimeType)
    {
        await _database.StreamAddAsync(StreamName,
            [
                new NameValueEntry(nameof(ImageProcessingTask.FilePath), filePath),
                new NameValueEntry(nameof(ImageProcessingTask.ImageHash), imageHash),
                new NameValueEntry(nameof(ImageProcessingTask.MimeType), mimeType),
            ],
            maxLength: 500,
            useApproximateMaxLength: true
        );
    }

    public async IAsyncEnumerable<ImageProcessingTask> GetPendingImageTasksAsync()
    {
        var entries = await _database.StreamReadGroupAsync(
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
        await _database.StreamAcknowledgeAsync(StreamName, GroupName, streamEntryId);
    }
    
    private void InitializeStreams()
    {
        if (_database.KeyExists(StreamName))
        {
            var groups = _database.StreamGroupInfo(StreamName);
            if (groups.All(group => group.Name != GroupName))
            {
                _database.StreamCreateConsumerGroup(StreamName, GroupName, "$", createStream: false);
            }
        }
        else
        {
            _database.StreamCreateConsumerGroup(StreamName, GroupName, "$", createStream: true);
        }
    }
}