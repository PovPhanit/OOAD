using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagementSystem.Database
{
    public class DbConnection
    {
        private static string connectionString =
            @"Data Source=MSI168\SQLSERVER;Initial Catalog=OOAD;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
