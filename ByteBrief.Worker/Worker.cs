using ByteBrief.Worker.scrapers;

namespace ByteBrief.Worker;

public class Worker : BackgroundService
{
    private readonly BleepingComputerScraper _scraper;
    private readonly ILogger<Worker> _logger;

    public Worker(BleepingComputerScraper scraper, ILogger<Worker> logger)
    {
        _scraper = scraper;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bleepingComputerArticles = await _scraper.ScrapeNewArticlesAsync();
        

        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
