using Xunit;
using System;
using System.Data;
using System.Collections.Generic;
using Catalog.Startup;
using Catalog.Objects;

namespace Catalog.Tests
{
  public class PatronTests : IDisposable
  {
    public void PatronTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=catalog_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DBReturnsEmptyAtFirst()
    {
      Assert.Equal(0, Patron.GetAll().Count);
    }

    [Fact]
    public void Test_OverloadedEquals()
    {
      Patron patron1 = new Patron("Ernest Hemmingway", "1234567890", "123 Fake Author Way");
      Patron patron2 = new Patron("Ernest Hemmingway", "1234567890", "123 Fake Author Way");
      Assert.Equal(patron1, patron2);
    }

    [Fact]
    public void Test_SavePatronToDatabase()
    {
      Patron testPatron = new Patron("Ayn Rand", "1234567890", "123 Fake Author Way");
      testPatron.Save();

      List<Patron> result = Patron.GetAll();
      List<Patron> testList = new List<Patron>{testPatron};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_FindPatronInDatabase()
    {
      Patron newPatron = new Patron("Leo Tolstoy", "1234567890", "123 Fake Author Way");
      newPatron.Save();

      Patron foundPatron = Patron.Find(newPatron.Id);

      Assert.Equal(newPatron, foundPatron);
    }

    [Fact]
    public void Test_UpdatePatronInDatabase()
    {
      string testName = "Steenbeck";
      Patron testPatron = new Patron(testName, "1234567890", "123 Fake Author Way");
      testPatron.Save();
      string newName = "Steinbeck";

      testPatron.Update(newName, "1234567890", "123 Fake Author Way");
      string result = testPatron.Name;

      Assert.Equal(newName, result);
    }

    public void Dispose()
    {
      Patron.DeleteAll();
    }
  }
}
