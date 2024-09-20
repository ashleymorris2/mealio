using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Api.Workers;

public class ImageProcessingWorker(
    IAiImageAnalysisService imageProcessor,
    IQueueService redisQueueService
) : BackgroundService
{
   

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var entries = await redisQueueService.();
        }
    }
}