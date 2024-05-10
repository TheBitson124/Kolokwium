using System.Transactions;
using Kolokwium.Models;
using Kolokwium.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BookController:ControllerBase
{
    private readonly IBookRepository _bookRepository;

    public BookController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    [HttpGet]
    [Route("{id:int}/genres")]
    public async Task<IActionResult> GetBookGenres(int id)
    {
        var title = await _bookRepository.GetBookTitle(id);
        if (title.Equals(""))
        {
            return NotFound($"Book id {id} not found");
        }

        ReturnBookDTO returnBookDto = new ReturnBookDTO()
        {
            id = id,
            title = title,
            genres = await _bookRepository.GetBookGenres(id)
        };
        return Ok(returnBookDto);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> PostBook(AddBook data)
    {
        ReturnBookDTO returnBookDto = new ReturnBookDTO();
        foreach (var genreId in data.genres)
        {
            var g = await _bookRepository.GetGenre(genreId);
            if (g == "")
            {
                return NotFound($"Genre Id {genreId} not found");
            }
            returnBookDto.genres.Add(g);
        }
        returnBookDto.title = data.title;
        var idBook = -1;
        using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            idBook = await _bookRepository.PostBook(data.title,data.genres);
            returnBookDto.id = idBook;
            foreach (var g in data.genres)
            {
                await _bookRepository.PostBookGenre(returnBookDto.id, g);
            }
            scope.Complete();
        }
        if (idBook == -1)
        {
            return NotFound("Book not created, payment required");
        }
        return Created(Request.Path.Value ?? $"api/books/{idBook}/genres", returnBookDto);
    }
}