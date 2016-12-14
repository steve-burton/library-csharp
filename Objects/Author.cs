using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Catalog.Objects
{
    public class Author
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }

        public Author(string authorName, int Id = 0)
        {
            this.AuthorName = authorName;
            this.Id = Id;
        }

        public override bool Equals(System.Object otherAuthor)
        {
            if (!(otherAuthor is Author))
            {
                return false;
            }
            else
            {
                Author newAuthor = (Author)otherAuthor;
                bool checkId = (this.Id == newAuthor.Id);
                bool checkAuthorName = (this.AuthorName == newAuthor.AuthorName);
                return (checkId && checkAuthorName);
            }
        }

        public override int GetHashCode()
        {
            return this.AuthorName.GetHashCode();
        }

        public static List<Author> GetAll()
        {
            List<Author> allAuthors = new List<Author> { };

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM authors;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int authorId = rdr.GetInt32(0);
                string authorName = rdr.GetString(1);
                Author newAuthor = new Author(authorName, authorId);
                allAuthors.Add(newAuthor);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return allAuthors;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO authors (name) OUTPUT INSERTED.id VALUES(@AuthorName)", conn);

            SqlParameter nameParam = new SqlParameter();
            nameParam.ParameterName = "@AuthorName";
            nameParam.Value = this.AuthorName;
            cmd.Parameters.Add(nameParam);

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

        public static Author Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM authors WHERE id = (@AuthorId)", conn);

            SqlParameter idParam = new SqlParameter();
            idParam.ParameterName = "@AuthorId";
            idParam.Value = id.ToString();
            cmd.Parameters.Add(idParam);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundAuthorId = 0;
            string foundAuthorName = null;

            while (rdr.Read())
            {
                foundAuthorId = rdr.GetInt32(0);
                foundAuthorName = rdr.GetString(1);
            }
            Author newAuthor = new Author(foundAuthorName, foundAuthorId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return newAuthor;
        }

        public void Update(string newAuthorName)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE authors SET name = @newAuthorName OUTPUT INSERTED.name WHERE id = @AuthorId;", conn);

            SqlParameter newAuthorNameParam = new SqlParameter();
            newAuthorNameParam.ParameterName = "@newAuthorName";
            newAuthorNameParam.Value = newAuthorName;
            cmd.Parameters.Add(newAuthorNameParam);

            SqlParameter idParam = new SqlParameter();
            idParam.ParameterName = "@AuthorId";
            idParam.Value = this.Id;
            cmd.Parameters.Add(idParam);

            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                this.AuthorName = rdr.GetString(0);
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

            SqlCommand cmd = new SqlCommand("DELETE FROM authors WHERE id = @AuthorId", conn);
            SqlParameter idParam = new SqlParameter();
            idParam.ParameterName = "@AuthorId";
            idParam.Value = this.Id;
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
            SqlCommand cmd = new SqlCommand("DELETE FROM authors;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
