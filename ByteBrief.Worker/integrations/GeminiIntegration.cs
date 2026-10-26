namespace ByteBrief.Worker.integrations;

using System.Text.Json;
using ByteBrief.Worker.models;
using Google.GenAI;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

public class GeminiIntegration
{
    private Client _client {get;set;}

    public GeminiIntegration(string apiKey) 
    {
        if (apiKey == string.Empty)throw new Exception("empty apiKey string");
        _client = new Client(apiKey: apiKey);   
        
    }


    public async Task enrichArticleWithAIAsynch(List<NewsArticle> articles)
    {
        if(articles == null) throw new Exception("articles is empty!");
        if(articles.Count == 0) throw new Exception("there is no articles"); 

        // building the prompt
        var prompt = "Hi Gemini, I am going to send you the content of 5 different articles, separated by '---'. " +
        "I would like you to summarize each one—in no more than 2 lines—in an impactful way." +
        "and then you will give the article a score in relevance (1 to 10), there should be no repeated score" +
        "Please respond exclusively with a JSON object containing the summary strings and score int, in the exact same order." +
        "Do not include any other text; provide only the JSON, in a format that i can deserialize using a List of the class GeminiResponseDTO, that is defined as:" +
        "{string summary; int score;}\n";

        foreach(var article in articles)
        {
            prompt += $"Article: {article.Title}\n";
            prompt += $"{article.TextContent}\n";
            prompt += "---\n";

        }

        Console.WriteLine(prompt + "\n");
        // trying to get a response from the API
        int maxAttempts = 4;

        for(int actualAttempt = 0; actualAttempt < maxAttempts; actualAttempt++)
        {
            try
            {
                var response = await _client.Models.GenerateContentAsync(
                    model: "gemini-3.1-flash-lite", contents: prompt
                );

                string rawResponse = response.Candidates[0].Content.Parts[0].Text ?? String.Empty;
                string cleanJson = rawResponse.Replace("```json", "").Replace("```", "").Trim();

                var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var summaries = JsonSerializer.Deserialize<List<GeminiResponseDTO>>(cleanJson, jsonOptions);
                int i = 0;

                if(summaries != null && summaries.Count == articles.Count)
                {
                    foreach(var article in articles)
                    {
                        article.Summary = summaries[i].summary;
                        article.Score = Convert.ToInt32(summaries[i].score);
                        i++;

                    }
                    return;
                }

            }catch(Exception ex)
            {
                Console.WriteLine("Error trying to get gemini api response: " + ex.Message);
                await Task.Delay(TimeSpan.FromSeconds(actualAttempt * 25));
            }
        }
    }
}