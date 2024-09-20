using RecipeProcessing.Infrastructure.Interfaces;
using StackExchange.Redis;

namespace RecipeProcessing.Infrastructure.Queues;

public class RedisStreamQueueService(IConnectionMultiplexer redis) : IQueueService
{
    public async Task EnqueueImageProcessingTaskAsync(string filePath, string imageHash)
    {
        var database = redis.GetDatabase();
        
        await database.StreamAddAsync("image-processing-stream",
            [
                new NameValueEntry(nameof(filePath), filePath),
                new NameValueEntry(nameof(imageHash), imageHash)
            ],
            maxLength: 500,
            useApproximateMaxLength: true
        );
    }
    
    public async Task<StreamEntry[]> GetPendingImageTasksBlockingAsync()
    {
        var database = redis.GetDatabase();
        
        // Blocking read: Wait for new entries
        var entries = await database.StreamReadGroupAsync(
            key: "image-processing-stream", 
            groupName:"image-process-group", 
            consumerName:"worker-1",  
            ">", 
            count: 1, 
            CommandFlags.None,
            0);
        
        return entries;
    }
}