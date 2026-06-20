using WebApplication1.Services;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

public BookController(IBookService bookService)
{
    _bookService = bookService;
}

[HttpGet]
public async Task<ActionResult<List<Book>>> GetAll()
{
    var books = await _bookService.GetAllAsync();
    return Ok(books);
}
}
}