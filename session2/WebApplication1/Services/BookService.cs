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
            for (int i = 0; i < InMemoryStore.Books.Count; i++)
            {
                if (InMemoryStore.Books[i].Id == book.Id)
                {
                    InMemoryStore.Books[i] = book;
                    break;
                }
            }
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            InMemoryStore.Books.RemoveAll(b => b.Id == id);
            await Task.CompletedTask;
        }
    }
}
        