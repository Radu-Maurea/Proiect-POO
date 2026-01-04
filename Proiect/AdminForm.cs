using System;
using System.Drawing; 
using System.Windows.Forms;
using System.Linq;

namespace Proiect;

public class AdminForm : Form
{
    private Admin admin;
    private Clinica clinica;
    private TextBox txtDisplay; 
    
    // Panouri principale pentru conținut
    private Panel panelAdaugare, panelStergere;
    
    // Controale specifice pentru TAB-ul de ADAUGARE
    private TextBox txtAdaugEmail;
    private TextBox txtAdaugPass;
    private ComboBox cmbSpecializari;
    private ComboBox cmbProgram;
    // Controale specifice pentru TAB-ul de STERGERE
    private TextBox txtStergeEmail;

    public AdminForm(Clinica clinica, Admin admin)
    {
        this.clinica = clinica;
        this.admin = admin;
        InitializeUI(); 
    }

    public void InitializeUI()
    {
        this.Text = "Admin Panel";
        this.Width = 600; 
        this.Height = 450;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(240, 248, 255);
        
        // --- HEADER (Rămâne neschimbat) ---
        Panel headerPanel = new Panel { BackColor = Color.DarkBlue, Dock = DockStyle.Top, Height = 80 };
        Label lbl = new Label {
            Text = $"Autentificat ca: {admin.Email}",
            ForeColor = Color.White, 
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            Dock = DockStyle.Fill, 
            TextAlign = ContentAlignment.MiddleCenter 
        };
        headerPanel.Controls.Add(lbl);
        
        // --- SIDEBAR (Meniu stânga) ---
        Panel sidePanel = new Panel { BackColor = Color.FromArgb(45, 45, 48), Dock = DockStyle.Left, Width = 180 };
        
        // --- CONTENT AREA (Zona unde se schimbă panourile) ---
        Panel mainContentPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };

        txtDisplay = new TextBox {
            Multiline = true, Dock = DockStyle.Fill, ReadOnly = true,
            ScrollBars = ScrollBars.Vertical, Font = new Font("Consolas", 10), BackColor = Color.White
        };

        // Inițializăm cele două panouri de lucru
        UI_Adaugare();
        UI_Stergere();

        // Le adăugăm în containerul principal
        mainContentPanel.Controls.Add(txtDisplay);
        mainContentPanel.Controls.Add(panelAdaugare); 
        mainContentPanel.Controls.Add(panelStergere);

        // --- CREARE BUTOANE MENIU ---
        Button CreateMenuButton(string text, int top) {
            return new Button {
                Text = text, Top = top, Left = 10, Width = 160, Height = 40,
                FlatStyle = FlatStyle.Flat, ForeColor = Color.White,
                BackColor = Color.FromArgb(63, 63, 70), 
                Font = new Font("Segoe UI", 9, FontStyle.Regular), Cursor = Cursors.Hand
            };
        }

        Button btnAdauga = CreateMenuButton("Adaugă Medici", 20);
        btnAdauga.Click += (s, e) => { SchimbaPanel(panelAdaugare); };

        Button btnSterge = CreateMenuButton("Șterge Medici", 70);
        btnSterge.Click += (s, e) => { SchimbaPanel(panelStergere); };

        Button btnModifica = CreateMenuButton("Modifică Medici", 120);
        btnModifica.Click += (s, e) => MessageBox.Show("Funcționalitate în curs de implementare...");

        Button btnAfisare = CreateMenuButton("Afisare Conturi", 170);
        btnAfisare.Click += btnAfisare_Click;
        
        Button btnLogout = CreateMenuButton("Logout", 220);
        btnLogout.Click += btnLogout_Click;

        sidePanel.Controls.AddRange(new Control[] { btnAdauga, btnSterge, btnModifica, btnAfisare, btnLogout });

        this.Controls.Add(mainContentPanel);
        this.Controls.Add(sidePanel);
        this.Controls.Add(headerPanel);
    }

    // Metodă pentru a schimba vizibilitatea între secțiuni
    private void SchimbaPanel(Panel panelActiv)
    {
        txtDisplay.Visible = false;
        panelAdaugare.Visible = false;
        panelStergere.Visible = false;
        
        panelActiv.Visible = true;
        panelActiv.BringToFront();
    }

    public void UI_Adaugare()
    {
        panelAdaugare = new Panel { Dock = DockStyle.Fill, Visible = false };

        Label lblEmail = new Label { Text = "Email Medic:", Top = 20, Left = 10, AutoSize = true };
        txtAdaugEmail = new TextBox { Top = 20, Left = 120, Width = 200 };

        Label lblPass = new Label { Text = "Parolă:", Top = 60, Left = 10, AutoSize = true };
        txtAdaugPass = new TextBox { Top = 60, Left = 120, Width = 200, UseSystemPasswordChar = true };

        Label lblSpec = new Label { Text = "Specializare:", Top = 100, Left = 10, AutoSize = true };
        cmbSpecializari = new ComboBox { 
            Top = 100, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList 
        };
        cmbSpecializari.Items.AddRange(new object[] { "Cardiologie", "Dermatologie", "Neurologie", "Pediatrie" });
        cmbSpecializari.SelectedIndex = 0;
        
        Label lblProgram = new Label { Text = "Program:", Top = 140, Left = 10, AutoSize = true };
        cmbProgram = new ComboBox { Top = 140, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        cmbProgram.Items.AddRange(new object[] { "10:00 - 18:00", "18:00-12:00", "12:00-06:00" });
        cmbProgram.SelectedIndex = 0;

        Button btnConfirm = new Button {
            Text = "Confirmă Adăugare", Top = 180, Left = 120, Width = 150, Height = 30,
            BackColor = Color.LightGreen, FlatStyle = FlatStyle.Flat
        };
        btnConfirm.Click += BtnSalveazaMedic_Click;

        panelAdaugare.Controls.AddRange(new Control[] { lblEmail, txtAdaugEmail, lblPass, txtAdaugPass, lblSpec, cmbSpecializari,lblProgram,cmbProgram, btnConfirm });
    }

    private void BtnSalveazaMedic_Click(object sender, EventArgs e)
    {
        string email = txtAdaugEmail.Text.Trim();
        string pass = txtAdaugPass.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass)) {
            MessageBox.Show("Eroare: Toate câmpurile sunt obligatorii!");
            return;
        }

        if (clinica.ExistaEmail(email)) {
            MessageBox.Show("Eroare: Acest email este deja înregistrat!");
            return;
        }

        Medic medicNou = new Medic(email, pass);
        medicNou.SetSpecialitate(new Specialitate(cmbSpecializari.SelectedItem.ToString()));
        medicNou.SetProgram(cmbProgram.SelectedItem.ToString());
        clinica.AdaugaUtilizator(medicNou);
        MessageBox.Show("Medic adăugat cu succes!");
        
        txtAdaugEmail.Clear();
        txtAdaugPass.Clear();
        btnAfisare_Click(null, null); 
    }

    public void UI_Stergere()
    {
        panelStergere = new Panel { Dock = DockStyle.Fill, Visible = false };

        Label lblEmail = new Label { Text = "Email Medic de șters:", Top = 20, Left = 10, AutoSize = true };
        txtStergeEmail = new TextBox { Top = 20, Left = 150, Width = 200 };

        Button btnConfirmStergere = new Button {
            Text = "Confirmă Ștergere", Top = 70, Left = 150, Width = 150, Height = 30,
            BackColor = Color.Red, FlatStyle = FlatStyle.Flat, ForeColor = Color.White
        };
        btnConfirmStergere.Click += btnStergeMedic_Click;

        panelStergere.Controls.AddRange(new Control[] { lblEmail, txtStergeEmail, btnConfirmStergere });
    }

    private void btnStergeMedic_Click(object sender, EventArgs e)
    {
        string email = txtStergeEmail.Text.Trim().ToLower();
        if (string.IsNullOrEmpty(email)) {
            MessageBox.Show("Introduceți email-ul medicului pentru ștergere.");
            return;
        }

        bool succes = clinica.StergeUtilizatorDupaEmail(email); //
        if (succes) {
            MessageBox.Show("Utilizatorul a fost eliminat definitiv.");
            txtStergeEmail.Clear();
            btnAfisare_Click(null, null);
        } else {
            MessageBox.Show("Eroare: Medicul nu a fost găsit.");
        }
    }

    private void btnAfisare_Click(object sender, EventArgs e)
    {
        SchimbaPanel(new Panel()); 
        txtDisplay.Visible = true;
        txtDisplay.Clear();
        txtDisplay.AppendText("Lista Utilizatori Sistem:" + Environment.NewLine);
        txtDisplay.AppendText("--------------------------" + Environment.NewLine);
        
        foreach (var u in clinica.UtilizatoriReadOnly) {
            txtDisplay.AppendText($"[{u.Rol()}] - {u.Email}" + Environment.NewLine); //
        }
    }

    private void btnLogout_Click(object sender, EventArgs e)
    {
        this.Hide();
        MainForm mainForm = new MainForm(); 
        mainForm.Show();
        this.Close();
    }
}
