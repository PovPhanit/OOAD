using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagementSystem
{
    internal class Operation
    {
        public static SqlConnection con;
        public static void myConnection()
        {
            string conStr = @"Data source=MSI168\SQLSERVER;Initial catalog=sale_management_system;Integrated Security=true";
            con = new SqlConnection(conStr);
            con.Open();
        }
    }
}
