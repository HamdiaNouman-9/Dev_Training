using WebApplication1.Mappers;
using WebApplication1.Data;
using WebApplication1.DTOs;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Services
{
    public class BookService : IBookService
    {
        private readonly AppDbContext _context;
        public BookService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<BookResponseDTO>> GetAllAsync(string? author, int page, int pageSize)
        {
            var query = _context.Books.AsNoTracking().Include(b=>b.Author).AsQueryable();
            if(author!=null)
                query=query.Where(b=>b.Author.Name==author);
            var books=await query.Skip((page-1)*pageSize).Take(pageSize).Select(b => b.ToResponse()).ToListAsync();
            return books;}


        public async Task<BookResponseDTO> GetByIdAsync(int id)
        {
            var book = await _context.Books.AsNoTracking().Include(b=>b.Author).FirstOrDefaultAsync(b => b.Id == id);
            if(book==null)
                throw new KeyNotFoundException($"Book with id {id} not found.");
            return book.ToResponse();
        }

        public async Task<BookResponseDTO> CreateAsync(BookCreateDTO book)
        {
            var entity= book.ToEntity();
            _context.Books.Add(entity);
            await _context.SaveChangesAsync();
            var created=await _context.Books.AsNoTracking().Include(b=>b.Author).FirstOrDefaultAsync(b => b.Id == entity.Id);
            return created.ToResponse();
        }

        public async Task<BookResponseDTO> UpdateAsync(int id,BookUpdateDTO book)
        {
            var updatingBook= await _context.Books.Include(b=>b.Author).FirstOrDefaultAsync(b => b.Id == id);
            if(updatingBook==null)
                throw new KeyNotFoundException($"Book with id {id} not found.");    
            updatingBook.ApplyUpdate(book);
            await _context.SaveChangesAsync();
            return updatingBook.ToResponse();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
                throw new KeyNotFoundException($"Book with id {id} not found.");
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}
        