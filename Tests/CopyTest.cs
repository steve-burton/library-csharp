using Xunit;
using System;
using System.Data;
using System.Collections.Generic;
using Catalog.Startup;
using Catalog.Objects;

namespace Catalog.Tests
{
  public class CopyTests : IDisposable
  {
    public void BookTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=catalog_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DBReturnsEmptyAtFirst()
    {
      Assert.Equal(0, Copy.GetAll().Count);
    }

    [Fact]
    public void Test_OverloadedEquals()
    {
      Copy copy1 = new Copy(1, 5);
      Copy copy2 = new Copy(1, 5);
      Assert.Equal(copy1, copy2);
    }

    [Fact]
    public void Test_SavesCopyToDatabase()
    {
      Copy testCopy = new Copy(2, 1, 1);
      testCopy.Save();
      Book newBook = new Book("The Sun Also Rises", "1930s Spain", 1);
      newBook.Save();
      // testCopy.AddCopy(newBook);

      List<Copy> result = Copy.GetAll();
      List<Copy> testList = new List<Copy>{testCopy};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_FindCopyInDatabase()
    {
      Copy newCopy = new Copy(1, 1);
      newCopy.Save();

      Copy foundCopy = Copy.Find(newCopy.Id);

      Assert.Equal(newCopy, foundCopy);
    }

    [Fact]
    public void Test_AddCopyOfBookToDatabase()
    {
      Copy testCopy = new Copy(2, 1, 1);
      testCopy.Save();
      Book newBook = new Book("The Sun Also Rises", "1930s Spain", 1);
      newBook.Save();
      testCopy.AddCopy(newBook);

      List<Copy> result = Copy.GetAll();
      List<Copy> testList = new List<Copy>{testCopy};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_UpdateCopyQuantityInDatabase()
    {
      Copy testCopy = new Copy(2, 1);
      testCopy.Save();
      int newQuantity = (5);

      testCopy.UpdateQuantity(newQuantity);
      int result = testCopy.Quantity;

      Assert.Equal(newQuantity, result);
    }


    public void Dispose()
    {
      Book.DeleteAll();
      Author.DeleteAll();
      Book.DeleteAllInJoinTable();
      Copy.DeleteAll();
    }
  }
}
