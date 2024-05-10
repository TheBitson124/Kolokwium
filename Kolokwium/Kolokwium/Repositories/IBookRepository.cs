namespace Kolokwium.Repositories;

public interface IBookRepository
{
    Task<bool> DoesBookExist(int id);
}