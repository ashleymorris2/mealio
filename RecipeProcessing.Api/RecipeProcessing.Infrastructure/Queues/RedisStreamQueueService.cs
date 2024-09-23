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

    public async Task<StreamEntry[]> GetPendingImageTasksAsync()
    {
        var database = await EnsureRedisInitialisedAsync(StreamName, GroupName);

        var entries = await database.StreamReadGroupAsync(
            key: StreamName,
            groupName: GroupName,
            consumerName: "worker-1",
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

        //Create stream if it doesn't exist
        if (await database.KeyExistsAsync(streamName) ||
            (await database.StreamGroupInfoAsync(streamName)).Any(x => x.Name == groupName)) return database;
        
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