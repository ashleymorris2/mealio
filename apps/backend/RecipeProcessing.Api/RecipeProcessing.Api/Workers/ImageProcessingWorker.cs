using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Api.Workers;

internal class ImageProcessingWorker(
    IAiImageAnalysisService imageProcessor,
    IFileService fileService,
    IQueueService queueService,
    IServiceScopeFactory serviceScopeFactory
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await foreach (var entry in queueService.GetPendingImageTasksAsync().WithCancellation(stoppingToken))
            {
                using var scope = serviceScopeFactory.CreateScope();
                var recipeService = scope.ServiceProvider.GetRequiredService<IRecipeService>();
                
                try
                {
                    var result = await ProcessImageQueueTask(
                        filePath: entry.FilePath,
                        mimeType: entry.MimeType,
                        stoppingToken
                    );

                    await queueService.AcknowledgeProcessedTaskAsync(entry.StreamEntryId);
                    await recipeService.CreateRecipeFromResult(result, entry.ImageHash);
                    //todo save to cache here
                    
                    fileService.DeleteTemporaryFile(entry.FilePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //todo add logging
                }
            }
            
            //todo add configurable exponential wait?
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }

    private async Task<string> ProcessImageQueueTask(string filePath, string mimeType, CancellationToken stoppingToken)
    {
        var fileBytes = await File.ReadAllBytesAsync(filePath, stoppingToken);
        using var memoryStream = new MemoryStream(fileBytes);

        return await imageProcessor.Process(memoryStream, mimeType);
    }
}