using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proiect
{
    public class SignupForm : Form
    {
        private TextBox txtNume; 
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnSignup;
        
        private Clinica clinica;

        public void InitializeUI()
        {
            this.Text = "Creare Cont Pacient";
            this.Width = 350;
            this.Height = 250; 
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);
            
            var lblNume = new Label { Left = 30, Top = 30, Text = "Nume Complet:", Width = 90 };
            txtNume = new TextBox { Left = 130, Top = 25, Width = 160 };
            
            var lblEmail = new Label { Left = 30, Top = 70, Text = "Email:" };
            txtEmail = new TextBox { Left = 130, Top = 65, Width = 160 };


            var lblPassword = new Label { Left = 30, Top = 110, Text = "Parolă:" };
            txtPassword = new TextBox {
                Left = 130, Top = 105, Width = 160,
                UseSystemPasswordChar = true
            };
            
            btnSignup = new Button
            {
                Left = 130, Top = 160, Width = 160, Height = 35,
                Text = "Înregistrare",
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnSignup.Click += BtnSignup_Click;

            Controls.AddRange(new Control[]
            {
                lblNume, txtNume,
                lblEmail, txtEmail,
                lblPassword, txtPassword,
                btnSignup
            });
        }

        private void BtnSignup_Click(object sender, EventArgs e)
        {
            string nume = txtNume.Text.Trim();
            string email = txtEmail.Text.Trim().ToLower();
            string password = txtPassword.Text;
            
            if (string.IsNullOrWhiteSpace(nume) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vă rugăm să completați toate câmpurile!");  
                return;
            }
            
            if (clinica.ExistaEmail(email))
            {
                MessageBox.Show("Acest email este deja utilizat!");
                return; 
            }
            
            Pacient nouPacient = new Pacient(email, password);
            nouPacient.SetNume(nume);
            
            clinica.AdaugaUtilizator(nouPacient);
            
            MessageBox.Show($"Contul pacientului {nume} a fost creat cu succes!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public SignupForm(Clinica clinica)
        {
            this.clinica = clinica;
            InitializeUI();
        }
    }
}