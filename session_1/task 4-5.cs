public interface IBookService
{
   List<Book> GetAll();
   Book? GetById(int id);
   void Create(Book book);
   void Update(Book book);
    void Delete(int id);
}

public class BookService : IBookService
{
    public List<Book> GetAll()
    {
        return InMemoryStore.Books;
    }

    public Book? GetById(int id)
    {
        for(int i = 0; i < InMemoryStore.Books.Count; i++)
        {
            if (InMemoryStore.Books[i].Id == id)
            {
                return InMemoryStore.Books[i];
            }
        }
        return null;
    }

    public void Create(Book book)
    {
        InMemoryStore.Books.Add(book);
    }

    public void Update(Book book)
    {
        for(int i = 0; i < InMemoryStore.Books.Count; i++)
        {
            if (InMemoryStore.Books[i].Id == book.Id)
            {
                InMemoryStore.Books[i].Title = book.Title;
                InMemoryStore.Books[i].Year = book.Year;
                InMemoryStore.Books[i].PageCount = book.PageCount;
                InMemoryStore.Books[i].AuthorId = book.AuthorId;
                break;
            }
        }
       
    }

    public void Delete(int id)
    {
        for(int i = 0; i < InMemoryStore.Books.Count; i++)
        {
            if (InMemoryStore.Books[i].Id == id)
            {
                InMemoryStore.Books.RemoveAt(i);
                break;
            }
        }
    }
}