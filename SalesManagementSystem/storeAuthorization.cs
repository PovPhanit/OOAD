using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SalesManagementSystem
{
    internal class storeAuthorization
    {
        public static String name { get; set; }
        public static String role { get; set; }
        public static int id { get; set; }
        public static void PermissionNavigate(String navigate, Panel panelContainer)
        {
            panelContainer.Controls.Clear();
            Form childForm = null;
            if ((role.ToLower() == "admin" || role.ToLower() == "staff") && navigate == "staff")
            {
                childForm = new Employee();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "sale") && navigate == "sale")
            {
                childForm = new Sale();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "product") && navigate == "product")
            {
                childForm = new Product();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "customer") && navigate == "customer")
            {
                childForm = new Customer();
            }
           
           
            childForm.TopLevel = false;
            childForm.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(childForm);
            childForm.Show();
        }

        public static void activeMenu(String typeMenu, Panel panelMenu)
        {
            foreach (Control buttonMenu in panelMenu.Controls)
            {
                if (buttonMenu.GetType() == typeof(Button))
                {

                    if (buttonMenu.Tag?.ToString() == typeMenu)
                    {
                        buttonMenu.BackColor = Color.DarkGray;
                    }
                    else
                    {
                        buttonMenu.BackColor = Color.MidnightBlue;
                    }



                }
            }
        }

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
        public static void LockIcon(Form frm, string locks)
        {
            ShowTags(frm.Controls, locks);
        }

        private static void ShowTags(Control.ControlCollection controls, string locks)
        {
            foreach (Control ct in controls)
            {
                if (ct is PictureBox)
                {
                    if (ct.Tag?.ToString().StartsWith("Lock") == true)
                    {

                        if (ct.Tag?.ToString().ToLower() == "lock." + locks.ToLower() || locks.ToLower() == "admin")
                        {
                            ct.Visible = false;
                        }
                        else
                        {
                            ct.Visible = true;
                        }
                    }
                }
                // Recursively check child controls
                if (ct.HasChildren)
                {
                    ShowTags(ct.Controls, locks);
                }
            }
        }
    }
}
