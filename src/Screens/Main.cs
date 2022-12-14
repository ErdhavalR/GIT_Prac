using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using GIT_Prac;
using App_Globals;
using DataBaseClass;

namespace GIT_Prac
{
    public partial class Form1 : Form
    {
        #region // ------------------------------ Form Variables ------------------------------ //
        public int GridViewCellIndex;
        #endregion

        #region // ------------------------------ TextBox ZipCode KeyPress Event ------------------------------ //
        private void txtZipCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #endregion

        #region // ------------------------------ Data_Grid_View_Personal_Details Cell_Formatting Event ------------------------------ //
        private void dgvPersonalDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int Index = 0;
            foreach (DataGridViewRow Row in dgvPersonalDetails.Rows)
            {
                Row.Cells[Index].Value = Convert.ToString(Row.Index + 1);
            }
        }
        #endregion

        #region // ------------------------------ Button Add Click Event ------------------------------ //
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtAddress.Text == "" || txtZipCode.Text == "" || cmbCity.Text == "")
            {
                MessageBox.Show("Please Fill All The Fields");
            }
            else
            {
                string s = txtAddress.Text;
                RegexOptions OP = RegexOptions.None;
                Regex Reg = new Regex("[\r]{1}");
                s = Reg.Replace(s, " ");
                txtAddress.Text = s;
                dgvPersonalDetails.Rows.Add("", txtName.Text, txtAddress.Text, cmbCity.SelectedItem, txtZipCode.Text);
                MessageBox.Show("Record Added Successful");
                txtName.Clear();
                cmbCity.SelectedIndex = 0;
                txtAddress.Clear();
                txtZipCode.Clear();
            }
        }
        #endregion

        #region // ------------------------------ Main_Form Load Event ------------------------------ //
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbCity.SelectedIndex = 0;
        }
        #endregion

        #region // ------------------------------ Button Delete Click Event ------------------------------ //
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int DeleteRowIndex = 0;
            DialogResult DRObj = MessageBox.Show("Do You Really Want To Delete Last Entered Row ?","Delete Row", MessageBoxButtons.YesNo);
            if (DRObj == DialogResult.Yes)
            {
                DeleteRowIndex = dgvPersonalDetails.CurrentCell.RowIndex;
                dgvPersonalDetails.Rows.RemoveAt(DeleteRowIndex);
                MessageBox.Show("Last Row Deleted Successfully");
            }
            else
            {
                //txtName.Clear();
                //cmbCity.SelectedIndex = 0;
                //txtAddress.Clear();
                //txtZipCode.Clear();
            }
        }
        #endregion

        #region // ------------------------------ Button Update Click Event ------------------------------ //
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult DRobj = MessageBox.Show("Do You Want To Update The Data", "Update User :- '" + txtName.Text + "' Data", MessageBoxButtons.YesNo);
            if (DRobj == DialogResult.Yes)
            {
                DataGridViewRow NewData = dgvPersonalDetails.Rows[GridViewCellIndex];
                NewData.Cells[1].Value = txtName.Text;
                NewData.Cells[2].Value = txtAddress.Text;
                NewData.Cells[3].Value = cmbCity.Text;
                NewData.Cells[4].Value = txtZipCode.Text;
            }
            else if(DRobj == DialogResult.No)
            {
                txtName.Clear();
                cmbCity.SelectedIndex = 0;
                txtAddress.Clear();
                txtZipCode.Clear();
                //GridViewCellIndex = null;
            }
        }
        #endregion

        #region // ------------------------------ Data_Grid__View_Personal_Details Cell Click Event ------------------------------ //
        private void dgvPersonalDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridViewCellIndex       = e.RowIndex;
            DataGridViewRow Row     = dgvPersonalDetails.Rows[GridViewCellIndex];
            txtName.Text            = Row.Cells[1].Value.ToString();
            txtAddress.Text         = Row.Cells[2].Value.ToString();
            cmbCity.SelectedItem    = Row.Cells[3].Value.ToString();
            txtZipCode.Text         = Row.Cells[4].Value.ToString();
        }
        #endregion
    }
}
