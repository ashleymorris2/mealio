using RecipeProcessing.Infrastructure.Interfaces;
using StackExchange.Redis;

namespace RecipeProcessing.Infrastructure.Queues;

public class RedisStreamQueueService(IConnectionMultiplexer redis) : IQueueService
{
    public async Task EnsureConsumerGroupExistsAsync(string streamName, string groupName)
    {
        var database = redis.GetDatabase();

        try
        {
            // Create the consumer group
            await database.StreamCreateConsumerGroupAsync(streamName, groupName, position: StreamPosition.NewMessages);
        }
        catch (RedisServerException ex) when (ex.Message.Contains("BUSYGROUP"))
        {
        }
        catch (RedisServerException ex)
        {
            throw;
        }
    }

    public async Task EnqueueImageProcessingTaskAsync(string filePath, string imageHash, string mimeType)
    {
        var database = redis.GetDatabase();

        

        await database.StreamAddAsync("image-processing-stream",
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
        var database = redis.GetDatabase();
        
        await EnsureConsumerGroupExistsAsync("image-processing-stream", "image-process-group");

        var entries = await database.StreamReadGroupAsync(
            key: "image-processing-stream",
            groupName: "image-process-group",
            consumerName: "worker-1",
            position: ">",
            count: 1
        );

        return entries;
    }

    public async Task AcknowledgeProcessedTaskAsync(string streamEntryId)
    {
        var db = redis.GetDatabase();

        // Acknowledge the message as processed
        await db.StreamAcknowledgeAsync("image-processing-stream", "image-process-group", streamEntryId);
    }
}