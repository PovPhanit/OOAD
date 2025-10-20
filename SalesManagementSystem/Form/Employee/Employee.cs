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
    public partial class Employee : Form
    {
        public Employee()
        {
            InitializeComponent();
        }
        SqlDataAdapter DA;
        DataTable TB;
        SqlCommand com;
        byte[] Photo;
        string filepath;
        bool isCreateUPdate = false;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "JPEG FILE|*.jpg; *.jpeg |PNG FILE|*.png";
            fd.Title = "Open an image...";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                filepath = fd.FileName;
                imageStaff.Image = Image.FromFile(filepath);
            }
        }

        public void Fillcbx(ComboBox cbx, string fd1, string fd2, string TB2)
        {
            DA = new SqlDataAdapter("select " + fd1 + "," + fd2 + " From " + TB2, Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            cbx.DataSource = TB;
            cbx.DisplayMember = fd2;
            cbx.ValueMember = fd1;
        }
        private void loadData()
        {
            DA = new SqlDataAdapter("select * from viewStaff order by StaffID desc", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            dgvStaff.DataSource = TB;
            dgvStaff.ColumnHeadersHeight = 40;
            dgvStaff.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvStaff.RowTemplate.Height = 100;
            foreach (DataGridViewRow row in dgvStaff.Rows)
            {
                row.Height = 100;
            }
            dgvStaff.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            dgvStaff.DefaultCellStyle.Font = new Font("Khmer os system", 12);
            dgvStaff.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStaff.Columns["StaffID"].Visible = false;
            dgvStaff.Columns["RoleID"].Visible = false;
            dgvStaff.Columns["Name KH"].Width = 100;
            dgvStaff.Columns["Name EN"].Width = 100;
            dgvStaff.Columns["Salary"].DefaultCellStyle.Format = "c";
            DataGridViewImageColumn img = new DataGridViewImageColumn();
            img = (DataGridViewImageColumn)dgvStaff.Columns["Image"];
            img.ImageLayout = DataGridViewImageCellLayout.Stretch;




            foreach (DataGridViewColumn col in dgvStaff.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


        }
        private void fieldGender()
        {
            cbxGender.Items.Add("Male");
            cbxGender.Items.Add("Female");
            cbxGender.SelectedIndex = 0;
        }
        private void Employee_Load(object sender, EventArgs e)
        {
            Fillcbx(cbxRole, "roleID", "roleType", "role");
            fieldGender();
            txtNameKH.Focus();
            txtSearch.Text = "Search staff hear...";
            txtSearch.ForeColor = Color.Gray;
            loadData();
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search staff hear...")
            {
                txtSearch.ForeColor = Color.Black;
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                txtSearch.Text = "Search staff hear...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("SearchStaffByName", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.AddWithValue("@Keyword", txtSearch.Text.Trim());
            TB = new DataTable();
            DA.Fill(TB);
            dgvStaff.DataSource = TB;
        }

        private void dgvStaff_CellContentClick(object sender, DataGridViewCellEventArgs e)
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


        private void btnEdit_Click(object sender, EventArgs e)
        {
            isCreateUPdate = false;
            btnNew.BackColor = Color.IndianRed;
            btnNew.Image = SalesManagementSystem.Properties.Resources.Cancel;
            btnNew.Text = "បោះបង់";
            txtNameKH.Focus();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បោះបង់")
            {
                if (string.IsNullOrEmpty(txtNameKH.Text.Trim()))
                {
                    MessageBox.Show("Please Input Name KH...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNameKH.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtNameEN.Text.Trim()))
                {
                    MessageBox.Show("Please Input Name EN...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNameEN.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtPhnoe.Text.Trim()))
                {
                    MessageBox.Show("Please Input Phone number...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhnoe.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtVillage.Text.Trim()))
                {
                    MessageBox.Show("Please Input Village...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtVillage.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtSongkat.Text.Trim()))
                {
                    MessageBox.Show("Please Input Sangkat...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSongkat.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtKhan.Text.Trim()))
                {
                    MessageBox.Show("Please Input Khan...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtKhan.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtCity.Text.Trim()))
                {
                    MessageBox.Show("Please Input City...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCity.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                {
                    MessageBox.Show("Please Input Email...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    MessageBox.Show("Please Input Passowrd...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }
                if (isCreateUPdate)
                {
                    SqlCommand com = new SqlCommand("insertStaff", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@RoleID", int.Parse(cbxRole.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@StaffNameKH", txtNameKH.Text.ToString());
                    com.Parameters.AddWithValue("@StaffNameEN", txtNameEN.Text.ToString());
                    String Gender;
                    if (cbxGender.SelectedIndex == 0)
                    {
                        Gender = "Male";
                    }
                    else
                    {
                        Gender = "Female";
                    }
                    com.Parameters.AddWithValue("@StaffGender", Gender);
                    com.Parameters.AddWithValue("@StaffPhoneNumber", txtPhnoe.Text.ToString());
                    com.Parameters.AddWithValue("@StaffDob", dpDOB.Value);
                    com.Parameters.AddWithValue("@StaffVillage", txtVillage.Text.ToString());
                    com.Parameters.AddWithValue("@StaffSangkatKhum", txtSongkat.Text.ToString());
                    com.Parameters.AddWithValue("@StaffKhanSrok", txtKhan.Text.ToString());
                    com.Parameters.AddWithValue("@StaffProvinceCity", txtCity.Text.ToString());
                    com.Parameters.AddWithValue("@StaffEmail", txtEmail.Text.ToString());
                    com.Parameters.AddWithValue("@StaffPassword", txtPassword.Text.ToString());
                    com.Parameters.AddWithValue("@StaffStoppedWork", 0);
                    com.Parameters.AddWithValue("@StaffSalary", decimal.Parse(txtSalary.Text.ToString()));
                    if (filepath != null)
                    {
                        Photo = File.ReadAllBytes(filepath);   //using system io    
                    }
                    else
                    {
                        Photo = File.ReadAllBytes("../../Resources/bookDisplay.png");
                    }
                    com.Parameters.AddWithValue("@StaffImage", Photo);
                    filepath = null;
                    ControlForm.ClearData(this);
                    txtSearch.Text = "Search staff hear...";
                    txtSearch.ForeColor = Color.Gray;
                    txtNameKH.Focus();
                    int rowEffect = com.ExecuteNonQuery();
                    loadData();
                }
                else
                {
                    SqlCommand com = new SqlCommand("UpdateStaff", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@StaffID", int.Parse(txtNameKH.Tag.ToString()));
                    com.Parameters.AddWithValue("@RoleID", int.Parse(cbxRole.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@StaffNameKH", txtNameKH.Text.ToString());
                    com.Parameters.AddWithValue("@StaffNameEN", txtNameEN.Text.ToString());
                    String Gender;
                    if (cbxGender.SelectedIndex == 0)
                    {
                        Gender = "Male";
                    }
                    else
                    {
                        Gender = "Female";
                    }
                    com.Parameters.AddWithValue("@StaffGender", Gender);
                    com.Parameters.AddWithValue("@StaffPhoneNumber", txtPhnoe.Text.ToString());
                    com.Parameters.AddWithValue("@StaffDob", dpDOB.Value);
                    com.Parameters.AddWithValue("@StaffVillage", txtVillage.Text.ToString());
                    com.Parameters.AddWithValue("@StaffSangkatKhum", txtSongkat.Text.ToString());
                    com.Parameters.AddWithValue("@StaffKhanSrok", txtKhan.Text.ToString());
                    com.Parameters.AddWithValue("@StaffProvinceCity", txtCity.Text.ToString());
                    com.Parameters.AddWithValue("@StaffEmail", txtEmail.Text.ToString());
                    com.Parameters.AddWithValue("@StaffPassword", txtPassword.Text.ToString());
                    com.Parameters.AddWithValue("@StaffStoppedWork", 0);
                    com.Parameters.AddWithValue("@StaffSalary", decimal.Parse(txtSalary.Text.ToString()));
                    if (filepath != null)
                    {
                        Photo = File.ReadAllBytes(filepath);   //using system io    
                    }
                    com.Parameters.AddWithValue("@StaffImage", Photo);
                    filepath = null;
                    int rowEffect = com.ExecuteNonQuery();
                    loadData();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int stop;
            if (CheckboxStopWork.Checked.ToString() == "True")
            {
                stop = 1;
            }
            else
            {
                stop = 0;
            }
            if (string.IsNullOrEmpty(txtNameKH.Tag.ToString().Trim()))
            {
                MessageBox.Show("Please select list for delete", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult re = new DialogResult();
            re = MessageBox.Show("Do you want to delete it ?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (re == DialogResult.OK)
            {
                SqlCommand com = new SqlCommand("DeleteStaffByStopWork", Operation.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@StaffID", int.Parse(txtNameKH.Tag.ToString()));
                com.Parameters.AddWithValue("@StopWork", stop);
                int rowEffect = com.ExecuteNonQuery();
                CheckboxStopWork.Checked = false;
                ControlForm.ClearData(this);
                txtSearch.Text = "Search staff hear...";
                txtSearch.ForeColor = Color.Gray;
                txtNameKH.Tag = "";
                loadData();
            }
        }

        private void txtNameKH_KeyUp_1(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtSalary, txtNameEN);
        }

        private void txtNameEN_KeyUp_1(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtNameKH, cbxGender);
        }

        private void cbxGender_KeyUp_1(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtNameEN, txtPhnoe);
        }

        private void txtPhnoe_KeyUp_1(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, cbxGender, dpDOB);
        }

        private void dpDOB_KeyUp_1(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtPhnoe, txtVillage);
        }

        private void txtVillage_KeyUp_1(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, dpDOB, txtSongkat);
        }

        private void txtSongkat_KeyUp_1(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtVillage, txtKhan);
        }

        private void txtKhan_KeyUp_1(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtSongkat, cbxRole);
        }

        private void cbxRole_KeyUp_1(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtKhan, txtCity);
        }

        private void txtCity_KeyUp_1(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, cbxRole, txtEmail);
        }

        private void txtEmail_KeyUp_1(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtCity, txtPassword);
        }

        private void txtPassword_KeyUp_1(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtEmail, txtSalary);
        }

        private void txtSalary_KeyUp_1(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtPassword, txtNameKH);
        }
    }
}
