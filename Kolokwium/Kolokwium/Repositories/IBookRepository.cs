using Kolokwium.Models;

namespace Kolokwium.Repositories;

public interface IBookRepository
{
    Task<string?> GetBookTitle(int id);
    Task<int> GetBookId(string title);
    Task<List<string>> GetBookGenres(int id);
    Task<int> PostBook(string title,List<int> genres);
    Task<string> GetGenre(int id);
    Task PostBookGenre(int bookId, int genreId);
}