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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            Welcome.Text = "Welcome, " + storeAuthorization.name;
            storeAuthorization.LockIcon(this, storeAuthorization.role);
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "staff")
            {
                storeAuthorization.activeMenu("staff", panelMenus);
                storeAuthorization.PermissionNavigate("staff", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "sale")
            {
                storeAuthorization.activeMenu("sale", panelMenus);
                storeAuthorization.PermissionNavigate("sale", panelContainerForm);
            }

            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "product")
            {
                storeAuthorization.activeMenu("product", panelMenus);
                storeAuthorization.PermissionNavigate("product", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "customer")
            {
                storeAuthorization.activeMenu("customer", panelMenus);
                storeAuthorization.PermissionNavigate("customer", panelContainerForm);
            }

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult re = new DialogResult();
            re = MessageBox.Show("Do you want to logout account ?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (re == DialogResult.Yes)
            {
                this.Close();
                Main Logout = new Main();
                Logout.Show();
                storeAuthorization.name = "";
                storeAuthorization.role = "";
            }
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "staff")
            {
                storeAuthorization.activeMenu("staff", panelMenus);
                storeAuthorization.PermissionNavigate("staff", panelContainerForm);
            }
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "sale")
            {
                storeAuthorization.activeMenu("sale", panelMenus);
                storeAuthorization.PermissionNavigate("sale", panelContainerForm);
            }
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "product")
            {
                storeAuthorization.activeMenu("product", panelMenus);
                storeAuthorization.PermissionNavigate("product", panelContainerForm);
            }
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "customer")
            {
                storeAuthorization.activeMenu("customer", panelMenus);
                storeAuthorization.PermissionNavigate("customer", panelContainerForm);
            }
        }
    }
}
