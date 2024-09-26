using RecipeProcessing.Infrastructure.Models;
using RecipeProcessing.Infrastructure.Queues;
using StackExchange.Redis;

namespace RecipeProcessing.Infrastructure.Interfaces;

public interface IQueueService
{
    Task AddImageProcessingTaskAsync(string filePath, string imageHash, string extension);
    IAsyncEnumerable<ImageProcessingTask> GetPendingImageTasksAsync();
    Task AcknowledgeProcessedTaskAsync(string streamEntryId);
}