using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Catalog.Objects
{
  public class Patron
  {
    public int Id {get;set;}
    public string Name {get;set;}
    public string Phone {get;set;}
    public string Address {get;set;}

    public Patron (string name, string phone, string address, int id = 0)
    {
      this.Id = id;
      this.Name = name;
      this.Phone = phone;
      this.Address = address;
    }

    public override bool Equals(System.Object otherPatron)
    {
        if (!(otherPatron is Patron))
        {
            return false;
        }
        else
        {
            Patron newPatron = (Patron)otherPatron;
            bool checkId = (this.Id == newPatron.Id);
            bool checkName = (this.Name == newPatron.Name);
            bool checkPhone = (this.Phone == newPatron.Phone);
            bool checkAddress = (this.Address == newPatron.Address);
            return (checkId && checkName && checkPhone && checkAddress);
        }
    }

    public override int GetHashCode()
    {
        return this.Name.GetHashCode();
    }

    public static List<Patron> GetAll()
    {
        List<Patron> allPatrons = new List<Patron> { };

        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM patrons;", conn);
        SqlDataReader rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            int patronId = rdr.GetInt32(0);
            string patronName = rdr.GetString(1);
            string patronPhone = rdr.GetString(2);
            string patronAddress = rdr.GetString(3);
            Patron newPatron = new Patron(patronName, patronPhone, patronAddress, patronId);
            allPatrons.Add(newPatron);
        }

        if (rdr != null)
        {
            rdr.Close();
        }
        if (conn != null)
        {
            conn.Close();
        }

        return allPatrons;
    }

    public void Save()
    {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("INSERT INTO patrons (name, phone, address) OUTPUT INSERTED.id VALUES(@PatronName, @PatronPhone, @PatronAddress)", conn);

        SqlParameter nameParam = new SqlParameter("@PatronName", this.Name);
        cmd.Parameters.Add(nameParam);

        SqlParameter phoneParam = new SqlParameter("@PatronPhone", this.Phone);
        cmd.Parameters.Add(phoneParam);

        SqlParameter addressParam = new SqlParameter("@PatronAddress", this.Address);
        cmd.Parameters.Add(addressParam);

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

    public static List<Patron> SearchByName(string patronName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE name = (@PatronName);", conn);

      SqlParameter nameParameter = new SqlParameter("@PatronName", patronName);
      cmd.Parameters.Add(nameParameter);

      SqlDataReader rdr = cmd.ExecuteReader();
      List<Patron> searchPatron = new List<Patron>{};
      while (rdr.Read())
      {
        int searchId = rdr.GetInt32(0);
        string searchName = rdr.GetString(1);
        string searchPhone = rdr.GetString(2);
        string searchAddress = rdr.GetString(3);
        Patron patronSearch = new Patron(searchName, searchPhone, searchAddress, searchId);
        searchPatron.Add(patronSearch);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return searchPatron;
    }

    public static Patron Find(int id)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE id = (@PatronId);", conn);

        SqlParameter idParam = new SqlParameter("@PatronId", id.ToString());
        cmd.Parameters.Add(idParam);

        SqlDataReader rdr = cmd.ExecuteReader();

        int foundPatronId = 0;
        string foundPatronName = null;
        string foundPatronPhone = "";
        string foundPatronAddress = "";

        while (rdr.Read())
        {
            foundPatronId = rdr.GetInt32(0);
            foundPatronName = rdr.GetString(1);
            foundPatronPhone = rdr.GetString(2);
            foundPatronAddress = rdr.GetString(3);
        }
        Patron newPatron = new Patron(foundPatronName, foundPatronPhone, foundPatronAddress, foundPatronId);

        if (rdr != null)
        {
            rdr.Close();
        }
        if (conn != null)
        {
            conn.Close();
        }

        return newPatron;
    }

    public void Update(string newName, string newPhone, string newAddress)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("UPDATE patrons SET name = @newName, phone = @newPhone, address = @newAddress OUTPUT INSERTED.name, INSERTED.phone, INSERTED.address WHERE id = @PatronId;", conn);

        SqlParameter newNameParam = new SqlParameter("@newName", newName);
        SqlParameter newPhoneParam = new SqlParameter("@newPhone", newPhone);
        SqlParameter newAddressParam = new SqlParameter("@newAddress", newAddress);
        SqlParameter idParam = new SqlParameter("@PatronId", this.Id);

        cmd.Parameters.Add(newNameParam);
        cmd.Parameters.Add(newPhoneParam);
        cmd.Parameters.Add(newAddressParam);
        cmd.Parameters.Add(idParam);

        SqlDataReader rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            this.Name = rdr.GetString(0);
            this.Phone = rdr.GetString(1);
            this.Address = rdr.GetString(2);
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

        SqlCommand cmd = new SqlCommand("DELETE FROM patrons WHERE id = @PatronId", conn);
        SqlParameter idParam = new SqlParameter("@PatronId", this.Id);
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
        SqlCommand cmd = new SqlCommand("DELETE FROM patrons;", conn);
        cmd.ExecuteNonQuery();
        conn.Close();
    }

  }
}
