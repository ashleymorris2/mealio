using RecipeProcessing.Infrastructure.Interfaces;
using StackExchange.Redis;

namespace RecipeProcessing.Infrastructure.Queues;

public class RedisStreamQueueService(IConnectionMultiplexer redis) : IQueueService
{
    public async Task EnqueueImageProcessingTaskAsync(string filePath, string imageHash, string fileExtension)
    {
        var database = redis.GetDatabase();

        await database.StreamAddAsync("image-processing-stream",
            [
                new NameValueEntry(nameof(filePath), filePath),
                new NameValueEntry(nameof(imageHash), imageHash),
                new NameValueEntry(nameof(fileExtension), fileExtension),
            ],
            maxLength: 500,
            useApproximateMaxLength: true
        );
    }

    public async Task<StreamEntry[]> GetPendingImageTasksAsync()
    {
        var database = redis.GetDatabase();

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