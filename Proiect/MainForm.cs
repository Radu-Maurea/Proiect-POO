using System;
using System.Windows.Forms;

namespace Proiect
{
    //Clinica clinica = new Clinica();
    public class MainForm : Form
    {
        private Clinica clinica = new Clinica();

        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnSignup;

        public MainForm()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Clinic Authentication";
            this.Width = 350;
            this.Height = 230;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Email
            var lblEmail = new Label
            {
                Left = 30,
                Top = 30,
                Text = "Email:",
                AutoSize = true
            };

            txtEmail = new TextBox
            {
                Left = 100,
                Top = 25,
                Width = 200
            };

            // Password
            var lblPassword = new Label
            {
                Left = 30,
                Top = 70,
                Text = "Password:",
                AutoSize = true
            };

            txtPassword = new TextBox
            {
                Left = 100,
                Top = 65,
                Width = 200,
                UseSystemPasswordChar = true
            };

            // Login button
            btnLogin = new Button
            {
                Left = 50,
                Top = 120,
                Width = 100,
                Text = "Login"
            };
            btnLogin.Click += BtnLogin_Click;

            // Sign Up button
            btnSignup = new Button
            {
                Left = 180,
                Top = 120,
                Width = 100,
                Text = "Sign Up"
            };
            btnSignup.Click += BtnSignup_Click;

            // Add controls
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnSignup);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter email and password.");
                return;
            }

            if (clinica.VerificareLogin(email, password))
            {
                User userLogat = clinica.GetUserLogin(email, password);
                MessageBox.Show($"Welcome {userLogat.Email}!");
                this.Hide();
                switch (userLogat)
                {
                    case Admin admin:
                        AdminForm adminForm = new AdminForm(clinica, admin);
                        adminForm.ShowDialog();
                        break;
                    case Medic medic:
                        MedicForm medicForm = new MedicForm(clinica, medic);
                        medicForm.ShowDialog();
                        break;
                    case Pacient pacient:
                        PacientForm pacientForm = new PacientForm(clinica, pacient);
                        pacientForm.ShowDialog();
                        break;
                    default:
                        MessageBox.Show("Something went wrong!");
                        break;
                }
            }
            else
            {
                MessageBox.Show("Login failed!");
                this.Close();
            }
        }

        private void BtnSignup_Click(object sender, EventArgs e)
        {
            using (var signupForm = new SignupForm(clinica))
            {
                signupForm.ShowDialog();
            }
        }
    }
}
