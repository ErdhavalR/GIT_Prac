using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace GIT_Prac
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


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
        private void Form1_Load(object sender, EventArgs e)
        {
            cmbCity.SelectedIndex = 0;
        }
        #endregion

    }
}
