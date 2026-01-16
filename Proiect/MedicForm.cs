using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Proiect;

public class MedicForm : Form
{
    private readonly Clinica _clinica;
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;
    
    // Obiectul Medic logat curent
    private Medic _medic; 
    
    private Button btnSalveazaDiagnostic;
    private Panel mainContentPanel, panelIstoric, panelDiagnostic;
    private ComboBox cmbPacienti;
    private TextBox txtIstoric, txtDiagnostic;

    // CONSTRUCTORUL: Primeste serviciile de la container
    
    private readonly AuthService _authService;
    
    public MedicForm(Clinica clinica, ILogger logger, IServiceProvider serviceProvider, AuthService authService)
    {
        _clinica = clinica;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _authService = authService;
        
        _medic = (Medic)_authService.CurrentUser;
        
        _medic.SetClinica(_clinica);
        _medic.SetLogger(_logger);
        
        InitializeUI();
    }

    // Metoda de legatura intre obiectul Medic si Interfata
    // public void SetLoggedMedic(Medic medic)
    // {
    //     _medic = medic;
    //     // Injectam dependintele necesare in obiectul medic
    //     _medic.SetClinica(_clinica);
    //     _medic.SetLogger(_logger); 
    //     InitializeUI();
    // }

    public void InitializeUI()
    {
        this.Text = "Medic Panel";
        this.Width = 750; 
        this.Height = 500;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(240, 248, 255);
        
        Panel headerPanel = new Panel { BackColor = Color.DarkBlue, Dock = DockStyle.Top, Height = 80 };
        Label lbl = new Label {
            Text = $"Autentificat ca: {_medic.Email}", // Folosim _medic
            ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold),
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter
        };
        headerPanel.Controls.Add(lbl);
        
        Panel sidePanel = new Panel { BackColor = Color.FromArgb(45, 45, 48), Dock = DockStyle.Left, Width = 180 };
        mainContentPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
        
        UI_Istoric();
        UI_Diagnostic();
        
        mainContentPanel.Controls.AddRange(new Control[] { panelIstoric, panelDiagnostic });
        
        Button CreateMenuButton(string text, int top) {
            return new Button {
                Text = text, Top = top, Left = 10, Width = 160, Height = 40,
                FlatStyle = FlatStyle.Flat, ForeColor = Color.White,
                BackColor = Color.FromArgb(63, 63, 70), Cursor = Cursors.Hand
            };
        }
        
        Button btnProgramari = CreateMenuButton("Programari", 20);
        btnProgramari.Click += (s, e) => SchimbaPanel(panelIstoric);
        
        Button btnDiagnostic = CreateMenuButton("Diagnostic", 70);
        btnDiagnostic.Click += (s, e) => SchimbaPanel(panelDiagnostic);
        
        Button btnLogout = CreateMenuButton("Logout", 120);
        btnLogout.Click += btnLogout_Click;
        
        sidePanel.Controls.AddRange(new Control[] { btnProgramari, btnDiagnostic, btnLogout });
        this.Controls.AddRange(new Control[] { mainContentPanel, sidePanel, headerPanel });
    }

    private void UI_Istoric()
    {
        _clinica.IncarcaDate();
        panelIstoric = new Panel { Dock = DockStyle.Fill, Visible = false };
        Label lblIstoric = new Label { Text = "Istoric Programari:", Top = 20, Left = 10, AutoSize = true };
        txtIstoric = new TextBox { Top = 80, Left = 10, Width = 450, Height = 250, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Font = new Font("Consolas", 10) };
        
        ActualizareIstoric();
        panelIstoric.Controls.AddRange(new Control[] { lblIstoric, txtIstoric });
    }

    private void ActualizareIstoric()
    {
        if (txtIstoric != null)
        {
            txtIstoric.Clear();
            foreach (var programare in _clinica.ProgramariReadOnly)
            {
                if(programare.Medic.Email == _medic.Email && programare.Vazut == false)
                    txtIstoric.AppendText(programare.ToString() + Environment.NewLine);
            }
        }
    }

    private void UI_Diagnostic()
    {
        panelDiagnostic = new Panel { Dock = DockStyle.Fill, Visible = false };
        
        Label lblPacient = new Label { Text = "Selectează Programare:", Top = 20, Left = 10, AutoSize = true };
        cmbPacienti = new ComboBox { Top = 20, Left = 150, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

        ActualizeazaComboDiagnostic();

        Label lblDiag = new Label { Text = "Diagnostic:", Top = 60, Left = 10, AutoSize = true };
        txtDiagnostic = new TextBox { 
            Top = 80, Left = 10, Width = 400, Height = 150, 
            Multiline = true, ScrollBars = ScrollBars.Vertical 
        };

        btnSalveazaDiagnostic = new Button {
            Text = "Salvează Diagnostic", Top = 240, Left = 10, Width = 150, 
            BackColor = Color.LightGreen, FlatStyle = FlatStyle.Flat
        };

        btnSalveazaDiagnostic.Click += (s, e) => {
            if (cmbPacienti.SelectedItem == null || string.IsNullOrWhiteSpace(txtDiagnostic.Text))
            {
                _logger?.LogWarning($"[VALIDARE ESUATA] Medicul {_medic.Email} a incercat salvarea unui diagnostic fara a completa toate datele.");
                MessageBox.Show("Te rugăm să selectezi un pacient și să introduci diagnosticul!");
                return;
            }

            _medic.GiveDiagnostic(cmbPacienti.SelectedItem.ToString(), txtDiagnostic.Text);
            
            _clinica.SalvareInFisier();
            ActualizeazaComboDiagnostic();
            ActualizareIstoric();
            MessageBox.Show("Diagnostic salvat cu succes!");
            txtDiagnostic.Clear();
        };

        panelDiagnostic.Controls.AddRange(new Control[] { lblPacient, cmbPacienti, lblDiag, txtDiagnostic, btnSalveazaDiagnostic });
    }

    private void ActualizeazaComboDiagnostic()
    {
        try
        {
            cmbPacienti.Items.Clear();
            foreach (var programare in _clinica.ProgramariReadOnly)
            {
                if (programare.Medic.Email == _medic.Email && programare.Vazut == false)
                {
                    cmbPacienti.Items.Add(programare.Pacient.Email + " - " + programare.DataOra);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Eroare la actualizarea combobox-ului: {ex.Message}");
            MessageBox.Show("Eroare la actualizarea combobox-ului!");
        }
        
    }
    
    private void SchimbaPanel(Panel panelActiv)
    {
        panelIstoric.Visible = false;
        panelDiagnostic.Visible = false;
        panelActiv.Visible = true;
        panelActiv.BringToFront();
    }

    private void btnLogout_Click(object sender, EventArgs e) {
        _authService.Logout();
        var mainFomr = _serviceProvider.GetRequiredService<MainForm>();
        mainFomr.Show();
        Hide();
    }
}