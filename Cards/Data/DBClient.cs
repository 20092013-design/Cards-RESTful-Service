using Microsoft.Data.SqlClient;
using System.Data;

namespace Cards.Data
{
    public class DBClient
    {
        public static IDbConnection GetInstance()
        {
          IDbConnection _db = new SqlConnection("Data Source=DESKTOP-L5SHLVS;Initial Catalog=Cards;Integrated Security=True;Encrypt=False");
            return _db;
        }
    }
}
