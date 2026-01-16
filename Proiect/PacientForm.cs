using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Proiect;

public class PacientForm : Form
{
    // Dependinte injectate prin Generic Host
    private readonly Clinica _clinica;
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly AuthService _authService;
    // Obiectul Pacient logat curent
    private Pacient _pacient; 

    private Panel mainContentPanel, panelCautare, panelCautareSpecializare, panelProgramare, panelIstoric, panelModificare, panelDiagnostic;
    private TextBox txtCautareNume, txtRezultat, txtRezultatSpecialitate, txtIstoric, txtDiagnostic;
    private ComboBox cmbSelectieSpecializare, cmbSelectieDoctor, cmbSelectieServiciu, cmbSelectieOra, cmbSelectieReprogramare;
    private Button btnConfirmaProgramarea, btnStergeProgramare, btnReprogramare;
    
    private ComboBox cmbModifDoctor, cmbModifServiciu, cmbModifOra;
    private Label lblModifDoctor, lblModifServiciu, lblModifOra;
    private Button btnSalveazaModificare;
    private string programareDeModificat = ""; 
    
    

    
    public PacientForm(Clinica clinica, ILogger logger, IServiceProvider serviceProvider,AuthService authService)
    {
        _clinica = clinica;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _authService = authService;
        _pacient = (Pacient)_authService.CurrentUser;
        _pacient.SetClinica(_clinica);
        _pacient.SetLogger(_logger);
        InitializeUI();
    }

    // Metoda de legatura intre obiectul Pacient si Interfata
    // public void SetLoggedPacient(Pacient pacient)
    // {
    //     this._pacient = pacient;
    //     this._pacient.SetClinica(_clinica);
    //     this._pacient.SetLogger(_logger);
    //     InitializeUI();
    // }

    public void InitializeUI()
    {
        this.Text = "Pacient Panel";
        this.Width = 750;
        this.Height = 500;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(240, 248, 255);

        Panel headerPanel = new Panel { BackColor = Color.DarkBlue, Dock = DockStyle.Top, Height = 80 };
        Label lbl = new Label {
            Text = $"Autentificat ca: {_pacient.Email}", // Folosim _pacient
            ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold),
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter
        };
        headerPanel.Controls.Add(lbl);

        Panel sidePanel = new Panel { BackColor = Color.FromArgb(45, 45, 48), Dock = DockStyle.Left, Width = 180 };
        mainContentPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };

        UI_CautareNume();
        UI_CautareSpecialitate();
        UI_CreereProgramare();
        UI_Istoric();
        UI_Modificare(); 
        UI_Diagnostic();
        
        mainContentPanel.Controls.AddRange(new Control[] { panelCautare, panelCautareSpecializare, panelProgramare, panelIstoric, panelModificare, panelDiagnostic });

        Button CreateMenuButton(string text, int top) {
            return new Button {
                Text = text, Top = top, Left = 10, Width = 160, Height = 40,
                FlatStyle = FlatStyle.Flat, ForeColor = Color.White,
                BackColor = Color.FromArgb(63, 63, 70), Cursor = Cursors.Hand
            };
        }

        Button btnCauta = CreateMenuButton("Caută Medic", 20);
        btnCauta.Click += (s, e) => SchimbaPanel(panelCautare);
        Button btnCautaSpecialitate = CreateMenuButton("Caută Specialitate", 70);
        btnCautaSpecialitate.Click += (s, e) => SchimbaPanel(panelCautareSpecializare);
        Button btnProgramare = CreateMenuButton("Programare Nouă", 120);
        btnProgramare.Click += (s, e) => SchimbaPanel(panelProgramare);
        Button btnVeziProg = CreateMenuButton("Istoric Programari", 170);
        btnVeziProg.Click += (s, e) => SchimbaPanel(panelIstoric);
        Button btnStergeProg = CreateMenuButton("Reprogramare Programari", 220);
        btnStergeProg.Click += (s, e) => SchimbaPanel(panelModificare);
        Button btnDiagnostic = CreateMenuButton("Diagnostic", 270);
        btnDiagnostic.Click += (s, e) => SchimbaPanel(panelDiagnostic);
        Button btnLogout = CreateMenuButton("Logout", 320);
        btnLogout.Click += btnLogout_Click;

        sidePanel.Controls.AddRange(new Control[] { btnCauta, btnCautaSpecialitate, btnProgramare, btnVeziProg, btnStergeProg, btnDiagnostic, btnLogout });
        this.Controls.Add(mainContentPanel);
        this.Controls.Add(sidePanel);
        this.Controls.Add(headerPanel);
    }

    private void SchimbaPanel(Panel panelActiv)
    {
        panelCautare.Visible = false;
        panelCautareSpecializare.Visible = false;
        panelProgramare.Visible = false;
        panelIstoric.Visible = false;
        panelModificare.Visible = false;
        panelDiagnostic.Visible = false;
        panelActiv.Visible = true;
        panelActiv.BringToFront();
    }

    public void UI_CautareNume()
    {
        panelCautare = new Panel { Dock = DockStyle.Fill, Visible = false };
        Label lblNume = new Label { Text = "Nume Medic:", Top = 20, Left = 10, AutoSize = true };
        txtCautareNume = new TextBox { Top = 20, Left = 120, Width = 200 };
        Button btnExecuta = new Button { Text = "Caută", Top = 18, Left = 330, Width = 80, BackColor = Color.LightBlue, FlatStyle = FlatStyle.Flat };
        btnExecuta.Click += (s, e) => {
            string nume = txtCautareNume.Text.Trim();
            if (string.IsNullOrEmpty(nume)) { MessageBox.Show("Introduceți un nume!"); return; }
            txtRezultat.Text = _pacient.CautareDoctorNume(nume);
        };
        txtRezultat = new TextBox { Top = 80, Left = 10, Width = 450, Height = 250, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Font = new Font("Consolas", 10) };
        panelCautare.Controls.AddRange(new Control[] { lblNume, txtCautareNume, btnExecuta, txtRezultat });
    }

    public void UI_CautareSpecialitate()
    {
        panelCautareSpecializare = new Panel { Dock = DockStyle.Fill, Visible = false };
        Label lblSpec = new Label { Text = "Specializare:", Top = 20, Left = 10, AutoSize = true };
        cmbSelectieSpecializare = new ComboBox { Top = 20, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        cmbSelectieSpecializare.Items.AddRange(new object[] { "Cardiologie", "Dermatologie", "Neurologie", "Pediatrie" });
        Button btnExecuta = new Button { Text = "Caută", Top = 18, Left = 330, Width = 80, BackColor = Color.LightGreen, FlatStyle = FlatStyle.Flat };
        btnExecuta.Click += (s, e) => {
            if (cmbSelectieSpecializare.SelectedItem == null) return;
            txtRezultatSpecialitate.Text = _pacient.CautareDoctorSpecializare(cmbSelectieSpecializare.SelectedItem.ToString());
        };
        txtRezultatSpecialitate = new TextBox { Top = 80, Left = 10, Width = 450, Height = 250, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Font = new Font("Consolas", 10) };
        panelCautareSpecializare.Controls.AddRange(new Control[] { lblSpec, cmbSelectieSpecializare, btnExecuta, txtRezultatSpecialitate });
    }

    public void UI_CreereProgramare()
    {
        panelProgramare = new Panel { Dock = DockStyle.Fill, Visible = false };
        Label lblDoc = new Label { Text = "Doctor:", Top = 20, Left = 10, AutoSize = true };
        cmbSelectieDoctor = new ComboBox { Top = 20, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        foreach (var medic in _clinica.Medici) { cmbSelectieDoctor.Items.Add(medic.Nume); }

        Label lblServ = new Label { Text = "Serviciu:", Top = 65, Left = 10, AutoSize = true, Visible = false };
        cmbSelectieServiciu = new ComboBox { Top = 60, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, Visible = false };
        Label lblOra = new Label { Text = "Ora:", Top = 105, Left = 10, AutoSize = true, Visible = false };
        cmbSelectieOra = new ComboBox { Top = 100, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, Visible = false };

        btnConfirmaProgramarea = new Button { Text = "Confirmă Programarea", Top = 150, Left = 120, Width = 200, Height = 35, BackColor = Color.LightGreen, Visible = false, FlatStyle = FlatStyle.Flat };
        
        cmbSelectieDoctor.SelectedIndexChanged += (s, e) =>
        {
            var medicSelectat = _clinica.Medici.FirstOrDefault(m => m.Nume == cmbSelectieDoctor.SelectedItem.ToString());
            if (medicSelectat != null)
            {
                cmbSelectieServiciu.Items.Clear();
                cmbSelectieOra.Items.Clear();
                foreach (var serv in medicSelectat.ServiciiOferiteReadOnly) { cmbSelectieServiciu.Items.Add(serv.Denumire); }

                if (medicSelectat.ProgramLucru == "10:00-18:00") cmbSelectieOra.Items.AddRange(new object[] { "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00" });
                else if (medicSelectat.ProgramLucru == "18:00-12:00") cmbSelectieOra.Items.AddRange(new object[] { "18:00", "19:00", "20:00", "21:00", "22:00", "23:00", "00:00", "01:00" });
                else cmbSelectieOra.Items.AddRange(new object[] { "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "00:00", "01:00", "02:00", "03:00", "04:00", "05:00" });

                lblServ.Visible = cmbSelectieServiciu.Visible = lblOra.Visible = cmbSelectieOra.Visible = btnConfirmaProgramarea.Visible = true;
            }
        };

        btnConfirmaProgramarea.Click += (s, e) => {
            try
            {
                var medic = _clinica.Medici.FirstOrDefault(m => m.Nume == cmbSelectieDoctor.SelectedItem.ToString());
                var serviciu = medic?.ServiciiOferiteReadOnly.FirstOrDefault(sv =>
                    sv.Denumire == cmbSelectieServiciu.SelectedItem.ToString());
                string oraSelectata = cmbSelectieOra.SelectedItem.ToString();
                if (_pacient.CreereProgramare(medic, serviciu, oraSelectata))
                {
                    MessageBox.Show("Programarea a fost creata cu succes!");
                    ActualizareReprogramari();
                    ActualizareIstoric();
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                _logger.LogError("Progrmare Indisponibila" + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la crearea programarii! " + ex.Message);
                _logger.LogError("Eroare Neprevazuta" + ex.Message);
            }
        };
        panelProgramare.Controls.AddRange(new Control[] { lblDoc, cmbSelectieDoctor, lblServ, cmbSelectieServiciu, lblOra, cmbSelectieOra, btnConfirmaProgramarea });
    }

    public void UI_Istoric()
    {
        panelIstoric = new Panel { Dock = DockStyle.Fill, Visible = false };
        txtIstoric = new TextBox { Top = 50, Left = 10, Width = 450, Height = 250, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical };
        panelIstoric.Controls.Add(new Label { Text = "Programari active:", Top = 20, Left = 10 });
        panelIstoric.Controls.Add(txtIstoric);
        ActualizareIstoric();
    }

    private void ActualizareIstoric()
    {
        if (txtIstoric != null) txtIstoric.Text = _pacient.VeziProgramari(_pacient.Email);
    }

    public void UI_Modificare()
    {
        panelModificare = new Panel { Dock = DockStyle.Fill, Visible = false };
        Label lblReprogramare = new Label { Text = "Selectati o programare:", Top = 20, Left = 10, AutoSize = true };
        cmbSelectieReprogramare = new ComboBox { Top = 20, Left = 180, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        
        btnStergeProgramare = new Button { Text = "Sterge", Top = 60, Left = 100, Width = 80, BackColor = Color.Red };
        btnReprogramare = new Button { Text = "Modifica", Top = 60, Left = 220, Width = 80, BackColor = Color.LightBlue };

        lblModifDoctor = new Label { Text = "Doctor Nou:", Top = 100, Left = 10, Visible = false };
        cmbModifDoctor = new ComboBox { Top = 100, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, Visible = false };
        lblModifServiciu = new Label { Text = "Serviciu Nou:", Top = 140, Left = 10, Visible = false };
        cmbModifServiciu = new ComboBox { Top = 140, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, Visible = false };
        lblModifOra = new Label { Text = "Ora Noua:", Top = 180, Left = 10, Visible = false };
        cmbModifOra = new ComboBox { Top = 180, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, Visible = false };
        btnSalveazaModificare = new Button { Text = "Salvează", Top = 220, Left = 120, Width = 200, BackColor = Color.Gold, Visible = false };

        cmbModifDoctor.SelectedIndexChanged += (s, e) => {
            var medic = _clinica.Medici.FirstOrDefault(m => m.Nume == cmbModifDoctor.SelectedItem.ToString());
            if (medic != null) {
                cmbModifServiciu.Items.Clear();
                foreach (var serv in medic.ServiciiOferiteReadOnly) cmbModifServiciu.Items.Add(serv.Denumire);
                cmbModifOra.Items.Clear();
                if (medic.ProgramLucru == "10:00-18:00") cmbModifOra.Items.AddRange(new object[] { "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00" });
                else if (medic.ProgramLucru == "18:00-12:00") cmbModifOra.Items.AddRange(new object[] { "18:00", "19:00", "20:00", "21:00", "22:00", "23:00", "00:00", "01:00" });
                else cmbModifOra.Items.AddRange(new object[] { "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "00:00", "01:00", "02:00", "03:00", "04:00", "05:00" });
            }
        };

        btnReprogramare.Click += (s, e) => {
            if (cmbSelectieReprogramare.SelectedItem == null) return;
            programareDeModificat = cmbSelectieReprogramare.SelectedItem.ToString();
            cmbModifDoctor.Items.Clear();
            foreach (var m in _clinica.Medici) cmbModifDoctor.Items.Add(m.Nume);
            lblModifDoctor.Visible = cmbModifDoctor.Visible = lblModifServiciu.Visible = cmbModifServiciu.Visible = lblModifOra.Visible = cmbModifOra.Visible = btnSalveazaModificare.Visible = true;
        };

        btnSalveazaModificare.Click += (s, e) => {
            if (cmbModifDoctor.SelectedItem == null || cmbModifServiciu.SelectedItem == null || cmbModifOra.SelectedItem == null) return;
            if (_pacient.StergeProgramare(programareDeModificat)) {
                var medic = _clinica.Medici.FirstOrDefault(m => m.Nume == cmbModifDoctor.SelectedItem.ToString());
                var serviciu = medic?.ServiciiOferiteReadOnly.FirstOrDefault(ser => ser.Denumire == cmbModifServiciu.SelectedItem.ToString());
                if (_pacient.CreereProgramare(medic, serviciu, cmbModifOra.SelectedItem.ToString())) {
                    MessageBox.Show("Modificat cu succes!");
                    ActualizareReprogramari(); ActualizareIstoric();
                    lblModifDoctor.Visible = cmbModifDoctor.Visible = lblModifServiciu.Visible = cmbModifServiciu.Visible = lblModifOra.Visible = cmbModifOra.Visible = btnSalveazaModificare.Visible = false;
                }
            }
        };

        btnStergeProgramare.Click += (s, e) => {
            if (cmbSelectieReprogramare.SelectedItem != null) {
                if(_pacient.StergeProgramare(cmbSelectieReprogramare.SelectedItem.ToString())) {
                    MessageBox.Show("Sters!"); ActualizareReprogramari(); ActualizareIstoric();
                }
            }
        };

        panelModificare.Controls.AddRange(new Control[] { lblReprogramare, cmbSelectieReprogramare, btnStergeProgramare, btnReprogramare, lblModifDoctor, cmbModifDoctor, lblModifServiciu, cmbModifServiciu, lblModifOra, cmbModifOra, btnSalveazaModificare });
        ActualizareReprogramari();
    }

    private void ActualizareReprogramari()
    {
        cmbSelectieReprogramare.Items.Clear();
        _clinica.IncarcaDate();
        foreach (var p in _clinica.ProgramariReadOnly.Where(p => p.Pacient.Email == _pacient.Email && !p.Vazut))
            cmbSelectieReprogramare.Items.Add($"{p.DataOra} || {p.Medic.Email}");
    }

    public void UI_Diagnostic()
    {
        panelDiagnostic = new Panel { Dock = DockStyle.Fill, Visible = false };
        txtDiagnostic = new TextBox { Top = 50, Left = 10, Width = 450, Height = 250, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical };
        panelDiagnostic.Controls.Add(new Label { Text = "Diagnostice primite:", Top = 20, Left = 10 });
        panelDiagnostic.Controls.Add(txtDiagnostic);
        
        _clinica.IncarcaDate();
        txtDiagnostic.Clear();
        foreach (var p in _clinica.ProgramariReadOnly.Where(p => p.Pacient.Email == _pacient.Email && p.Vazut))
            txtDiagnostic.AppendText(p.Medic.Nume + " - " + p.Diagnostic + Environment.NewLine);
    }

    private void btnLogout_Click(object sender, EventArgs e) {
        _authService.Logout();
        var mainFomr = _serviceProvider.GetRequiredService<MainForm>();
        mainFomr.Show();
        Hide();
    }
}