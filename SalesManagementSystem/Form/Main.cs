using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagementSystem
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            var (staffname, role, id) = storeAuthorization.AuthenticateUser(username, password);
            if (role != null)
            {
                this.Hide();
                storeAuthorization.name = staffname;
                storeAuthorization.role = role;
                storeAuthorization.id = id;
                if (storeAuthorization.role.ToLower() == "product")
                {
                    new Product().Show();
                }
                else if (storeAuthorization.role.ToLower() == "customer")
                {
                    new Customer().Show();
                }
                else if (storeAuthorization.role.ToLower() == "employee")
                {
                    new Employee().Show();
                }
                else if (storeAuthorization.role.ToLower() == "sale")
                {
                    new Sale().Show();
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
