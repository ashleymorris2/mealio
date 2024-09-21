using StackExchange.Redis;

namespace RecipeProcessing.Infrastructure.Interfaces;

public interface IQueueService
{
    Task EnqueueImageProcessingTaskAsync(string filePath, string imageHash, string extension);
    Task<StreamEntry[]> GetPendingImageTasksAsync();
}