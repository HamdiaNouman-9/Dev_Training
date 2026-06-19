public class Author
{
    public int Id {get;set;}
    public string Name {get;set;}
    public ICollection<Book> Books {get;set;}
}

public class Book{
    public int Id {get;set;}
    public string Title {get;set;}
    public int Year  {get;set;}
    public int PageCount {get;set;}
    public int AuthorId {get;set;}
    public Author Author {get;set;}
}

public class Tag
{
    public int Id {get;set;}
    public string Name {get;set;}
}
public class BookTag
{
    public int BookId {get;set;}
    public Book Book {get;set;}
    public int TagId {get;set;}
    public Tag Tag {get;set;}
    
}

public static class InMemoryStore
{
    public static List<Author> Authors = new List<Author>
    {
        new Author{Id=1, Name="Author 1"},
        new Author{Id=2, Name="Author 2"},
        new Author{Id=3, Name="Author 3"}
    };
    public static List<Book> Books = new List<Book>
    {
        new Book{Id=1, Title="Book 1", Year=2000, PageCount=100, AuthorId=1},
        new Book{Id=2, Title="Book 2", Year=2001, PageCount=100, AuthorId=1},
        new Book{Id=3, Title="Book 3", Year=2002, PageCount=100, AuthorId=1},
        new Book{Id=4, Title="Book 4", Year=2003, PageCount=100, AuthorId=2},
        new Book{Id=5, Title="Book 5", Year=2004, PageCount=100, AuthorId=2},
        new Book{Id=6, Title="Book 6", Year=2005, PageCount=100, AuthorId=2},
        new Book{Id=7, Title="Book 7", Year=2006, PageCount=100, AuthorId=3},
        new Book{Id=8, Title="Book 8", Year=2007, PageCount=100, AuthorId=3}
    };
    public static List<Tag> Tags = new List<Tag>
    {
        new Tag{Id=1, Name="Tag 1"},
        new Tag{Id=2, Name="Tag 2"},
        new Tag{Id=3, Name="Tag 3"},
        new Tag{Id=4, Name="Tag 4"}
    };
    public static List<BookTag> BookTags = new List<BookTag>
    {
        new BookTag{BookId=1, TagId=1},
        new BookTag{BookId=1, TagId=2},
        new BookTag{BookId=2, TagId=1},
        new BookTag{BookId=2, TagId=3},
        new BookTag{BookId=3, TagId=2},
        new BookTag{BookId=3, TagId=4}
    };
  
}
public class Tasks
{
    public static void Main(string[] args)
    {
    List<Book> books=Books.Where(b=>b.AuthorId==1).ToList();
   List<Book> titlesSorted=Books.OrderBy(b=>b.Title).ToList();
   var groupByAuthor=Books.GroupBy(b=>b.AuthorId).ToList();
   double averagePageCount=Books.Average(b=>b.PageCount);
   bool greaterThan500Pages=Books.Any(b=>b.PageCount>500);
   Book? book=Books.FirstOrDefault(b=>b.Id==1);
   List<Book> threeLongestBooks=Books.OrderByDescending(b=>b.PageCount).Take(3).ToList();
   var joinBooksAuthors=Books.Join(Authors,b=>b.AuthorId,a=>a.Id,(b,a)=>new{b.Title,a.Name}).ToList();
        
    }
}