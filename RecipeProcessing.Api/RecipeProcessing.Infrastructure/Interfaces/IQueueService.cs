namespace RecipeProcessing.Infrastructure.Interfaces;

public interface IQueueService
{
    Task EnqueueImageProcessingTaskAsync(string filePath, string imageHash);
}