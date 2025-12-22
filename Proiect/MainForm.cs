using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Proiect
{
    public class MainForm : Form
    {
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnAddAdmin;
        private ListBox lstAdmins;

        private List<Admin> admins = new List<Admin>();

        public MainForm()
        {
            this.Text = "Clinic Admin Management";
            this.Width = 400;
            this.Height = 300;

            // Email
            var lblEmail = new Label { Left = 20, Top = 20, Text = "Email:" };
            txtEmail = new TextBox { Left = 100, Top = 20, Width = 200 };

            // Password
            var lblPassword = new Label { Left = 20, Top = 60, Text = "Password:" };
            txtPassword = new TextBox { Left = 100, Top = 60, Width = 200 };

            // Button
            btnAddAdmin = new Button { Left = 100, Top = 100, Text = "Add Admin" };
            btnAddAdmin.Click += BtnAddAdmin_Click;

            // List of Adminse
            lstAdmins = new ListBox { Left = 20, Top = 140, Width = 340, Height = 100 };

            // Add controls to form
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnAddAdmin);
            this.Controls.Add(lstAdmins);
        }

        private void BtnAddAdmin_Click(object? sender, EventArgs e)
        {
            var email = txtEmail.Text;
            var password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.");
                return;
            }

            var admin = new Admin(email, password);
            admins.Add(admin);

            lstAdmins.Items.Add($"{admin.Email} (ID: {admin.Id})");

            // Clear inputs
            txtEmail.Text = "";
            txtPassword.Text = "";
        }
    }
}
