using Kolokwium.Models;
using Microsoft.Data.SqlClient;

namespace Kolokwium.Repositories;

public class BookRepository:IBookRepository
{
    private readonly IConfiguration _configuration;

    public BookRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<string?> GetBookTitle(int id)
    {
        var query = "SELECT Title FROM books WHERE PK = @id;";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();
        if (res is null)
        {
            return "";
        }
        return Convert.ToString(res);
    }

    public async Task<int> GetBookId(string title)
    {
        var query = "SELECT PK FROM books WHERE title = @title;";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@title", title);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();
        if (res is null)
        {
            return -1;
        }
        return Convert.ToInt32(res);
    }

    public async Task<List<string>> GetBookGenres(int id)
    {
        var query = "SELECT FK_genre FROM books_genres WHERE FK_book = @id;";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id);
        await connection.OpenAsync();
        var reader = await command.ExecuteReaderAsync();
        List<string> genres = new List<string>();   
        var genreOrdinal = reader.GetOrdinal("FK_genre");
        while (await reader.ReadAsync())
        {
            genres.Add(reader.GetString(genreOrdinal));
        }

        return genres;
    }

    public async Task<int> PostBook(string title, List<int> genres)
    {
        var query = "INSERT INTO books(title) VALUES (@title); SELECT @@IDENTITY AS ID;";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@title", title);

        await connection.OpenAsync();
        var res = await command.ExecuteScalarAsync();
        if (res is null){
            throw new Exception("1");
        }

        var id = Convert.ToInt32(res);
        foreach (var genre in genres)
        {
            query = "INSERT INTO books_genres VALUES (@FK_book, @FK_genre)";
            command.Parameters.AddWithValue("@FK_book", id);
            command.Parameters.AddWithValue("@FK_genre", genre);
            res = await command.ExecuteScalarAsync();
            if (res is null){
                throw new Exception("2");
            }
        }

        return id;
    }


    public async Task<string> GetGenre(int genreId)
    {
        var query = "SELECT nam FROM genres WHERE PK = @id;";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", genreId);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();
        if (res is null)
        {
            return "";
        }
        return Convert.ToString(res);
    }
}