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
    [Route("api/books/{id:int}/genres")]
    public async Task<ActionResult> GetBookGenres()
    {
        return NotFound();
    }
}