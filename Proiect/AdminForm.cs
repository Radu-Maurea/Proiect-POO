using System;
using System.Drawing; 
using System.Windows.Forms;
using System.Linq;

namespace Proiect;

public class AdminForm : Form
{
    private Admin admin;
    private Clinica clinica;
    
    // Controale principale
    private TextBox txtDisplay;
    private Panel panelAdaugare, panelStergere, panelModifica, panelServicii, panelAsociere;

    // Controale Adaugare
    private TextBox txtAdaugEmail, txtNumeMedic, txtAdaugPass;
    private ComboBox cmbSpecializari, cmbProgram;

    // Controale Stergere
    private TextBox txtStergeEmail;

    // Controale Modificare
    private ComboBox cmbSelectieEmail, cmbModificaSpecializari, cmbModificaProgram;
    private Button btnSalveazaModificari;

    // Controale Servicii
    private TextBox txtDenumireServiciu, txtPretServiciu, txtDurataServiciu;

    // Controale Asociere
    private ComboBox cmbAsociereMedici, cmbAsociereServicii;

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
        this.Width = 700;
        this.Height = 600;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(240, 248, 255);

        // --- HEADER ---
        Panel headerPanel = new Panel { BackColor = Color.DarkBlue, Dock = DockStyle.Top, Height = 80 };
        Label lbl = new Label {
            Text = $"Autentificat ca: {admin.Email}",
            ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold),
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter
        };
        headerPanel.Controls.Add(lbl);

        // --- SIDEBAR ---
        Panel sidePanel = new Panel { BackColor = Color.FromArgb(45, 45, 48), Dock = DockStyle.Left, Width = 180 };

        // --- CONTENT AREA ---
        Panel mainContentPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };

        txtDisplay = new TextBox {
            Multiline = true, Dock = DockStyle.Fill, ReadOnly = true, Visible = false,
            ScrollBars = ScrollBars.Vertical, Font = new Font("Consolas", 10), BackColor = Color.White
        };

        // Initializare Paneluri
        UI_Adaugare();
        UI_Stergere();
        UI_Modifica();
        UI_Servicii();
        UI_Asociere();

        mainContentPanel.Controls.AddRange(new Control[] { 
            txtDisplay, panelAdaugare, panelStergere, panelModifica, panelServicii, panelAsociere 
        });

        // --- BUTOANE MENIU ---
        Button CreateMenuButton(string text, int top) {
            return new Button {
                Text = text, Top = top, Left = 10, Width = 160, Height = 40,
                FlatStyle = FlatStyle.Flat, ForeColor = Color.White,
                BackColor = Color.FromArgb(63, 63, 70), Cursor = Cursors.Hand
            };
        }

        Button btnAdauga = CreateMenuButton("Adaugă Medici", 20);
        btnAdauga.Click += (s, e) => SchimbaPanel(panelAdaugare);

        Button btnSterge = CreateMenuButton("Șterge Medici", 70);
        btnSterge.Click += (s, e) => SchimbaPanel(panelStergere);

        Button btnModifica = CreateMenuButton("Modifică Medici", 120);
        btnModifica.Click += (s, e) => SchimbaPanel(panelModifica);

        Button btnAfisare = CreateMenuButton("Afisare Conturi", 170);
        btnAfisare.Click += (s, e) => SchimbaPanel(txtDisplay);

        Button btnAddServicii = CreateMenuButton("Adaugare Servicii", 220);
        btnAddServicii.Click += (s, e) => SchimbaPanel(panelServicii);

        Button btnAsociaza = CreateMenuButton("Asociază Servicii", 270);
        btnAsociaza.Click += (s, e) => SchimbaPanel(panelAsociere);
        
        Button btnLogout = CreateMenuButton("Logout", 370);
        btnLogout.Click += btnLogout_Click;

        sidePanel.Controls.AddRange(new Control[] { btnAdauga, btnSterge, btnModifica, btnAfisare, btnAddServicii, btnAsociaza, btnLogout });

        this.Controls.Add(mainContentPanel);
        this.Controls.Add(sidePanel);
        this.Controls.Add(headerPanel);
    }

    private void SchimbaPanel(Control controlActiv)
    {
        // Ascundem tot
        txtDisplay.Visible = false;
        panelAdaugare.Visible = false;
        panelStergere.Visible = false;
        panelModifica.Visible = false;
        panelServicii.Visible = false;
        panelAsociere.Visible = false;

        // Populare date "On Demand"
        if (controlActiv == panelModifica) {
            cmbSelectieEmail.Items.Clear();
            foreach (var u in clinica.UtilizatoriReadOnly.Where(x => x.Rol() == "Medic"))
                cmbSelectieEmail.Items.Add(u.Email);
        }
        else if (controlActiv == panelAsociere) {
            cmbAsociereMedici.Items.Clear();
            foreach (var m in clinica.Medici)
                cmbAsociereMedici.Items.Add($"{m.Nume} | {m.Email}");

            cmbAsociereServicii.Items.Clear();
            foreach (var s in clinica.servicii)
                cmbAsociereServicii.Items.Add(s.Denumire);
        }
        else if (controlActiv == txtDisplay) {
            txtDisplay.Clear();
            txtDisplay.AppendText("Utilizatori in sistem:" + Environment.NewLine);
            foreach (var u in clinica.UtilizatoriReadOnly)
                txtDisplay.AppendText($"[{u.Rol()}] - {u.Email}" + Environment.NewLine);
        }

        controlActiv.Visible = true;
        controlActiv.BringToFront();
    }

    public void UI_Adaugare() {
        panelAdaugare = new Panel { Dock = DockStyle.Fill, Visible = false };
        txtAdaugEmail = new TextBox { Top = 20, Left = 120, Width = 200 };
        txtNumeMedic = new TextBox { Top = 60, Left = 120, Width = 200 };
        txtAdaugPass = new TextBox { Top = 100, Left = 120, Width = 200, UseSystemPasswordChar = true };
        
        cmbSpecializari = new ComboBox { Top = 140, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        cmbSpecializari.Items.AddRange(new object[] { "Cardiologie", "Dermatologie", "Neurologie", "Pediatrie" });
        cmbSpecializari.SelectedIndex = 0;

        cmbProgram = new ComboBox { Top = 180, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        cmbProgram.Items.AddRange(new object[] { "10:00-18:00", "18:00-12:00", "00:00-06:00" });
        cmbProgram.SelectedIndex = 0;

        Button btn = new Button { Text = "Salvează", Top = 230, Left = 120, BackColor = Color.LightGreen };
        btn.Click += BtnSalveazaMedic_Click;

        panelAdaugare.Controls.AddRange(new Control[] { 
            new Label { Text = "Email:", Top = 20, Left = 10 }, txtAdaugEmail,
            new Label { Text = "Nume:", Top = 60, Left = 10 }, txtNumeMedic,
            new Label { Text = "Parolă:", Top = 100, Left = 10 }, txtAdaugPass,
            new Label { Text = "Spec:", Top = 140, Left = 10 }, cmbSpecializari,
            new Label { Text = "Program:", Top = 180, Left = 10 }, cmbProgram, btn 
        });
    }

    private void BtnSalveazaMedic_Click(object sender, EventArgs e) {
        if (admin.AdaugaMedic(txtAdaugEmail.Text, txtNumeMedic.Text, txtAdaugPass.Text, 
            cmbSpecializari.SelectedItem.ToString(), cmbProgram.SelectedItem.ToString())) {
            MessageBox.Show("Succes!");
            txtAdaugEmail.Clear(); txtNumeMedic.Clear(); txtAdaugPass.Clear();
        }
    }

    public void UI_Stergere() {
        panelStergere = new Panel { Dock = DockStyle.Fill, Visible = false };
        txtStergeEmail = new TextBox { Top = 20, Left = 120, Width = 200 };
        Button btn = new Button { Text = "Șterge", Top = 60, Left = 120, BackColor = Color.Red, ForeColor = Color.White };
        btn.Click += (s, e) => { if(admin.StergeMedic(txtStergeEmail.Text)) MessageBox.Show("Șters!"); };
        panelStergere.Controls.AddRange(new Control[] { new Label { Text = "Email:", Top = 20, Left = 10 }, txtStergeEmail, btn });
    }

    public void UI_Modifica() {
        panelModifica = new Panel { Dock = DockStyle.Fill, Visible = false };
        cmbSelectieEmail = new ComboBox { Top = 20, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        cmbModificaSpecializari = new ComboBox { Top = 60, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        cmbModificaSpecializari.Items.AddRange(new object[] { "Cardiologie", "Dermatologie", "Neurologie", "Pediatrie" });
        cmbModificaProgram = new ComboBox { Top = 100, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        cmbModificaProgram.Items.AddRange(new object[] { "10:00 - 18:00", "18:00-12:00", "12:00-06:00" });
        
        Button btn = new Button { Text = "Modifică", Top = 150, Left = 120, BackColor = Color.LightSkyBlue };
        btn.Click += (s, e) => {
            if (admin.ModificaMedic(cmbSelectieEmail.Text, cmbModificaSpecializari.Text, cmbModificaProgram.Text))
                MessageBox.Show("Actualizat!");
        };
        panelModifica.Controls.AddRange(new Control[] { 
            new Label { Text = "Medic:", Top = 20, Left = 10 }, cmbSelectieEmail,
            new Label { Text = "Spec Nouă:", Top = 60, Left = 10 }, cmbModificaSpecializari,
            new Label { Text = "Prog Nou:", Top = 100, Left = 10 }, cmbModificaProgram, btn 
        });
    }

    public void UI_Servicii() {
        panelServicii = new Panel { Dock = DockStyle.Fill, Visible = false };
        txtDenumireServiciu = new TextBox { Top = 20, Left = 120, Width = 200 };
        txtPretServiciu = new TextBox { Top = 60, Left = 120, Width = 200 };
        txtDurataServiciu = new TextBox { Top = 100, Left = 120, Width = 200 };
        Button btn = new Button { Text = "Adaugă", Top = 150, Left = 120, BackColor = Color.LightBlue };
        btn.Click += BtnSalveazaServiciu_Click;
        panelServicii.Controls.AddRange(new Control[] { 
            new Label { Text = "Nume:", Top = 20, Left = 10 }, txtDenumireServiciu,
            new Label { Text = "Pret:", Top = 60, Left = 10 }, txtPretServiciu,
            new Label { Text = "Durata:", Top = 100, Left = 10 }, txtDurataServiciu, btn 
        });
    }

    private void BtnSalveazaServiciu_Click(object sender, EventArgs e) {
        if (decimal.TryParse(txtPretServiciu.Text, out decimal p) && int.TryParse(txtDurataServiciu.Text, out int d)) {
            if (admin.AdaugaServiciu(txtDenumireServiciu.Text, p, d)) {
                MessageBox.Show("Serviciu salvat!");
                txtDenumireServiciu.Clear(); txtPretServiciu.Clear(); txtDurataServiciu.Clear();
            }
        }
    }

    public void UI_Asociere() {
        panelAsociere = new Panel { Dock = DockStyle.Fill, Visible = false };
        cmbAsociereMedici = new ComboBox { Top = 20, Left = 120, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
        cmbAsociereServicii = new ComboBox { Top = 60, Left = 120, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
        Button btn = new Button { Text = "Asociază", Top = 110, Left = 120, BackColor = Color.Orange };
        btn.Click += (s, e) => {
            if (cmbAsociereMedici.SelectedItem != null && cmbAsociereServicii.SelectedItem != null) {
                string email = cmbAsociereMedici.Text.Split('|')[1].Trim();
                if (admin.AsociazaServiciuMedic(email, cmbAsociereServicii.Text)) MessageBox.Show("Asociat!");
            }
        };
        panelAsociere.Controls.AddRange(new Control[] { 
            new Label { Text = "Medic:", Top = 20, Left = 10 }, cmbAsociereMedici,
            new Label { Text = "Serviciu:", Top = 60, Left = 10 }, cmbAsociereServicii, btn 
        });
    }

    private void btnLogout_Click(object sender, EventArgs e) {
        this.Hide();
        new MainForm().Show();
        this.Close();
    }
}