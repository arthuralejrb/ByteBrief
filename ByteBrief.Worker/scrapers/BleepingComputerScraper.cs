namespace ByteBrief.Worker.scrapers;

using System.ComponentModel.DataAnnotations;
using ByteBrief.Worker.models;
using HtmlAgilityPack;

public class BleepingComputerScraper
{
    private readonly HttpClient _httpClient;

    public BleepingComputerScraper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<NewsArticle>> ScrapeNewArticlesAsync()
    {
        var news = new List<NewsArticle>();

        String source = "https://www.bleepingcomputer.com/";
        var mainPageHtml = await _httpClient.GetStringAsync(source);

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(mainPageHtml);

        var newsContainer = htmlDocument.DocumentNode.SelectNodes("//div[@class='bc_latest_news_text']");
        var newsLinks = new List<String>();

        if(newsContainer != null)
        {
            foreach(var container in newsContainer)
            {
                var categoryNode = container.SelectSingleNode(".//div[contains(@class='bc_latest_news_category')]")
                                        ?? container.SelectSingleNode(".//span[contains(@class='bc_news_cat')]");
                
                String categoryText = categoryNode?.InnerText.Trim().ToLower() ?? String.Empty;

                if (String.IsNullOrEmpty(categoryText) || categoryText.Contains("deals")) continue;

                var linkNode = container.SelectSingleNode(".//h4/a");
                if(linkNode != null)
                {
                    String newsLink = linkNode.GetAttributeValue("href", String.Empty);
                    newsLinks.Add(newsLink);

                }
            }
        }
    
        if(newsLinks == null) return news;

        foreach(var link in newsLinks)
        {
            String textContent = "";

            var newsPageHtml = await _httpClient.GetStringAsync(link);

            var newsPageHtmlDocument = new HtmlDocument();
            newsPageHtmlDocument.LoadHtml(newsPageHtml);

            String title = newsPageHtmlDocument.DocumentNode.SelectSingleNode("//div[@class='article_section']/h1") 
            ?.InnerText
            .Trim()
            ?? String.Empty;

            var newsParagraphs = newsPageHtmlDocument.DocumentNode.SelectNodes("//div[@class='articleBody']/p");

            if(newsParagraphs != null)
            {
                foreach(var paragraph in newsParagraphs)
                {
                    textContent += paragraph.InnerText.Trim() ?? String.Empty;
                    textContent += "\n";
                }
            }

            news.Add(new NewsArticle
            {
                Title = title,
                Id = new Guid(),
                Url = link,
                Source = source,
                TextContent = textContent,
                Score = 0,
                DateFetched = DateOnly.FromDateTime(DateTime.Now),
                Status = NewsStatus.Pending,
            
            });

        }

        return news;

    }
}

