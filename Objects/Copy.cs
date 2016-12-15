using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Catalog.Objects
{
    public class Copy
    {
        public int Id { get; set; }
        public int Quantity {get; set;}

        public Copy(int quantity = 0, int copyId = 0)
        {
            this.Id = copyId;
            this.Quantity = quantity;
        }

        public override bool Equals(System.Object otherCopy)
        {
            if (!(otherCopy is Copy))
            {
                return false;
            }
            else
            {
                Copy newCopy = (Copy)otherCopy;
                bool checkId = (this.Id == newCopy.Id);
                bool checkQuantity = (this.Quantity == newCopy.Quantity);
                return (checkId && checkQuantity);
            }
        }

        public override int GetHashCode()
        {
            return this.Quantity.GetHashCode();
        }

        public static List<Copy> GetAll()
        {
            List<Copy> allCopies = new List<Copy> { };

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM copies;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int copyId = rdr.GetInt32(0);
                int copyQuantity = rdr.GetInt32(1);
                Copy newCopy = new Copy(copyQuantity, copyId);
                allCopies.Add(newCopy);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return allCopies;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO copies (quantity) OUTPUT INSERTED.id VALUES(@CopyQuantity)", conn);

            SqlParameter quantityParam = new SqlParameter("@CopyQuantity", this.Quantity);
            cmd.Parameters.Add(quantityParam);

            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                this.Id = rdr.GetInt32(0);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }

        // public void AddCopy(Book newBook)
        // {
        //   SqlConnection conn = DB.Connection();
        //   conn.Open();
        //   SqlCommand cmd = new SqlCommand("INSERT INTO copies (book_id, quantity) VALUES (@BookId, @CopyQuantity);", conn);
        //   SqlParameter bookParam = new SqlParameter("@BookId", newBook.Id);
        //   SqlParameter copyQuantityParam = new SqlParameter("@CopyQuantity", this.Quantity);
        //   cmd.Parameters.Add(bookParam);
        //   cmd.Parameters.Add(copyQuantityParam);
        //   cmd.ExecuteNonQuery();
        //
        //   if (conn != null)
        //   {
        //     conn.Close();
        //   }
        // }

        // public static List<Book> SearchByTitle(string bookTitle)
        // {
        //   SqlConnection conn = DB.Connection();
        //   conn.Open();
        //
        //   SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE title = (@BookTitle);", conn);
        //
        //   SqlParameter titleParameter = new SqlParameter("@BookTitle", bookTitle);
        //   cmd.Parameters.Add(titleParameter);
        //
        //   SqlDataReader rdr = cmd.ExecuteReader();
        //   List<Book> searchBook = new List<Book>{};
        //   while (rdr.Read())
        //   {
        //     int searchId = rdr.GetInt32(0);
        //     string searchTitle = rdr.GetString(1);
        //     string searchDescription = rdr.GetString(2);
        //     Book bookSearch = new Book(searchTitle, searchDescription, searchId);
        //     searchBook.Add(bookSearch);
        //   }
        //   if (rdr != null)
        //   {
        //     rdr.Close();
        //   }
        //   if (conn != null)
        //   {
        //     conn.Close();
        //   }
        //   return searchBook;
        // }
        //
        // public static List<Book> SearchByAuthor(string author)
        // {
        //   SqlConnection conn = DB.Connection();
        //   conn.Open();
        //
        //   SqlCommand cmd = new SqlCommand((@"SELECT books.id, title, description FROM
        //                                       authors_books JOIN books ON (authors_books.book_id = books.id)
        //                                                     JOIN authors ON (authors.id = authors_books.author_id)
        //                                                     WHERE authors.name = @AuthorName;"), conn);
        //   SqlParameter authorParam = new SqlParameter("@AuthorName", author);
        //   cmd.Parameters.Add(authorParam);
        //
        //   SqlDataReader rdr = cmd.ExecuteReader();
        //
        //   List<Book> bookResults = new List<Book>();
        //   while (rdr.Read())
        //   {
        //     int id = rdr.GetInt32(0);
        //     string title = rdr.GetString(1);
        //     string desc = rdr.GetString(2);
        //     Book book = new Book(title, desc, id);
        //     bookResults.Add(book);
        //   }
        //   if (rdr != null)
        //   {
        //     rdr.Close();
        //   }
        //   if (conn != null)
        //   {
        //     rdr.Close();
        //   }
        //   return bookResults;
        // }
        //
        // public static Book Find(int id)
        // {
        //     SqlConnection conn = DB.Connection();
        //     conn.Open();
        //
        //     SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE id = (@BookId);", conn);
        //
        //     SqlParameter idParam = new SqlParameter("@BookId", id.ToString());
        //     cmd.Parameters.Add(idParam);
        //
        //     SqlDataReader rdr = cmd.ExecuteReader();
        //
        //     int foundBookId = 0;
        //     string foundBookTitle = null;
        //     string foundBookDesc = "";
        //
        //     while (rdr.Read())
        //     {
        //         foundBookId = rdr.GetInt32(0);
        //         foundBookTitle = rdr.GetString(1);
        //         foundBookDesc = rdr.GetString(2);
        //     }
        //     Book newBook = new Book(foundBookTitle, foundBookDesc, foundBookId);
        //
        //     if (rdr != null)
        //     {
        //         rdr.Close();
        //     }
        //     if (conn != null)
        //     {
        //         conn.Close();
        //     }
        //
        //     return newBook;
        // }
        //
        // public void Update(string newTitle, string newDescription)
        // {
        //     SqlConnection conn = DB.Connection();
        //     conn.Open();
        //
        //     SqlCommand cmd = new SqlCommand("UPDATE books SET title = @newTitle, description = @newDescription OUTPUT INSERTED.title, INSERTED.description WHERE id = @BookId;", conn);
        //
        //     SqlParameter newTitleParam = new SqlParameter("@newTitle", newTitle);
        //     SqlParameter newDescParam = new SqlParameter("@newDescription", newDescription);
        //     SqlParameter idParam = new SqlParameter("@BookId", this.Id);
        //
        //     cmd.Parameters.Add(newTitleParam);
        //     cmd.Parameters.Add(newDescParam);
        //     cmd.Parameters.Add(idParam);
        //
        //     SqlDataReader rdr = cmd.ExecuteReader();
        //
        //     while (rdr.Read())
        //     {
        //         this.Title = rdr.GetString(0);
        //         this.Description = rdr.GetString(1);
        //     }
        //     if (rdr != null)
        //     {
        //         rdr.Close();
        //     }
        //     if (conn != null)
        //     {
        //         conn.Close();
        //     }
        // }

        // public void Delete()
        // {
        //     SqlConnection conn = DB.Connection();
        //     conn.Open();
        //
        //     SqlCommand cmd = new SqlCommand("DELETE FROM copies WHERE id = @CopyId", conn);
        //     SqlParameter idParam = new SqlParameter("@CopyId", this.Id);
        //     cmd.Parameters.Add(idParam);
        //
        //     cmd.ExecuteNonQuery();
        //
        //     if (conn != null)
        //     {
        //         conn.Close();
        //     }
        // }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM copies;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
