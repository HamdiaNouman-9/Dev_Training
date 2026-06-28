using WebApplication1.Models;
using WebApplication1.DTOs;
namespace WebApplication1.Services
{
    public interface IBookService
    {
        Task<List<BookResponseDTO>> GetAllAsync(string? author, int page, int pageSize);
        Task<BookResponseDTO> GetByIdAsync(int id);
        Task<BookResponseDTO> CreateAsync(BookCreateDTO dto);
        Task<BookResponseDTO> UpdateAsync(int id, BookUpdateDTO dto);
        Task DeleteAsync(int id);
    }
}