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

        private void DisplayProducts()
        {
            DA = new SqlDataAdapter("SELECT * FROM ViewProductss ORDER BY ID DESC", Operation.con);
            TB = new DataTable();
            Image defaultImage = Image.FromFile("../../Resources/roomDisplay.png"); 
            DA.Fill(TB);
            productContainer.Controls.Clear();

            // Create FlowLayoutPanel
            FlowLayoutPanel flow = new FlowLayoutPanel();
            flow.Dock = DockStyle.Fill;
            flow.WrapContents = true;
            flow.AutoScroll = true;
            flow.FlowDirection = FlowDirection.LeftToRight;
            flow.Padding = new Padding(3);

            foreach (DataRow dr in TB.Rows)
            {
                Panel itemPanel = new Panel();
                itemPanel.Size = new Size(200, 250);
                itemPanel.Margin = new Padding(5);
                itemPanel.BackColor = Color.White;
                itemPanel.BorderStyle = BorderStyle.Fixed3D;


                // Product Image
                PictureBox pic = new PictureBox();
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Size = new Size(150, 150);
                pic.Location = new Point(25, 10);

                if (dr["Image"] != DBNull.Value)
                {
                    byte[] imageData = (byte[])dr["Image"];
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        pic.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pic.Image = defaultImage;
                }

                // Product Name Label
                Label lblProductName = new Label();
                lblProductName.Text = dr["Product Name"].ToString();
                lblProductName.ForeColor = Color.Black;
                lblProductName.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblProductName.TextAlign = ContentAlignment.MiddleCenter;
                lblProductName.Size = new Size(180, 25);
                lblProductName.Location = new Point(10, 170);

                // Price Label
                Label lblPrice = new Label();
                decimal price = decimal.Parse(dr["Unit Price"].ToString());
                lblPrice.Text = price.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
                lblPrice.ForeColor = Color.Red;
                lblPrice.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblPrice.TextAlign = ContentAlignment.MiddleCenter;
                lblPrice.Size = new Size(180, 20);
                lblPrice.Location = new Point(10, 195);

                // Stock Label
                Label lblStock = new Label();
                lblStock.Text = $"In Stock: {dr["In Stock"]}";
                lblStock.ForeColor = Color.Green;
                lblStock.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                lblStock.TextAlign = ContentAlignment.MiddleCenter;
                lblStock.Size = new Size(180, 20);
                lblStock.Location = new Point(10, 215);

                // Click events for selection
                if (!isCreateUPdate)
                {
                    pic.Click += (s, e1) => SelectProduct(dr);
                    lblProductName.Click += (s, e2) => SelectProduct(dr);
                    lblPrice.Click += (s, e3) => SelectProduct(dr);
                    lblStock.Click += (s, e4) => SelectProduct(dr);
                    itemPanel.Click += (s, e5) => SelectProduct(dr);
                }

                itemPanel.Controls.Add(pic);
                itemPanel.Controls.Add(lblProductName);
                itemPanel.Controls.Add(lblPrice);
                itemPanel.Controls.Add(lblStock);
                flow.Controls.Add(itemPanel);
            }

            // Add flow panel to the main panel
            productContainer.Controls.Add(flow);
        }

        private void SelectProduct(DataRow dr)
        {
            txtCode.Text = dr["ID"].ToString();
            txtName.Text = dr["Product Name"].ToString();
            txtPrice.Text = decimal.Parse(dr["Unit Price"].ToString()).ToString("N2");
            txtQuantity.Text = dr["In Stock"].ToString();

            // Handle image
            if (dr["Image"] != DBNull.Value)
            {
                Photo = (byte[])dr["Image"];
                MemoryStream ms = new MemoryStream(Photo);
                imageProduct.Image = Image.FromStream(ms);
            }
            else
            {
                imageProduct.Image = null;
                Photo = null;
            }
        }

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
            DisplayProducts(); // Load the product display
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
            using (SqlCommand cmd = new SqlCommand("SearchProductByName", Operation.con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductName", txtSearch.Text.Trim());

                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DataTable TB = new DataTable();
                DA.Fill(TB);

                dgvProduct.DataSource = TB;
                DisplayFilteredProducts(TB); // Update the product display with search results

                dgvProduct.ColumnHeadersHeight = 40;
                dgvProduct.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgvProduct.RowTemplate.Height = 100;

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
        }

        private void DisplayFilteredProducts(DataTable filteredTable)
        {
            productContainer.Controls.Clear();
            Image defaultImage = Image.FromFile("../../Resources/roomDisplay.png");

            // Create FlowLayoutPanel
            FlowLayoutPanel flow = new FlowLayoutPanel();
            flow.Dock = DockStyle.Fill;
            flow.WrapContents = true;
            flow.AutoScroll = true;
            flow.FlowDirection = FlowDirection.LeftToRight;
            flow.Padding = new Padding(3);

            foreach (DataRow dr in filteredTable.Rows)
            {
                Panel itemPanel = new Panel();
                itemPanel.Size = new Size(200, 250);
                itemPanel.Margin = new Padding(5);
                itemPanel.BorderStyle = BorderStyle.FixedSingle;
                itemPanel.BackColor = Color.White;

                // Product Image
                PictureBox pic = new PictureBox();
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Size = new Size(150, 150);
                pic.Location = new Point(25, 10);

                if (dr["Image"] != DBNull.Value)
                {
                    byte[] imageData = (byte[])dr["Image"];
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        pic.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pic.Image = defaultImage;
                }

                // Product Name Label
                Label lblProductName = new Label();
                lblProductName.Text = dr["Product Name"].ToString();
                lblProductName.ForeColor = Color.Black;
                lblProductName.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblProductName.TextAlign = ContentAlignment.MiddleCenter;
                lblProductName.Size = new Size(180, 25);
                lblProductName.Location = new Point(10, 170);

                // Price Label
                Label lblPrice = new Label();
                decimal price = decimal.Parse(dr["Unit Price"].ToString());
                lblPrice.Text = price.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
                lblPrice.ForeColor = Color.Red;
                lblPrice.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblPrice.TextAlign = ContentAlignment.MiddleCenter;
                lblPrice.Size = new Size(180, 20);
                lblPrice.Location = new Point(10, 195);

                // Stock Label
                Label lblStock = new Label();
                lblStock.Text = $"In Stock: {dr["In Stock"]}";
                lblStock.ForeColor = Color.Green;
                lblStock.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                lblStock.TextAlign = ContentAlignment.MiddleCenter;
                lblStock.Size = new Size(180, 20);
                lblStock.Location = new Point(10, 215);

                // Click events for selection
                if (!isCreateUPdate)
                {
                    pic.Click += (s, e1) => SelectProduct(dr);
                    lblProductName.Click += (s, e2) => SelectProduct(dr);
                    lblPrice.Click += (s, e3) => SelectProduct(dr);
                    lblStock.Click += (s, e4) => SelectProduct(dr);
                    itemPanel.Click += (s, e5) => SelectProduct(dr);
                }

                itemPanel.Controls.Add(pic);
                itemPanel.Controls.Add(lblProductName);
                itemPanel.Controls.Add(lblPrice);
                itemPanel.Controls.Add(lblStock);
                flow.Controls.Add(itemPanel);
            }

            productContainer.Controls.Add(flow);
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isCreateUPdate && e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];

                txtCode.Text = row.Cells["ID"].Value.ToString();
                txtName.Text = row.Cells["Product Name"].Value.ToString();
                txtPrice.Text = decimal.Parse(row.Cells["Unit Price"].Value.ToString()).ToString("N2");
                txtQuantity.Text = row.Cells["In Stock"].Value.ToString();

                // Handle image
                if (row.Cells["Image"].Value != DBNull.Value)
                {
                    Photo = (byte[])row.Cells["Image"].Value;
                    MemoryStream ms = new MemoryStream(Photo);
                    imageProduct.Image = Image.FromStream(ms);
                }
                else
                {
                    imageProduct.Image = null;
                    Photo = null;
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បង្កើតថ្មី")
            {
                btnNew.BackColor = Color.IndianRed;
                btnNew.Image = SalesManagementSystem.Properties.Resources.Cancel;
                btnNew.Text = "បោះបង់";
                ControlForm.ClearData(this);
                txtSearch.Text = "Search product hear...";
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            isCreateUPdate = false;
            btnNew.BackColor = Color.IndianRed;
            btnNew.Image = SalesManagementSystem.Properties.Resources.Cancel;
            btnNew.Text = "បោះបង់";
            txtName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBox.Show("Please enter product name...", "Missing",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtPrice.Text.Trim()) || !decimal.TryParse(txtPrice.Text.Trim(), out decimal price))
            {
                MessageBox.Show("Please enter valid price...", "Missing",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtQuantity.Text.Trim()) || !int.TryParse(txtQuantity.Text.Trim(), out int quantity))
            {
                MessageBox.Show("Please enter valid quantity...", "Missing",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(filepath))
            {
                Photo = File.ReadAllBytes(filepath);
            }

            if (btnNew.Text == "បោះបង់")
            {
                if (isCreateUPdate)
                {
                    // INSERT Product
                    using (SqlCommand com = new SqlCommand("CreateProduct", Operation.con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ProductName", txtName.Text.Trim());
                        com.Parameters.AddWithValue("@Price", decimal.Parse(txtPrice.Text.Trim()));
                        com.Parameters.AddWithValue("@Quantity", int.Parse(txtQuantity.Text.Trim()));

                        if (Photo != null && Photo.Length > 0)
                        {
                            com.Parameters.AddWithValue("@ProductImage", Photo);
                        }
                        else
                        {
                            com.Parameters.AddWithValue("@ProductImage", DBNull.Value);
                        }

                        com.ExecuteNonQuery();
                        ControlForm.ClearData(this);
                        imageProduct.Image = null;
                        Photo = null;
                        filepath = string.Empty;

                        MessageBox.Show("Product created successfully!", "Success",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // UPDATE Product
                    using (SqlCommand com = new SqlCommand("sUpdateProduct", Operation.con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ProductId", Convert.ToInt32(txtCode.Text));
                        com.Parameters.AddWithValue("@ProductName", txtName.Text.Trim());
                        com.Parameters.AddWithValue("@Price", decimal.Parse(txtPrice.Text.Trim()));
                        com.Parameters.AddWithValue("@Quantity", int.Parse(txtQuantity.Text.Trim()));

                        if (Photo != null && Photo.Length > 0)
                        {
                            com.Parameters.AddWithValue("@ProductImage", Photo);
                        }
                        else
                        {
                            com.Parameters.AddWithValue("@ProductImage", DBNull.Value);
                        }

                        com.ExecuteNonQuery();
                        MessageBox.Show("Product updated successfully!", "Success",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                loadData();
                DisplayProducts(); // Refresh the product display

                btnNew.BackColor = Color.MidnightBlue;
                btnNew.Image = SalesManagementSystem.Properties.Resources.Add;
                btnNew.Text = "បង្កើតថ្មី";
                isCreateUPdate = false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtCode.Text == null || string.IsNullOrEmpty(txtCode.Text.ToString().Trim()))
            {
                MessageBox.Show("Please select a product to delete", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult re = new DialogResult();
            re = MessageBox.Show("Do you want to delete this product?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (re == DialogResult.OK)
            {
                using (SqlCommand com = new SqlCommand("DeleteProduct", Operation.con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ProductId", Convert.ToInt32(txtCode.Text));
                    int rowEffect = com.ExecuteNonQuery();

                    ControlForm.ClearData(this);
                    imageProduct.Image = null;
                    Photo = null;
                    loadData();
                    DisplayProducts(); // Refresh the product display
                }
            }
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers, decimal point, and control characters
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        // Additional method to handle quantity validation
        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers and control characters
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void productContainer_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}