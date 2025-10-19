using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagementSystem
{
    internal class storeAuthorization
    {
        public static String name { get; set; }
        public static String role { get; set; }
        public static int id { get; set; }
        public static (string Username, string Role, int id) AuthenticateUser(string username, string password)
        {
            Operation.myConnection();
            string query = "select StaffID, RoleType,StaffNameEN from Staff where StaffPassword =@password and StaffEmail =@username ";
            using (SqlCommand cmd = new SqlCommand(query, Operation.con))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string role = reader["RoleType"].ToString();
                        string name = reader["StaffNameEN"].ToString();
                        int id = Convert.ToInt32(reader["StaffID"].ToString());
                        return (name, role, id);
                    }
                    else
                    {
                        return (null, null, 0);
                    }
                }
            }

        }
    }
}
