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
    private Panel panelAdaugare, panelStergere, panelModifica;

    // Controale specifice pentru TAB-ul de ADAUGARE
    private TextBox txtAdaugEmail;
    private TextBox txtAdaugPass;

    private ComboBox cmbSpecializari,
        cmbProgram,
        cmbMedici,
        cmbSelectieEmail,
        cmbModificaSpecializari,
        cmbModificaProgram;

    // Controale specifice pentru TAB-ul de STERGERE
    private TextBox txtStergeEmail;

    // Controale specifice pentru TAB-ul de MODIFICARE
    private Button btnSalveazaModificari;

    public AdminForm(Clinica clinica, Admin admin)
    {
        this.clinica = clinica;
        this.admin = admin;
        InitializeUI();
    }

    public void InitializeUI()
    {
        admin.SetClinica(clinica);

        this.Text = "Admin Panel";
        this.Width = 600;
        this.Height = 450;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(240, 248, 255);

        // --- HEADER (Rămâne neschimbat) ---
        Panel headerPanel = new Panel { BackColor = Color.DarkBlue, Dock = DockStyle.Top, Height = 80 };
        Label lbl = new Label
        {
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

        txtDisplay = new TextBox
        {
            Multiline = true, Dock = DockStyle.Fill, ReadOnly = true,
            ScrollBars = ScrollBars.Vertical, Font = new Font("Consolas", 10), BackColor = Color.White
        };

        // Inițializăm cele două panouri de lucru
        UI_Adaugare();
        UI_Stergere();
        UI_Modifica();
        // Le adăugăm în containerul principal
        mainContentPanel.Controls.Add(txtDisplay);
        mainContentPanel.Controls.Add(panelAdaugare);
        mainContentPanel.Controls.Add(panelStergere);
        mainContentPanel.Controls.Add(panelModifica);

        // --- CREARE BUTOANE MENIU ---
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

        Button btnAdauga = CreateMenuButton("Adaugă Medici", 20);
        btnAdauga.Click += (s, e) => { SchimbaPanel(panelAdaugare); };

        Button btnSterge = CreateMenuButton("Șterge Medici", 70);
        btnSterge.Click += (s, e) => { SchimbaPanel(panelStergere); };

        Button btnModifica = CreateMenuButton("Modifică Medici", 120);
        btnModifica.Click += btnModifica_Click;

        Button btnAfisare = CreateMenuButton("Afisare Conturi", 170);
        btnAfisare.Click += btnAfisare_Click;

        Button btnAddServicii = CreateMenuButton("Adaugare Servicii medicale", 220);
        btnAddServicii.Click += btnAddServicii_Click;

        Button btnLogout = CreateMenuButton("Logout", 270);
        btnLogout.Click += btnLogout_Click;

        sidePanel.Controls.AddRange(new Control[] { btnAdauga, btnSterge, btnModifica, btnAfisare, btnAddServicii,btnLogout });

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
        panelModifica.Visible = false;

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
        cmbSpecializari = new ComboBox
        {
            Top = 100, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList
        };
        cmbSpecializari.Items.AddRange(new object[] { "Cardiologie", "Dermatologie", "Neurologie", "Pediatrie" });
        cmbSpecializari.SelectedIndex = 0;

        Label lblProgram = new Label { Text = "Program:", Top = 140, Left = 10, AutoSize = true };
        cmbProgram = new ComboBox { Top = 140, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        cmbProgram.Items.AddRange(new object[] { "10:00 - 18:00", "18:00-12:00", "12:00-06:00" });
        cmbProgram.SelectedIndex = 0;

        Button btnConfirm = new Button
        {
            Text = "Confirmă Adăugare", Top = 180, Left = 120, Width = 150, Height = 30,
            BackColor = Color.LightGreen, FlatStyle = FlatStyle.Flat
        };
        btnConfirm.Click += BtnSalveazaMedic_Click;

        panelAdaugare.Controls.AddRange(new Control[]
        {
            lblEmail, txtAdaugEmail, lblPass, txtAdaugPass, lblSpec, cmbSpecializari, lblProgram, cmbProgram, btnConfirm
        });
    }

    private void BtnSalveazaMedic_Click(object sender, EventArgs e)
    {
        string email = txtAdaugEmail.Text.Trim();
        string pass = txtAdaugPass.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
        {
            MessageBox.Show("Eroare: Toate câmpurile sunt obligatorii!");
            return;
        }

        if (clinica.ExistaEmail(email))
        {
            MessageBox.Show("Eroare: Acest email este deja înregistrat!");
            return;
        }

        bool succes = admin.AdaugaMedic(email, pass, cmbSpecializari.SelectedItem.ToString(),
            cmbProgram.SelectedItem.ToString());
        if (succes)
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

        Button btnConfirmStergere = new Button
        {
            Text = "Confirmă Ștergere", Top = 70, Left = 150, Width = 150, Height = 30,
            BackColor = Color.Red, FlatStyle = FlatStyle.Flat, ForeColor = Color.White
        };
        btnConfirmStergere.Click += btnStergeMedic_Click;

        panelStergere.Controls.AddRange(new Control[] { lblEmail, txtStergeEmail, btnConfirmStergere });
    }

    private void btnStergeMedic_Click(object sender, EventArgs e)
    {
        string email = txtStergeEmail.Text.Trim().ToLower();
        if (string.IsNullOrEmpty(email))
        {
            MessageBox.Show("Introduceți email-ul medicului pentru ștergere.");
            return;
        }

        bool succes = admin.StergeMedic(email);
        if (succes)
        {
            MessageBox.Show("Medicul a fost eliminat definitiv.");
            txtStergeEmail.Clear();
            btnAfisare_Click(null, null);
        }
        else
        {
            MessageBox.Show("Eroare:Medicul nu a fost gasit");
        }

    }

    private void btnAfisare_Click(object sender, EventArgs e)
    {
        SchimbaPanel(new Panel());
        txtDisplay.Visible = true;
        txtDisplay.Clear();
        txtDisplay.AppendText("Lista Utilizatori Sistem:" + Environment.NewLine);
        txtDisplay.AppendText("--------------------------" + Environment.NewLine);

        foreach (var u in clinica.UtilizatoriReadOnly)
        {
            txtDisplay.AppendText($"[{u.Rol()}] - {u.Email}" + Environment.NewLine); //
        }
    }

    private void btnModifica_Click(object sender, EventArgs e)
    {
        cmbSelectieEmail.Items.Clear();
        foreach (var u in clinica.UtilizatoriReadOnly)
        {
            if (u.Rol() == "Medic") cmbSelectieEmail.Items.Add(u.Email);
        }

        // 2. Afișează panelul existent
        SchimbaPanel(panelModifica);

    }

    private void UI_Modifica()
    {
        panelModifica = new Panel { Dock = DockStyle.Fill, Visible = false };
        Label lblEmail = new Label { Text = "Selectează Medic:", Top = 20, Left = 10, AutoSize = true };
        cmbSelectieEmail = new ComboBox
        {
            Top = 20, Left = 120, Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        Label lblSpecializare = new Label { Text = "Specializare Noua:", Top = 60, Left = 10, AutoSize = true };
        cmbModificaSpecializari = new ComboBox
        {
            Top = 60, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList
        };
        cmbModificaSpecializari.Items.AddRange(
            new object[] { "Cardiologie", "Dermatologie", "Neurologie", "Pediatrie" });
        cmbModificaSpecializari.SelectedIndex = 0;

        Label lblProgram = new Label { Text = "Program:", Top = 100, Left = 10, AutoSize = true };
        cmbModificaProgram = new ComboBox
            { Top = 100, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        cmbModificaProgram.Items.AddRange(new object[] { "10:00 - 18:00", "18:00-12:00", "12:00-06:00" });
        cmbModificaProgram.SelectedIndex = 0;

        btnSalveazaModificari = new Button
        {
            Text = "Salvează Modificări", Top = 150, Left = 130, Width = 150, Height = 35,
            BackColor = Color.LightSkyBlue, FlatStyle = FlatStyle.Flat
        };
        btnSalveazaModificari.Click += BtnSalveazaModificari_Click;

        panelModifica.Controls.Add(lblEmail);
        panelModifica.Controls.Add(lblSpecializare);
        panelModifica.Controls.Add(cmbModificaSpecializari);
        panelModifica.Controls.Add(cmbSelectieEmail);
        panelModifica.Controls.Add(lblProgram);
        panelModifica.Controls.Add(cmbModificaProgram);
        panelModifica.Controls.Add(btnSalveazaModificari);
    }

    private void BtnSalveazaModificari_Click(object sender, EventArgs e)
    {
        if (cmbSelectieEmail.SelectedItem == null)
        {
            MessageBox.Show("Vă rugăm selectați un medic!");
            return;
        }

        string email = cmbSelectieEmail.SelectedItem.ToString();
        string spec = cmbModificaSpecializari.SelectedItem?.ToString() ?? "Nespecificat";
        string prog = cmbModificaProgram.SelectedItem?.ToString() ?? "Nespecificat";

        bool succes = admin.ModificaMedic(email, spec, prog);

        if (succes)
        {
            MessageBox.Show($"Datele medicului {email} au fost actualizate!");
            btnAfisare_Click(null, null); // Refresh la listă
        }
        else
        {
            MessageBox.Show("Eroare la modificarea datelor.");
        }
    }

    private void btnAddServicii_Click(object sender, EventArgs e)
    {
        
    }

private void btnLogout_Click(object sender, EventArgs e)
    {
        this.Hide();
        MainForm mainForm = new MainForm(); 
        mainForm.Show();
        this.Close();
    }
    
    
}
