namespace Kolokwium.Models;

public class AddBook
{
    public  string title{ get; set; }
    public List<int> genres { get; set; } = new List<int>();

}