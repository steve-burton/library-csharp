using Xunit;
using System;
using System.Data;
using System.Collections.Generic;
using Catalog.Startup;
using Catalog.Objects;

namespace Catalog.Tests
{
  public class BookTests : IDisposable
  {
    public void BookTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=catalog_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DBReturnsEmptyAtFirst()
    {
      Assert.Equal(0, Book.GetAll().Count);
    }

    [Fact]
    public void Test_OverloadedEquals()
    {
      Book book1 = new Book("The Sun Also Rises", "Lorem ipsum");
      Book book2 = new Book("The Sun Also Rises", "Lorem ipsum");
      Assert.Equal(book1, book2);
    }

    [Fact]
    public void Test_SaveBookToDatabase()
    {
      Book testBook = new Book("Atlas Shrugged", "Lorem ipsum");
      testBook.Save();
      Author newAuthor = new Author("Ayn Rand");
      newAuthor.Save();
      testBook.AddBook(newAuthor);

      List<Book> result = Book.GetAll();
      List<Book> testList = new List<Book>{testBook};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_FindBookInDatabase()
    {
      Book newBook = new Book("War and Peace", "lorem ipsum");
      newBook.Save();

      Book foundBook = Book.Find(newBook.Id);

      Assert.Equal(newBook, foundBook);
    }

    [Fact]
    public void Test_GetAllBooksInLibrary_true()
    {
      Book testBook1 = new Book("The Sun Also Rises", "Lorem ipsum");
      Book testBook2 = new Book("The Grapes of Wrath", "Lorem ipsum");
      testBook1.Save();
      testBook2.Save();

      List<Book> result = Book.GetAll();
      List<Book> allBooks = new List<Book>{testBook1, testBook2};

      Assert.Equal(result, allBooks);
    }

    [Fact]
    public void Test_UpdateBookInDatabase()
    {
      string testName = "The Greats of Wrath";
      Book testBook = new Book(testName, "Lorem ipsuM");
      testBook.Save();
      string newName = "The Grapes of Wrath";

      testBook.Update(newName, "Lorem ipsum");
      string result = testBook.Title;

      Assert.Equal(newName, result);
    }

    [Fact]
    public void Test_Search_RetrievesListOfMatches()
    {
      Book testBook1 = new Book("The Call of Cthulhu", "A story written by HP Lovecraft.");
      Book testBook2 = new Book("Captain Underpants", "A silly children's book series.");
      Book testBook3 = new Book("The Call of Cthulhu", "A second edition printing by Penguin Books.");
      testBook1.Save();
      testBook2.Save();
      testBook3.Save();

      List<Book> results = Book.SearchByTitle("The Call of Cthulhu");
      List<Book> testAgainst = new List<Book>{testBook1, testBook3};

      Assert.Equal(results, testAgainst);
    }

    [Fact]
    public void Test_Search_RetrievesBooksByAuthor()
    {
      Book testBook1 = new Book("The Call of Cthulhu", "A story written by HP Lovecraft.");
      Book testBook2 = new Book("Captain Underpants", "A silly children's book series.");
      Book testBook3 = new Book("The Call of Cthulhu", "A second edition printing by Penguin Books.");
      testBook1.Save();
      testBook2.Save();
      testBook3.Save();

      Author newAuthor1 = new Author("HP Lovecraft");
      Author newAuthor2 = new Author("Dav Pilkey");
      newAuthor1.Save();
      newAuthor2.Save();

      testBook1.AddBook(newAuthor1);
      testBook2.AddBook(newAuthor2);
      testBook3.AddBook(newAuthor1);

      List<Book> testList = new List<Book>{testBook1, testBook3};
      List<Book> returnedList = Book.SearchByAuthor("HP Lovecraft");

      Assert.Equal(testList, returnedList);
    }

    [Fact]
    public void Test_MultAuthors_AddsMultAuthorsToBook()
    {
      Book testBook1 = new Book("The Call of Hamlet", "From the creators of Much Ado About Cthulhu.");
      testBook1.Save();

      Author newAuthor1 = new Author("HP Lovecraft");
      Author newAuthor2 = new Author("William Shakespeare");
      newAuthor1.Save();
      newAuthor2.Save();

      testBook1.AddBook(newAuthor1);
      testBook1.AddBook(newAuthor2);
      List<Book> returnedBook1 = Book.SearchByAuthor("HP Lovecraft");
      List<Book> returnedBook2 = Book.SearchByAuthor("William Shakespeare");

      Assert.Equal(returnedBook1, returnedBook2);
    }

    public void Dispose()
    {
      Book.DeleteAll();
      Author.DeleteAll();
      Book.DeleteAllInJoinTable();
    }
  }
}
