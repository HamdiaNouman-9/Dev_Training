using WebApplication1.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using Microsoft.AspNetCore.Authorization;
namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

public BookController(IBookService bookService)
{
    _bookService = bookService;
}

[HttpGet]
public async Task<ActionResult<List<BookResponseDTO>>> GetAll(
    [FromQuery] string? author,
    [FromQuery] int page=1,
    [FromQuery] int pageSize=10)
        {
            var books=await _bookService.GetAllAsync(author,page,pageSize);
            return Ok(books);
        }
[HttpGet("{id}")]       
public async Task<ActionResult<BookResponseDTO>> GetById([FromRoute] int id)
{
    try{
    var book = await _bookService.GetByIdAsync(id);
    return Ok(book);}
    catch(KeyNotFoundException ex)
    {
        return NotFound(ex.Message);
    }
}
[HttpPost]
public async Task<ActionResult<BookResponseDTO>> Create([FromBody] BookCreateDTO dto)
        {
            var book=await _bookService.CreateAsync(dto);
            return Created($"/api/books/{book.Id}", book);
        }
[HttpPut("{id}")]
public async Task<ActionResult<BookResponseDTO>> Update([FromRoute] int id, [FromBody] BookUpdateDTO dto)
{
    try{
    var book = await _bookService.UpdateAsync(id,dto);
    return Ok(book);}
    catch(KeyNotFoundException ex)
    {
        return NotFound(ex.Message);
    }
}
[HttpDelete("{id}")]
public async Task<ActionResult> Delete([FromRoute] int id)
{
    try{
    await _bookService.DeleteAsync(id);
    return NoContent();}
    catch(KeyNotFoundException ex)
    {
        return NotFound(ex.Message);
    }
}
}
}