using RecipeProcessing.Infrastructure.Interfaces;
using StackExchange.Redis;

namespace RecipeProcessing.Api.Workers;

public class ImageProcessingWorker(
    IAiImageAnalysisService imageProcessor,
    IRecipeService recipeService,
    IFileService fileService,
    IQueueService queueService
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
           
       
                await foreach (var entry in queueService.GetPendingImageTasksAsync(consumerName: "worker-1").WithCancellation(stoppingToken))
                {
                    try
                    {
                        var result = await ProcessImageQueueTask(
                            filePath: entry.FilePath,
                            mimeType: entry.MimeType,
                            stoppingToken
                        );

                        await queueService.AcknowledgeProcessedTaskAsync(entry.StreamEntryId);
                        await recipeService.SaveRecipeFromResult(result, entry.ImageHash);

                        fileService.DeleteTemporaryFile(entry.FilePath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        //add logging
                    }
                }
            
            // else
            // {
            //     await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            // }
        }
    }

    private async Task<string> ProcessImageQueueTask(string filePath, string mimeType, CancellationToken stoppingToken)
    {
        var fileBytes = await File.ReadAllBytesAsync(filePath, stoppingToken);
        using var memoryStream = new MemoryStream(fileBytes);

        return await imageProcessor.Process(memoryStream, mimeType);
    }
}