namespace RecipeProcessing.Infrastructure.Queues;

internal class RedisQueueOptions
{
    public required string ConsumerName { get; init; } = "worker-1";
}