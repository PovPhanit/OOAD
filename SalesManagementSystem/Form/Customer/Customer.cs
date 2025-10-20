using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SalesManagementSystem
{
    public partial class Customer : Form
    {
        public Customer()
        {
            InitializeComponent();
        }
        SqlDataAdapter DA;
        DataTable TB;
        SqlCommand com;
        bool isCreateUPdate = false;

        private void loadData()
        {
            DA = new SqlDataAdapter("select * from ViewCustomerss order by CustomerId desc", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);

            dgvCustomer.DataSource = TB;

            dgvCustomer.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            dgvCustomer.DefaultCellStyle.Font = new Font("Khmer os system", 12);
            dgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvCustomer.Columns["CustomerId"].Visible = false;

            foreach (DataGridViewColumn col in dgvCustomer.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void Customer_Load(object sender, EventArgs e)
        {
            txtName.Focus();
            txtSearch.Text = "Search customer here...";
            txtSearch.ForeColor = Color.Gray;
            loadData();
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isCreateUPdate && e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomer.Rows[e.RowIndex];

                txtCode.Text = row.Cells["CustomerID"].Value.ToString();
                txtName.Text = row.Cells["FullName"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();

            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search customer here...")
            {
                txtSearch.ForeColor = Color.Black;
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                txtSearch.Text = "Search customer here...";
                txtSearch.ForeColor = Color.Gray;
            }
        }
        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            using (SqlCommand cmd = new SqlCommand("SearchCustomer", Operation.con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Keyword", txtSearch.Text);

                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DataTable TB = new DataTable();
                DA.Fill(TB);

                dgvCustomer.DataSource = TB;

                dgvCustomer.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
                dgvCustomer.DefaultCellStyle.Font = new Font("Khmer os system", 12);
                dgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dgvCustomer.Columns["CustomerId"].Visible = false;

                foreach (DataGridViewColumn col in dgvCustomer.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
        }



        private void btnEdit_Click(object sender, EventArgs e)
        {
            isCreateUPdate = false;
            btnNew.BackColor = Color.IndianRed;
            btnNew.Image = SalesManagementSystem.Properties.Resources.Cancel;
            btnNew.Text = "បោះបង់";
            txtName.Focus();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បង្កើតថ្មី")
            {
                btnNew.BackColor = Color.IndianRed;
                btnNew.Image = SalesManagementSystem.Properties.Resources.Cancel;
                btnNew.Text = "បោះបង់";
                ControlForm.ClearData(this);
                txtSearch.Text = "Search staff hear...";
                txtSearch.ForeColor = Color.Gray;
                isCreateUPdate = true;
            }
            else
            {
                DialogResult re = new DialogResult();
                re = MessageBox.Show("Do you want to cancel it ?", "Cancel", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (re == DialogResult.OK)
                {
                    btnNew.BackColor = Color.MidnightBlue;
                    btnNew.Image = SalesManagementSystem.Properties.Resources.Add;
                    btnNew.Text = "បង្កើតថ្មី";
                    isCreateUPdate = false;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBox.Show("Please enter Name...", "Missing",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                MessageBox.Show("Please enter phone...", "Missing",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtAddress.Text.Trim()))
            {
                MessageBox.Show("Please enter address...", "Missing",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return;
            }




            if (btnNew.Text == "បោះបង់")
            {

                if (isCreateUPdate)
                {
                    // INSERT Document
                    using (SqlCommand com = new SqlCommand("AddCustomer", Operation.con))
                    {

                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@FullName", txtName.Text.Trim());
                        com.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        com.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());

                        com.ExecuteNonQuery();
                        txtName.Text = "";
                        txtPhone.Text = "";
                        txtAddress.Text = "";
                    }
                }
                else
                {
                    // UPDATE Document
                    using (SqlCommand com = new SqlCommand("UpdateCustomer", Operation.con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@CustomerId", Convert.ToInt32(txtCode.Text));
                        com.Parameters.AddWithValue("@FullName", txtName.Text.Trim());
                        com.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        com.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());

                        com.ExecuteNonQuery();
                    }
                }
                loadData();

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtCode.Text == null || string.IsNullOrEmpty(txtCode.Text.ToString().Trim()))
            {
                MessageBox.Show("Please select a list item to delete", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult re = new DialogResult();
            re = MessageBox.Show("Do you want to delete it ?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (re == DialogResult.OK)
            {
                SqlCommand com = new SqlCommand("DeleteCustomer", Operation.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustomerId", Convert.ToInt32(txtCode.Text));
                int rowEffect = com.ExecuteNonQuery();

                // Clear and refresh UI
                txtName.Text = "";
                txtPhone.Text = "";
                txtAddress.Text = "";
                loadData();
            }
        }




    }
}
