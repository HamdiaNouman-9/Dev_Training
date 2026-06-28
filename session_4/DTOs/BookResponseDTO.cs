
namespace WebApplication1.DTOs;
public record BookResponseDTO
{
    public int Id { get; init; }
    public string Title { get; init; }
    public int Year { get; init; }
    public int PageCount { get; init; }
    public string AuthorName { get; init; }
}