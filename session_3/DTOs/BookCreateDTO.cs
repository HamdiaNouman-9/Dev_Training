using System.ComponentModel.DataAnnotations;
namespace WebApplication1.DTOs;
public class BookCreateDTO
{
    [Required (ErrorMessage = "Title is required.")]
    public string Title { get; set; }
    public int Year { get; set; }
    public int PageCount { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "AuthorId must be valid.")]
    public int AuthorId { get; set; }
}