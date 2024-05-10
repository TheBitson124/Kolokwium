namespace Kolokwium.Models;

public class ReturnBookDTO
{
    public int id { get; set; }
    public  string? title{ get; set; }
    public List<string> genres { get; set; } = new List<string>();
}