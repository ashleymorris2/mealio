namespace RecipeProcessing.Infrastructure.Interfaces;

public interface IQueueService
{
    Task EnqueueImageProcessingTaskAsync(Stream imageStream, string contentType, string imageHash);
}