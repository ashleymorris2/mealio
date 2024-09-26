using RecipeProcessing.Infrastructure.Queues;
using StackExchange.Redis;

namespace RecipeProcessing.Infrastructure.Interfaces;

public interface IQueueService
{
    Task AddImageProcessingTaskAsync(string filePath, string imageHash, string extension);
    IAsyncEnumerable<ImageProcessingTask> GetPendingImageTasksAsync(string consumerName);
    Task AcknowledgeProcessedTaskAsync(string streamEntryId);
}