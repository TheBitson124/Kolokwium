using Microsoft.Data.SqlClient;

namespace Kolokwium.Repositories;

public class BookRepository:IBookRepository
{
    private readonly IConfiguration _configuration;

    public BookRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<string> GetBookTitle(int id)
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
}