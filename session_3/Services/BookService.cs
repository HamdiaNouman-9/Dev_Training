using WebApplication1.Mappers;
using WebApplication1.Models;
using WebApplication1.Store;
using WebApplication1.DTOs;

namespace WebApplication1.Services
{
    public class BookService : IBookService
    {
        public async Task<List<BookResponseDTO>> GetAllAsync(string? author, int page, int pageSize)
        {
            var query = InMemoryStore.Books.AsQueryable();
            if(author!=null)
                query=query.Where(b=>b.Author.Name==author);
            var books=query.Skip((page-1)*pageSize).Take(pageSize).Select(b=>BookMapper.ToResponse(b)).ToList();
            return await Task.FromResult(books);}


        public async Task<BookResponseDTO> GetByIdAsync(int id)
        {
            var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
            if(book==null)
                throw new KeyNotFoundException($"Book with id {id} not found.");
            return await Task.FromResult(book.ToResponse());
        }

        public async Task<BookResponseDTO> CreateAsync(BookCreateDTO book)
        {
            var entity= book.ToEntity();
            InMemoryStore.Books.Add(entity);
            await Task.CompletedTask;
            return entity.ToResponse();
        }

        public async Task<BookResponseDTO> UpdateAsync(int id,BookUpdateDTO book)
        {
            var updatingBook= InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
            if(updatingBook==null)
                throw new KeyNotFoundException($"Book with id {id} not found.");    
            updatingBook.ApplyUpdate(book);
            await Task.CompletedTask;
            return updatingBook.ToResponse();
        }

        public async Task DeleteAsync(int id)
        {
            var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                throw new KeyNotFoundException($"Book with id {id} not found.");
            InMemoryStore.Books.Remove(book);
            await Task.CompletedTask;
        }
    }
}
        