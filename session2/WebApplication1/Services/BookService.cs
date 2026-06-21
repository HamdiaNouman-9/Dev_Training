using WebApplication1.Models;
using WebApplication1.Store;

namespace WebApplication1.Services
{
    public class BookService : IBookService
    {
        public async Task<List<Book>> GetAllAsync()
        {
            return await Task.FromResult(InMemoryStore.Books);
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
                return await Task.FromResult(book);
            return await Task.FromResult<Book?>(null);
        }

        public async Task CreateAsync(Book book)
        {
            InMemoryStore.Books.Add(book);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Book book)
        {
         var index = InMemoryStore.Books.FindIndex(b => b.Id == book.Id);
         if (index != -1)
           InMemoryStore.Books[index] = book;
         await Task.CompletedTask;}

        public async Task DeleteAsync(int id)
        {
            InMemoryStore.Books.RemoveAll(b => b.Id == id);
            await Task.CompletedTask;
        }
    }
}
        
