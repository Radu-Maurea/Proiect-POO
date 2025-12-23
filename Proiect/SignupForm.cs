using System;
using System.Windows.Forms;

namespace Proiect
{
    public class SignupForm : Form
    {
        private ComboBox Roluri;
        private TextBox Email;
        private TextBox Password;
        private Button BtnSignup;
        
        private Clinica clinica;

        public void InitializeUI()
        {
            this.Text = "Creeaza Contul";
            this.Width = 350;
            this.Height = 230;
            this.StartPosition = FormStartPosition.CenterScreen;

            //Selectie Roluri
            var lblRole = new Label { Left = 30, Top = 30, Text = "Rol:" };
            Roluri = new ComboBox
            {
                Left = 125, Top = 25, Width = 170,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            Roluri.Items.AddRange(new Object[] {"Admin", "Medic", "Pacient"});
            Roluri.SelectedIndex = 0;

            //Introducere Email
            var lblEmail = new Label { Left = 30, Top = 70, Text = "Email:" };
            Email = new TextBox { Left = 120, Top = 65, Width = 170 };

            //Parola 
            var lblPassword = new Label { Left = 30, Top = 110, Text = "Password:" };
            Password = new TextBox{
                Left = 120, Top = 105, Width = 170,
                UseSystemPasswordChar = true
            };
            
            //Buton
            BtnSignup = new Button
            {
                Left = 120, Top = 150, Width = 170,
                Text = "Create Account"
            };
            BtnSignup.Click += BtnSignup_Click;
            Controls.AddRange(new Control[]
            {
                lblRole, Roluri,
                lblEmail, Email,
                lblPassword, Password,
                BtnSignup
            });

        }

        private void BtnSignup_Click(object sender, EventArgs e)
        {
            string rol = Roluri.SelectedItem.ToString();
            string email = Email.Text.Trim().ToLower();
            string password = Password.Text;
            
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Va rog completati toate campurile!");  
                return;
            }

            if (clinica.ExistaEmail(email))
            {
                MessageBox.Show("Emailul este deja folosit!");
                return; 
            }

            User user = rol switch
            {
                "Admin" => new Admin(email, password),
                "Medic" => new Medic(email, password),
                "Pacient" => new Pacient(email, password),
                _ => null
            };
            clinica.AdaugaUtilizator(user);
            
            MessageBox.Show("Account created successfully!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        public SignupForm(Clinica clinica)
        {
            this.clinica = clinica;
            InitializeUI();

            foreach (var pacient in clinica.UtilizatoriReadOnly)
            {
                Console.WriteLine(pacient.Email);
            }
        }
    }
}
