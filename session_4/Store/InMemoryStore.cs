using WebApplication1.Models;

namespace WebApplication1.Store
{
    public static class InMemoryStore
    {
        public static List<Author> Authors = new List<Author>
        {
            new Author { Id = 1, Name = "Author 1" },
            new Author { Id = 2, Name = "Author 2" },
            new Author { Id = 3, Name = "Author 3" }
        };

        public static List<Book> Books = new List<Book>
        {
            new Book { Id = 1, Title = "Book 1", Year = 2000, PageCount = 100, AuthorId = 1 },
            new Book { Id = 2, Title = "Book 2", Year = 2001, PageCount = 100, AuthorId = 1 },
            new Book { Id = 3, Title = "Book 3", Year = 2002, PageCount = 100, AuthorId = 1 },
            new Book { Id = 4, Title = "Book 4", Year = 2003, PageCount = 100, AuthorId = 2 },
            new Book { Id = 5, Title = "Book 5", Year = 2004, PageCount = 100, AuthorId = 2 },
            new Book { Id = 6, Title = "Book 6", Year = 2005, PageCount = 100, AuthorId = 2 },
            new Book { Id = 7, Title = "Book 7", Year = 2006, PageCount = 100, AuthorId = 3 },
            new Book { Id = 8, Title = "Book 8", Year = 2007, PageCount = 100, AuthorId = 3 }
        };

        public static List<Tag> Tags = new List<Tag>
        {
            new Tag { Id = 1, Name = "Tag 1" },
            new Tag { Id = 2, Name = "Tag 2" },
            new Tag { Id = 3, Name = "Tag 3" },
            new Tag { Id = 4, Name = "Tag 4" }
        };

        public static List<BookTag> BookTags = new List<BookTag>
        {
            new BookTag { BookId = 1, TagId = 1 },
            new BookTag { BookId = 1, TagId = 2 },
            new BookTag { BookId = 2, TagId = 1 },
            new BookTag { BookId = 2, TagId = 3 },
            new BookTag { BookId = 3, TagId = 2 },
            new BookTag { BookId = 3, TagId = 4 }
        };
    }
}