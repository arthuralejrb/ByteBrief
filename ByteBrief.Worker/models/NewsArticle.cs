using System.ComponentModel.DataAnnotations;

namespace ByteBrief.Worker.models;

public class NewsArticle
{   
    public Guid Id {get; set;}
    public string Title {get; set;} = null!;
    public string Url {get; set;} = null!;
    public string Source {get; set;} = null!;
    public string TextContent {get; set;} = null!;
    public string? Summary {get; set;}
    public int Score {get; set;}
    public DateOnly DateFetched {get; set;}
    public DateOnly? DateSent {get; set;}
    public NewsStatus Status {get; set;}

}