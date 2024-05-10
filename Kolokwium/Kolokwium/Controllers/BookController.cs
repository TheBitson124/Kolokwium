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
    public async Task<ActionResult<ReturnBookDTO>> GetBookGenres(int id)
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
        return returnBookDto;
    }
    [HttpPost]
    [Route()]
}