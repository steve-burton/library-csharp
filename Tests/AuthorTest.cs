using Xunit;
using System;
using System.Data;
using System.Collections.Generic;
using Catalog.Startup;
using Catalog.Objects;

namespace Catalog.Tests
{
  public class AuthorTests : IDisposable
  {
    public void AuthorTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=catalog_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DBReturnsEmptyAtFirst()
    {
      Assert.Equal(0, Author.GetAll().Count);
    }

    [Fact]
    public void Test_OverloadedEquals()
    {
      Author author1 = new Author("Ernest Hemmingway");
      Author author2 = new Author("Ernest Hemmingway");
      Assert.Equal(author1, author2);
    }

    [Fact]
    public void Test_SaveAuthorToDatabase()
    {
      Author testAuthor = new Author("Ayn Rand");
      testAuthor.Save();

      List<Author> result = Author.GetAll();
      List<Author> testList = new List<Author>{testAuthor};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_FindAuthorInDatabase()
    {
      Author newAuthor = new Author("Leo Tolstoy");
      newAuthor.Save();

      Author foundAuthor = Author.Find(newAuthor.Id);

      Assert.Equal(newAuthor, foundAuthor);
    }

    [Fact]
    public void Test_UpdateAuthorInDatabase()
    {
      string testName = "Steenbeck";
      Author testAuthor = new Author(testName);
      testAuthor.Save();
      string newName = "Steinbeck";

      testAuthor.Update(newName);
      string result = testAuthor.AuthorName;

      Assert.Equal(newName, result);
    }

    public void Dispose()
    {
      Author.DeleteAll();
    }
  }
}
