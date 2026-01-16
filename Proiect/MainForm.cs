using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Proiect;

public class MainForm : Form
{
    private readonly ILogger _logger;
    private readonly Clinica _clinica;
    private readonly IServiceProvider _serviceProvider;
    
    private TextBox txtEmail;
    private TextBox txtPassword;
    private Button btnLogin;
    private Button btnSignup;

    public MainForm(Clinica clinica, ILogger logger, IServiceProvider serviceProvider)
    {
        _clinica = clinica; 
        _logger = logger;
        _serviceProvider = serviceProvider;
        InitializeUI();
    }

    private void InitializeUI()
    {
        this.Text = "Clinic Authentication";
        this.Width = 350;
        this.Height = 230;
        this.StartPosition = FormStartPosition.CenterScreen;

        // Email UI
        var lblEmail = new Label { Left = 30, Top = 30, Text = "Email:", AutoSize = true };
        txtEmail = new TextBox { Left = 100, Top = 25, Width = 200 };

        // Password UI
        var lblPassword = new Label { Left = 30, Top = 70, Text = "Password:", AutoSize = true };
        txtPassword = new TextBox { Left = 100, Top = 65, Width = 200, UseSystemPasswordChar = true };

        // Login button
        btnLogin = new Button { Left = 50, Top = 120, Width = 100, Text = "Login" };
        btnLogin.Click += BtnLogin_Click;

        // Sign Up button
        btnSignup = new Button { Left = 180, Top = 120, Width = 100, Text = "Sign Up" };
        btnSignup.Click += BtnSignup_Click;

        this.Controls.AddRange(new Control[] { lblEmail, txtEmail, lblPassword, txtPassword, btnLogin, btnSignup });
    }

    private void BtnLogin_Click(object sender, EventArgs e)
    {
        string email = txtEmail.Text;
        string password = txtPassword.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Vă rugăm să completați toate câmpurile!");
            return;
        }

        if (_clinica.VerificareLogin(email, password))
        {
            User userLogat = _clinica.GetUserLogin(email, password);
            var authService = _serviceProvider.GetRequiredService<AuthService>();
            authService.CurrentUser = userLogat;
            
            MessageBox.Show($"Bine ai venit {userLogat.Email}!");

            Form nextForm = null;
            switch (userLogat)
            {
                case Admin:
                    nextForm = _serviceProvider.GetRequiredService<AdminForm>();
                    break;
                case Medic:
                    nextForm = _serviceProvider.GetRequiredService<MedicForm>();
                    break;
                case Pacient:
                    nextForm = _serviceProvider.GetRequiredService<PacientForm>();
                    break;
                default:
                    MessageBox.Show("Eroare neasteptata");
                    return;
            }

            if (nextForm != null)
            {
                nextForm.FormClosed += (s, args) =>
                {
                    if (!Visible)
                    {
                        Application.Exit();
                    }
                };

                this.Hide(); 
                nextForm.Show(); 
                txtEmail.Clear();
                txtPassword.Clear();
            }
        }
        else
        {
            MessageBox.Show("Login esuat!");
        }
    }

    private void BtnSignup_Click(object sender, EventArgs e)
    {
        var signupForm = _serviceProvider.GetRequiredService<SignupForm>();
        signupForm.ShowDialog();
    }
}