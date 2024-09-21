using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Api.Workers;

public class ImageProcessingWorker(
    IAiImageAnalysisService imageProcessor,
    IFileService fileService,
    IQueueService redisQueueService
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var entries = await redisQueueService.GetPendingImageTasksAsync();
            if (entries.Length != 0)
            {
                foreach (var entry in entries)
                {
                    var imagePath = entry.Values.FirstOrDefault(v => v.Name == "filePath").Value.ToString();
                    var fileExtension = entry.Values.FirstOrDefault(v => v.Name == "filePath").Value.ToString();
                    
                    var fileBytes = await File.ReadAllBytesAsync(imagePath, stoppingToken);
                    using var memStream = new MemoryStream(fileBytes);


                    await imageProcessor.Process(memStream, fileExtension );
                }
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
   
            }
        }
    }
}