using RecipeProcessing.Infrastructure.Interfaces;
using StackExchange.Redis;

namespace RecipeProcessing.Infrastructure.Queues;

public class RedisStreamQueueService(IConnectionMultiplexer redis) : IQueueService
{
    private readonly IConnectionMultiplexer _redis = redis;

    public async Task EnqueueImageProcessingTaskAsync(Stream imageStream, string contentType, string imageHash)
    {
        var database = _redis.GetDatabase();
        
        using var memoryStream = new MemoryStream();
        await imageStream.CopyToAsync(memoryStream);
        var imageBase64 = Convert.ToBase64String(memoryStream.ToArray());

        await database.StreamAddAsync("image-processing-stream", [
            new NameValueEntry(nameof(imageBase64), imageBase64),
            new NameValueEntry(nameof(contentType), contentType),
            new NameValueEntry(nameof(imageHash), imageHash)
        ]);
    }
}