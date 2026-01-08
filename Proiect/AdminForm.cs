using System;
using System.Drawing; 
using System.Windows.Forms;
using System.Linq;

namespace Proiect;

public class AdminForm : Form
{
    private Admin admin;
    private ILogger logger;
    private Clinica clinica;
   
    private TextBox txtDisplay,txtStergeEmail,txtDenumireServiciu, txtPretServiciu, txtDurataServiciu, txtListaServicii,txtProgrmari,txtProgramari2;
    private TextBox txtStatConturi, txtStatMedici, txtStatServicii, txtStatProgTotal, txtStatProgDiag, txtStatProgInProgres;
    private TextBox txtAdaugEmail, txtNumeMedic, txtAdaugPass;
    
    private Panel panelAdaugare, panelStergere, panelModifica, panelServicii, panelAsociere, panelVeziServicii;
    private Panel panelProgrmari,panelStatistici,panelStergereProgramare;
    
    private ComboBox cmbSpecializari, cmbProgram;
    private ComboBox cmbSelectieEmail, cmbModificaSpecializari, cmbModificaProgram,cmbAsociereMedici, cmbAsociereServicii,cmbSelectieStergereProg,cmbSpec;
    
    private Button btnConfirmaStergereProg;

    public AdminForm(Clinica clinica, Admin admin)
    {
        this.clinica = clinica;
        logger = new FileLogger();
        this.admin = admin;
        InitializeUI();
    }

    public void InitializeUI()
    {
        admin.SetClinica(clinica,logger);

        this.Text = "Admin Panel";
        this.Width = 800;
        this.Height = 750;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(240, 248, 255);

        Panel headerPanel = new Panel { BackColor = Color.DarkBlue, Dock = DockStyle.Top, Height = 80 };
        Label lbl = new Label {
            Text = $"Autentificat ca: {admin.Email}",
            ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold),
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter
        };
        headerPanel.Controls.Add(lbl);

        Panel sidePanel = new Panel { BackColor = Color.FromArgb(45, 45, 48), Dock = DockStyle.Left, Width = 180 };

        Panel mainContentPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };

        txtDisplay = new TextBox {
            Multiline = true, Dock = DockStyle.Fill, ReadOnly = true, Visible = false,
            ScrollBars = ScrollBars.Vertical, Font = new Font("Consolas", 10), BackColor = Color.White
        };

        UI_Adaugare();
        UI_Stergere();
        UI_Modifica();
        UI_Servicii();
        UI_Asociere();
        UI_VeziServicii();
        UI_VeziProg();
        UI_Statistici();
        UI_StergereProgramare();
        
        mainContentPanel.Controls.AddRange(new Control[] { 
            txtDisplay, panelAdaugare, panelStergere, panelModifica, panelServicii, panelAsociere, panelVeziServicii, panelProgrmari, panelStatistici, panelStergereProgramare
        });

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

        Button btnVeziServicii = CreateMenuButton("Vizualizare Servicii", 320);
        btnVeziServicii.Click += (s, e) => SchimbaPanel(panelVeziServicii);
        
        Button btnVeziProg = CreateMenuButton("Istoric Programari", 370);
        btnVeziProg.Click += (s, e) => SchimbaPanel(panelProgrmari);
        
        Button btnStatistici = CreateMenuButton("Statistici", 420);
        btnStatistici.Click += (s, e) => SchimbaPanel(panelStatistici);
        
        Button btnStergeProgramare = CreateMenuButton("Sterge Programare", 470);
        btnStergeProgramare.Click += (s, e) => SchimbaPanel(panelStergereProgramare);
        
        Button btnLogout = CreateMenuButton("Logout", 520);
        btnLogout.Click += btnLogout_Click;

        sidePanel.Controls.AddRange(new Control[] { btnAdauga, btnSterge, btnModifica, btnAfisare, btnAddServicii, btnAsociaza, btnVeziServicii, btnVeziProg, btnStatistici, btnStergeProgramare, btnLogout });

        this.Controls.Add(mainContentPanel);
        this.Controls.Add(sidePanel);
        this.Controls.Add(headerPanel);
    }

    private void SchimbaPanel(Control controlActiv)
    {
        txtDisplay.Visible = false;
        panelAdaugare.Visible = false;
        panelStergere.Visible = false;
        panelModifica.Visible = false;
        panelServicii.Visible = false;
        panelAsociere.Visible = false;
        panelVeziServicii.Visible = false;
        panelProgrmari.Visible = false;
        panelStatistici.Visible = false;
        panelStergereProgramare.Visible = false;
        
        if (controlActiv == panelModifica) 
        {
            cmbSelectieEmail.Items.Clear();
            foreach (var u in clinica.UtilizatoriReadOnly.Where(x => x.Rol() == "Medic"))
                cmbSelectieEmail.Items.Add(u.Email);
        }
        else if (controlActiv == panelAsociere) 
        {
            cmbAsociereMedici.Items.Clear();
            foreach (var m in clinica.Medici)
                cmbAsociereMedici.Items.Add($"{m.Nume} | {m.Email}");

            cmbAsociereServicii.Items.Clear();
            foreach (var s in clinica.servicii)
                cmbAsociereServicii.Items.Add(s.Denumire);
        }
        else if (controlActiv == panelVeziServicii)
        {
            txtListaServicii.Clear();
            txtListaServicii.AppendText("Servicii Medicale Disponibile:" + Environment.NewLine);
            txtListaServicii.AppendText("--------------------------------------" + Environment.NewLine);
            foreach (var s in clinica.servicii)
                txtListaServicii.AppendText($"Denumire: {s.Denumire} | Pret: {s.Pret} RON | Durata: {s.DurataMinute} min" + Environment.NewLine);
        }
        else if (controlActiv == txtDisplay) 
        {
            txtDisplay.Clear();
            txtDisplay.AppendText("Utilizatori in sistem:" + Environment.NewLine);
            foreach (var u in clinica.UtilizatoriReadOnly)
                txtDisplay.AppendText($"[{u.Rol()}] - {u.Email}" + Environment.NewLine);
        }
        else if (controlActiv == panelProgrmari)
        {
            txtProgrmari.Clear();
            txtProgramari2.Clear();
            txtProgrmari.Text = admin.VeziProgramari();
            txtProgramari2.Text = admin.VeziProgramariDiagnosticate();
        }
        else if (controlActiv == panelStergereProgramare)
            ActualizareListaStergereAdmin();
        
        else if (controlActiv == panelStatistici)
        {
            clinica.IncarcaProgramariDinFisier();
            txtStatConturi.Text = admin.NumarConturi().ToString();
            txtStatMedici.Text = admin.NumarMedici().ToString();
            txtStatServicii.Text = admin.NumarServicii().ToString();
            txtStatProgTotal.Text = admin.NumarProgramari().ToString();
            txtStatProgDiag.Text = admin.NumarProgramariDiagnosticate().ToString();
            txtStatProgInProgres.Text = admin.NumarProgramariInProgres().ToString();
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
        
        cmbSpec = new ComboBox { Top = 220, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        foreach (var s in clinica.servicii) cmbSpec.Items.Add(s.Denumire);
        
        Button btn = new Button { Text = "Salvează", Top = 260, Left = 120, BackColor = Color.LightGreen };
        btn.Click += BtnSalveazaMedic_Click;

        panelAdaugare.Controls.AddRange(new Control[] { 
            new Label { Text = "Email:", Top = 20, Left = 10 }, txtAdaugEmail,
            new Label { Text = "Nume:", Top = 60, Left = 10 }, txtNumeMedic,
            new Label { Text = "Parolă:", Top = 100, Left = 10 }, txtAdaugPass,
            new Label { Text = "Spec:", Top = 140, Left = 10 }, cmbSpecializari,
            new Label { Text = "Serviciu:", Top = 220, Left = 10 }, cmbSpec,
            new Label { Text = "Program:", Top = 180, Left = 10 }, cmbProgram, btn 
        });
    }

    private void BtnSalveazaMedic_Click(object sender, EventArgs e) 
    {
        string emailIntrodus = admin.Email;
        
        if (string.IsNullOrWhiteSpace(txtAdaugEmail.Text) || string.IsNullOrWhiteSpace(txtNumeMedic.Text) ||
            string.IsNullOrWhiteSpace(txtAdaugPass.Text))
        {
            MessageBox.Show("Completati toate campurile!"); 
            logger?.LogWarning($"[EROARE] Încercare adăugare medic cu date incomplete. Email='{emailIntrodus}'");
            return;
            
        }
        if (cmbSpec.SelectedItem == null) { MessageBox.Show("Selectati un serviciu!"); return; }
        else if (clinica.Medici.Any(m => m.Email == txtAdaugEmail.Text)) { MessageBox.Show("Emailul este deja folosit!"); return; }
        else if (clinica.Medici.Any(m => m.Nume == txtNumeMedic.Text)) { MessageBox.Show("Numele este deja folosit!"); return; }
        
        string denumireSelectata = cmbSpec.SelectedItem.ToString();
        ServiciuMedical serviciu = clinica.servicii.FirstOrDefault(s => s.Denumire == denumireSelectata);
        if (admin.AdaugaMedic(txtAdaugEmail.Text, txtNumeMedic.Text, txtAdaugPass.Text,cmbSpecializari.SelectedItem.ToString(), cmbProgram.SelectedItem.ToString(),serviciu))
        {
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

    public void UI_VeziServicii() {
        panelVeziServicii = new Panel { Dock = DockStyle.Fill, Visible = false };
        txtListaServicii = new TextBox { Top = 50, Left = 10, Width = 550, Height = 450, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Font = new Font("Consolas", 10) };
        panelVeziServicii.Controls.AddRange(new Control[] { new Label { Text = "Lista Servicii:", Top = 20, Left = 10, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, txtListaServicii });
    }

    private void UI_VeziProg()
    {
        panelProgrmari = new Panel { Dock = DockStyle.Fill, Visible = false };
        Label lbl = new Label { Text = "Istoric Programari:", Top = 20, Left = 10 };
        txtProgrmari = new TextBox { Top = 50, Left = 10, Width = 600, Height = 200,Multiline = true, ReadOnly = true,ScrollBars = ScrollBars.Vertical};
        txtProgramari2 = new TextBox{Top = 280, Left = 10, Width = 600, Height = 200,Multiline = true, ReadOnly = true,ScrollBars = ScrollBars.Vertical};
        panelProgrmari.Controls.AddRange(new Control[] { lbl, txtProgrmari, new Label { Text = "Programari Diagnosticate:", Top = 250, Left = 10 }, txtProgramari2});
    }
    
    void AdaugaStatistica(string text, int top, out TextBox tb)
    {
        Label lbl = new Label { Text = text, Top = top, Left = 10, Width = 200, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
        tb = new TextBox { Top = top, Left = 220, Width = 60, ReadOnly = true, TextAlign = HorizontalAlignment.Center, BackColor = Color.White };
        panelStatistici.Controls.Add(lbl);
        panelStatistici.Controls.Add(tb);
    }
    
    private void UI_Statistici()
    {
        panelStatistici = new Panel { Dock = DockStyle.Fill, Visible = false };
        AdaugaStatistica("Total Conturi Utilizatori:", 20, out txtStatConturi);
        AdaugaStatistica("Număr Medici Activ:", 60, out txtStatMedici);
        AdaugaStatistica("Servicii Medicale Disponibile:", 100, out txtStatServicii);
        AdaugaStatistica("Total Programări Înregistrate:", 140, out txtStatProgTotal);
        AdaugaStatistica("Programări Diagnosticate (Finalizate):", 180, out txtStatProgDiag);
        AdaugaStatistica("Programări În Progres (Noi):", 220, out txtStatProgInProgres);
    }

    public void UI_StergereProgramare()
    {
        panelStergereProgramare = new Panel { Dock = DockStyle.Fill, Visible = false };
        cmbSelectieStergereProg = new ComboBox { Top = 50, Left = 10, Width = 400, DropDownStyle = ComboBoxStyle.DropDownList };
        btnConfirmaStergereProg = new Button { Text = "Șterge Programarea", Top = 90, Left = 10, Width = 150, Height = 35, BackColor = Color.Red, ForeColor = Color.White };
        btnConfirmaStergereProg.Click += (s, e) => {
            if (cmbSelectieStergereProg.SelectedItem != null) ExecutaStergereProgramare(cmbSelectieStergereProg.SelectedItem.ToString());
        };
        panelStergereProgramare.Controls.AddRange(new Control[] { new Label { Text = "Selectați programarea pentru eliminare:", Top = 20, Left = 10, Width = 300 }, cmbSelectieStergereProg, btnConfirmaStergereProg });
    }

    private void ActualizareListaStergereAdmin()
    {
        cmbSelectieStergereProg.Items.Clear();
        clinica.IncarcaProgramariDinFisier(); 
        foreach (var prog in clinica.programari.Where(p => !p.Vazut))
            cmbSelectieStergereProg.Items.Add($"{prog.DataOra} || {prog.Medic.Email}");
    }

    private void ExecutaStergereProgramare(string infoProgramare)
    {
        if (MessageBox.Show("Sunteți sigur?", "Confirmare", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            if (admin.StergeProgramare(infoProgramare)) { MessageBox.Show("Succes!"); ActualizareListaStergereAdmin(); }
        }
    }

    private void btnLogout_Click(object sender, EventArgs e) {
        this.Hide(); new MainForm().Show(); this.Close();
    }
}