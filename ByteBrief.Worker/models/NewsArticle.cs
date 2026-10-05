namespace ByteBrief.Worker.models;

public class NewsArticle
{   
    public Guid Id {get; set;}
    public string Url {get; set;} = null!;
    public string Source {get; set;} = null!;
    public string TextContent {get; set;} = null!;
    public int Score {get; set;}
    public DateOnly DataFetched {get; set;}
    public DateOnly DataSent {get; set;}    
    public NewsStatus Status {get; set;}

}