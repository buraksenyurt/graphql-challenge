namespace ProductService.Model;

public class UserComment
{
    public string Content { get; set; } = string.Empty;
    public int Star { get; set; }
    public DateTime Date { get; set; }
}