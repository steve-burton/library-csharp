using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Catalog.Objects
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description {get; set;}

        public Book(string bookTitle, string bookDesc, int bookId = 0)
        {
            this.Id = bookId;
            this.Title = bookTitle;
            this.Description = bookDesc;
        }

        public override bool Equals(System.Object otherBook)
        {
            if (!(otherBook is Book))
            {
                return false;
            }
            else
            {
                Book newBook = (Book)otherBook;
                bool checkId = (this.Id == newBook.Id);
                bool checkTitle = (this.Title == newBook.Title);
                bool checkDesc = (this.Description == newBook.Description);
                return (checkId && checkTitle && checkDesc);
            }
        }

        public override int GetHashCode()
        {
            return this.Title.GetHashCode();
        }

        public static List<Book> GetAll()
        {
            List<Book> allBooks = new List<Book> { };

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM books;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int bookId = rdr.GetInt32(0);
                string bookTitle = rdr.GetString(1);
                string bookDescription = rdr.GetString(2);
                Book newBook = new Book(bookTitle, bookDescription, bookId);
                allBooks.Add(newBook);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return allBooks;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO books (title, description) OUTPUT INSERTED.id VALUES(@BookTitle, @BookDesc)", conn);

            SqlParameter nameParam = new SqlParameter("@BookTitle", this.Title);
            cmd.Parameters.Add(nameParam);

            SqlParameter descParam = new SqlParameter("@BookDesc", this.Description);
            cmd.Parameters.Add(descParam);

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

        public static Book Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE id = (@BookId)", conn);

            SqlParameter idParam = new SqlParameter("@BookId", id.ToString());
            cmd.Parameters.Add(idParam);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundBookId = 0;
            string foundBookTitle = null;
            string foundBookDesc = "";

            while (rdr.Read())
            {
                foundBookId = rdr.GetInt32(0);
                foundBookTitle = rdr.GetString(1);
                foundBookDesc = rdr.GetString(2);
            }
            Book newBook = new Book(foundBookTitle, foundBookDesc, foundBookId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return newBook;
        }

        public void Update(string newTitle, string newDescription)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE books SET title = @newTitle, description = @newDescription OUTPUT INSERTED.title, INSERTED.description WHERE id = @BookId;", conn);

            SqlParameter newTitleParam = new SqlParameter("@newTitle", newTitle);
            SqlParameter newDescParam = new SqlParameter("@newDescription", newDescription);
            SqlParameter idParam = new SqlParameter("@BookId", this.Id);

            cmd.Parameters.Add(newTitleParam);
            cmd.Parameters.Add(newDescParam);
            cmd.Parameters.Add(idParam);

            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                this.Title = rdr.GetString(0);
                this.Description = rdr.GetString(1);
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

        public void Delete()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM books WHERE id = @BookId", conn);
            SqlParameter idParam = new SqlParameter("@BookId", this.Id);
            cmd.Parameters.Add(idParam);

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM books;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
