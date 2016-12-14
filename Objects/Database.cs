using System.Data;
using System.Data.SqlClient;
using Catalog.Startup;

namespace Catalog
{
  public class DB
  {
    public static SqlConnection Connection()
    {
      SqlConnection conn = new SqlConnection(DBConfiguration.ConnectionString);
      return conn;
    }
  }
}
