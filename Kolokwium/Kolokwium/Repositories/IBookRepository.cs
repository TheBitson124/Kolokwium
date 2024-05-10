namespace Kolokwium.Repositories;

public interface IBookRepository
{
    Task<string> GetBookTitle(int id);
    Task<List<string>> GetBookGenres(int id);
}