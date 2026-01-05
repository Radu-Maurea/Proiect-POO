using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace Proiect;

public class PacientForm : Form
{
    private Pacient pacient;
    private Clinica clinica;

    private Panel mainContentPanel, panelCautare, panelCautareSpecializare;
    private TextBox txtCautareNume, txtRezultat,txtRezultatSpecialitate;
    private ComboBox cmbSelectieSpecializare;

    public PacientForm(Clinica clinica, Pacient pacient)
    {
        this.clinica = clinica;
        this.pacient = pacient;
        InitializeUI();
    }

    public void InitializeUI()
    {
        pacient.SetClinica(clinica);
        this.Text = "Pacient Panel";
        this.Width = 700;
        this.Height = 450;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(240, 248, 255);

        Panel headerPanel = new Panel { BackColor = Color.DarkBlue, Dock = DockStyle.Top, Height = 80 };
        Label lbl = new Label
        {
            Text = $"Autentificat ca: {pacient.Email}",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };
        headerPanel.Controls.Add(lbl);

        Panel sidePanel = new Panel { BackColor = Color.FromArgb(45, 45, 48), Dock = DockStyle.Left, Width = 180 };
        
        mainContentPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };

        UI_CautareNume();
        UI_CautareSpecialitate();
        
        mainContentPanel.Controls.Add(panelCautare);
        mainContentPanel.Controls.Add(panelCautareSpecializare);

        Button CreateMenuButton(string text, int top)
        {
            return new Button
            {
                Text = text, Top = top, Left = 10, Width = 160, Height = 40,
                FlatStyle = FlatStyle.Flat, ForeColor = Color.White,
                BackColor = Color.FromArgb(63, 63, 70),
                Font = new Font("Segoe UI", 9, FontStyle.Regular), Cursor = Cursors.Hand
            };
        }

        Button btnCauta = CreateMenuButton("Caută Medic", 20);
        btnCauta.Click += (s, e) => { SchimbaPanel(panelCautare); };
        
        Button btnCautaSpecialitate = CreateMenuButton("Cauta Specialitate", 70);
        btnCautaSpecialitate.Click += (s, e) => { SchimbaPanel(panelCautareSpecializare);};
        
        Button btnLogout = CreateMenuButton("Logout", 120);
        btnLogout.Click += btnLogout_Click;

        sidePanel.Controls.AddRange(new Control[] { btnCauta, btnCautaSpecialitate ,btnLogout });

        this.Controls.Add(mainContentPanel);
        this.Controls.Add(sidePanel);
        this.Controls.Add(headerPanel);
    }

    private void SchimbaPanel(Panel panelActiv)
    {
        panelCautare.Visible = false;
        panelCautareSpecializare.Visible = false; 
    
        panelActiv.Visible = true;
        panelActiv.BringToFront();
    }

    public void UI_CautareNume()
    {
        panelCautare = new Panel { Dock = DockStyle.Fill, Visible = false };

        Label lblNume = new Label { Text = "Nume Medic:", Top = 20, Left = 10, AutoSize = true };
        txtCautareNume = new TextBox { Top = 20, Left = 120, Width = 200 };

        Button btnExecutaCautare = new Button { 
            Text = "Caută", Top = 18, Left = 330, Width = 80, 
            BackColor = Color.LightBlue, FlatStyle = FlatStyle.Flat 
        };
        btnExecutaCautare.Click += btnExecutaCautare_Click;

        Label lblRez = new Label { Text = "Rezultate:", Top = 60, Left = 10, AutoSize = true };
        txtRezultat = new TextBox { 
            Top = 80, Left = 10, Width = 380, Height = 250, 
            Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical,
            Font = new Font("Consolas", 10), BackColor = Color.White
        };

        panelCautare.Controls.AddRange(new Control[] { lblNume, txtCautareNume, btnExecutaCautare, lblRez, txtRezultat });
    }

    private void btnExecutaCautare_Click(object sender, EventArgs e)
    {
        Console.WriteLine("Buton apasat pentru: " + txtCautareNume.Text);
        string numedoctor = txtCautareNume.Text.Trim();
        if (string.IsNullOrEmpty(numedoctor))
        {
            MessageBox.Show("Introduceți un nume pentru căutare!");
            return;
        }

        string lista_numelor = pacient.CautareDoctorNume(numedoctor);
        txtRezultat.Text = lista_numelor;
    }

    public void UI_CautareSpecialitate()
    {
        panelCautareSpecializare = new Panel { Dock = DockStyle.Fill, Visible = false };
    
        Label lblSpec = new Label { Text = "Selectează Specializarea:", Top = 20, Left = 10, AutoSize = true };
        cmbSelectieSpecializare = new ComboBox
        {
            Top = 20, Left = 160, Width = 160, DropDownStyle = ComboBoxStyle.DropDownList
        };
        // Adăugăm aceleași specializări ca în AdminForm
        cmbSelectieSpecializare.Items.AddRange(new object[] { "Cardiologie", "Dermatologie", "Neurologie", "Pediatrie" });
        cmbSelectieSpecializare.SelectedIndex = 0;

        Button btnExecutaCautareSpec = new Button { 
            Text = "Caută", Top = 18, Left = 330, Width = 80, 
            BackColor = Color.LightGreen, FlatStyle = FlatStyle.Flat 
        };
        btnExecutaCautareSpec.Click += btnExecutaCautareSpec_Click;

        Label lblRez = new Label { Text = "Medici disponibili:", Top = 60, Left = 10, AutoSize = true };
    
        txtRezultatSpecialitate = new TextBox { 
            Top = 80, Left = 10, Width = 400, Height = 250, 
            Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical,
            Font = new Font("Consolas", 10), BackColor = Color.White
        };

        panelCautareSpecializare.Controls.AddRange(new Control[] { 
            lblSpec, cmbSelectieSpecializare, btnExecutaCautareSpec, lblRez, txtRezultatSpecialitate 
        });
    }
    
    private void btnExecutaCautareSpec_Click(object sender, EventArgs e)
    {
        if (cmbSelectieSpecializare.SelectedItem == null) return;
    
        string specializare = cmbSelectieSpecializare.SelectedItem.ToString();
        string rezultate = pacient.CautareDoctorSpecializare(specializare);
        txtRezultatSpecialitate.Text = rezultate;
    }
    
    private void btnLogout_Click(object sender, EventArgs e)
    {
        this.Hide();
        MainForm loginForm = new MainForm();
        loginForm.Show();
        this.Close();
    }
}