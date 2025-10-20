using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagementSystem
{
    public partial class Product : Form
    {
        public Product()
        {
            InitializeComponent();
        }
        SqlDataAdapter DA;
        DataTable TB;
        SqlCommand com;
        byte[] Photo;
        string filepath;
        bool isCreateUPdate = false;



        private void btnBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "JPEG FILE|*.jpg; *.jpeg |PNG FILE|*.png";
            fd.Title = "Open an image...";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                filepath = fd.FileName;
                imageProduct.Image = Image.FromFile(filepath);
            }
        }
        private void loadData()
        {
            DA = new SqlDataAdapter("select * from ViewProductss ORDER BY ID DESC", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            dgvProduct.DataSource = TB;
            dgvProduct.ColumnHeadersHeight = 40;
            dgvProduct.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvProduct.RowTemplate.Height = 100;
            foreach (DataGridViewRow row in dgvProduct.Rows)
            {
                row.Height = 100;
            }
            dgvProduct.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            dgvProduct.DefaultCellStyle.Font = new Font("Khmer os system", 12);
            dgvProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProduct.Columns["ID"].Visible = false;
            dgvProduct.Columns["Image"].Width = 100;
            dgvProduct.Columns["Unit Price"].DefaultCellStyle.Format = "c";
            DataGridViewImageColumn img = new DataGridViewImageColumn();
            img = (DataGridViewImageColumn)dgvProduct.Columns["Image"];
            img.ImageLayout = DataGridViewImageCellLayout.Stretch;




            foreach (DataGridViewColumn col in dgvProduct.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


        }
        private void Product_Load(object sender, EventArgs e)
        {
            txtName.Focus();
            txtSearch.Text = "Search product hear...";
            txtSearch.ForeColor = Color.Gray;
            loadData();
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search product hear...")
            {
                txtSearch.ForeColor = Color.Black;
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                txtSearch.Text = "Search product hear...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("SearchProductByName", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.AddWithValue("@ProductName", txtSearch.Text.Trim());
            TB = new DataTable();
            DA.Fill(TB);
            dgvProduct.DataSource = TB;
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isCreateUPdate)
            {
                if (e.RowIndex >= 0) // Make sure it's not a header
                {
                    DataGridViewRow row = dgvStaff.Rows[e.RowIndex];
                    var salary = decimal.Parse(row.Cells["Salary"].Value.ToString(), NumberStyles.Currency);
                    txtNameKH.Tag = row.Cells["StaffID"].Value.ToString();
                    txtNameKH.Text = row.Cells["Name KH"].Value.ToString();
                    txtNameEN.Text = row.Cells["Name EN"].Value.ToString();
                    if (row.Cells["Gender"].Value.ToString().Trim() == "Male")
                    {
                        cbxGender.SelectedIndex = 0;
                    }
                    else
                    {
                        cbxGender.SelectedIndex = 1;
                    }
                    txtPhnoe.Text = row.Cells["Phone"].Value.ToString();
                    dpDOB.Text = row.Cells["DOB"].Value.ToString();
                    txtVillage.Text = row.Cells["Village"].Value.ToString();
                    txtSongkat.Text = row.Cells["Sangkat_Khum"].Value.ToString();
                    txtKhan.Text = row.Cells["Khan_Srok"].Value.ToString();
                    txtCity.Text = row.Cells["Province_City"].Value.ToString();
                    txtEmail.Text = row.Cells["Email"].Value.ToString();
                    txtPassword.Text = row.Cells["Password"].Value.ToString();
                    cbxRole.SelectedValue = int.Parse(row.Cells["RoleID"].Value.ToString());

                    txtSalary.Text = salary.ToString("N2");
                    Photo = (byte[])row.Cells["Image"].Value;
                    MemoryStream ms = new MemoryStream(Photo);
                    imageStaff.Image = Image.FromStream(ms);

                }
            }
        }
    }
}
