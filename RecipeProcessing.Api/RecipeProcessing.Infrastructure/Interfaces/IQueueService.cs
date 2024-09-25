using StackExchange.Redis;

namespace RecipeProcessing.Infrastructure.Interfaces;

public interface IQueueService
{
    Task AddImageProcessingTaskAsync(string filePath, string imageHash, string extension);
    Task<StreamEntry[]> GetPendingImageTasksAsync(string consumerName);
    Task AcknowledgeProcessedTaskAsync(string streamEntryId);
}