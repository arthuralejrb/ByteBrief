using ByteBrief.Worker.data;
using ByteBrief.Worker.integrations;
using ByteBrief.Worker.scrapers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ByteBrief.Worker;

public class Worker : BackgroundService
{
    private readonly BleepingComputerScraper _scraper;
    private readonly ILogger<Worker> _logger;
    private readonly IDbContextFactory<ByteBriefContext> _dbContextFactory;
    private readonly IConfiguration _configuration;

    public Worker(BleepingComputerScraper scraper, ILogger<Worker> logger, IDbContextFactory<ByteBriefContext> dbContextFactory, IConfiguration configuration)
    {
        _scraper = scraper;
        _logger = logger;
        _dbContextFactory = dbContextFactory;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var bleepingComputerArticles = await _scraper.ScrapeNewArticlesAsync();
        _logger.LogInformation("Bleeping computer scrapped: {newsCount} articles at: {time}", bleepingComputerArticles.Count(), DateTimeOffset.Now);
        
        var apiKey = _configuration.GetValue<string>("Gemini:apiKey") ?? String.Empty;
        var geminiAPI = new GeminiIntegration(apiKey);

        await geminiAPI.enrichArticleWithAIAsynch(bleepingComputerArticles);
        foreach(var article in bleepingComputerArticles)
        {
            Console.WriteLine("Article: "+ article.Title + "\nSummary: " + article.Summary + "\n" + "Score: " + article.Score + "\n");
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
