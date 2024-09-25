using RecipeProcessing.Infrastructure.Interfaces;
using StackExchange.Redis;

namespace RecipeProcessing.Api.Workers;

public class ImageProcessingWorker(
    IAiImageAnalysisService imageProcessor,
    IRecipeService recipeService,
    IFileService fileService,
    IQueueService redisQueueService
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var entries = await redisQueueService.GetPendingImageTasksAsync(consumerName: "worker-1");
            if (entries.Length != 0)
            {
                foreach (var entry in entries)
                {
                    try
                    {
                        var filePath = entry.Values.FirstOrDefault(v => v.Name == "filePath").Value.ToString();
                        var mimeType = entry.Values.FirstOrDefault(v => v.Name == "mimeType").Value.ToString();
                        var imageHash = entry.Values.FirstOrDefault(v => v.Name == "imageHash").Value.ToString();

                        var result = await ProcessImageQueueTask(
                            filePath: filePath,
                            mimeType: mimeType,
                            stoppingToken
                        );

                        await redisQueueService.AcknowledgeProcessedTaskAsync(entry.Id!);
                        await recipeService.SaveRecipeFromResult(result, imageHash);

                        fileService.DeleteTemporaryFile(filePath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        //add logging
                    }
                }
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }

    private async Task<string> ProcessImageQueueTask(string filePath, string mimeType, CancellationToken stoppingToken)
    {
        var fileBytes = await File.ReadAllBytesAsync(filePath, stoppingToken);
        using var memStream = new MemoryStream(fileBytes);

        return await imageProcessor.Process(memStream, mimeType);
    }
}