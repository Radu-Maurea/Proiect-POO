using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace Proiect;

public class PacientForm : Form
{
    private Pacient pacient;
    private Clinica clinica;

    private Panel mainContentPanel, panelCautare, panelCautareSpecializare, panelProgramare,panelIstoric,panelModificare;
    private TextBox txtCautareNume, txtRezultat, txtRezultatSpecialitate,txtIstoric;
    private ComboBox cmbSelectieSpecializare, cmbSelectieDoctor, cmbSelectieServiciu, cmbSelectieOra,cmbSelectieReprogramare,cmbModificareOra,cmbModificareDoctor;
    private Button btnConfirmaProgramarea,btnStergeProgramare,btnReprogramare;
    private ComboBox cmbModifDoctor, cmbModifServiciu, cmbModifOra;
    private Label lblModifDoctor, lblModifServiciu, lblModifOra;
    private Button btnSalveazaModificare;
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
        this.Width = 750;
        this.Height = 500;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(240, 248, 255);

        Panel headerPanel = new Panel { BackColor = Color.DarkBlue, Dock = DockStyle.Top, Height = 80 };
        Label lbl = new Label {
            Text = $"Autentificat ca: {pacient.Email}",
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
        
        mainContentPanel.Controls.AddRange(new Control[] { panelCautare, panelCautareSpecializare, panelProgramare,panelIstoric,panelModificare});

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
        
        Button btnLogout = CreateMenuButton("Logout", 270);
        btnLogout.Click += btnLogout_Click;

        sidePanel.Controls.AddRange(new Control[] { btnCauta, btnCautaSpecialitate, btnProgramare, btnVeziProg ,btnStergeProg ,btnLogout });
        
        this.Controls.AddRange(new Control[] { mainContentPanel ,sidePanel,headerPanel});
        // this.Controls.Add(mainContentPanel);
        // this.Controls.Add(sidePanel);
        // this.Controls.Add(headerPanel);
    }

    private void SchimbaPanel(Panel panelActiv)
    {
        panelCautare.Visible = false;
        panelCautareSpecializare.Visible = false;
        panelProgramare.Visible = false;
        panelIstoric.Visible = false;
        panelModificare.Visible = false;
        panelActiv.Visible = true;
        panelActiv.BringToFront();
    }

    public void UI_CautareNume()
    {
        panelCautare = new Panel { Dock = DockStyle.Fill, Visible = false };
        Label lblNume = new Label { Text = "Nume Medic:", Top = 20, Left = 10, AutoSize = true };
        txtCautareNume = new TextBox { Top = 20, Left = 120, Width = 200 };
        Button btnExecuta = new Button { Text = "Caută", Top = 18, Left = 330, Width = 80, BackColor = Color.LightBlue, FlatStyle = FlatStyle.Flat };
        btnExecuta.Click += btnExecutaCautare_Click;
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
        btnExecuta.Click += btnExecutaCautareSpec_Click;
        txtRezultatSpecialitate = new TextBox { Top = 80, Left = 10, Width = 450, Height = 250, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Font = new Font("Consolas", 10) };
        panelCautareSpecializare.Controls.AddRange(new Control[] { lblSpec, cmbSelectieSpecializare, btnExecuta, txtRezultatSpecialitate });
    }

    public void UI_CreereProgramare()
    {
        panelProgramare = new Panel { Dock = DockStyle.Fill, Visible = false };

        Label lblDoc = new Label { Text = "Doctor:", Top = 20, Left = 10, AutoSize = true };
        cmbSelectieDoctor = new ComboBox { Top = 20, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        foreach (var medic in clinica.Medici) { cmbSelectieDoctor.Items.Add(medic.Nume); }

        Label lblServ = new Label { Text = "Serviciu:", Top = 65, Left = 10, AutoSize = true, Visible = false };
        cmbSelectieServiciu = new ComboBox { Top = 60, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, Visible = false };

        Label lblOra = new Label { Text = "Ora:", Top = 105, Left = 10, AutoSize = true, Visible = false };
        cmbSelectieOra = new ComboBox { Top = 100, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, Visible = false };

        btnConfirmaProgramarea = new Button { Text = "Confirmă Programarea", Top = 150, Left = 120, Width = 200, Height = 35, BackColor = Color.LightGreen, Visible = false, FlatStyle = FlatStyle.Flat };
        
        // LEGARE REPARATA: Apelam metoda de procesare
        btnConfirmaProgramarea.Click += (s, e) => ProceseazaProgramareNoua();

        cmbSelectieDoctor.SelectedIndexChanged += (s, e) =>
        {
            var medicSelectat = clinica.Medici.FirstOrDefault(m => m.Nume == cmbSelectieDoctor.SelectedItem.ToString());
            if (medicSelectat != null)
            {
                cmbSelectieServiciu.Items.Clear();
                cmbSelectieOra.Items.Clear();

                foreach (var serv in medicSelectat.ServiciiOferite) { cmbSelectieServiciu.Items.Add(serv.Denumire); }

                switch (medicSelectat.ProgramLucru)
                {
                    case "10:00 - 18:00":
                        cmbSelectieOra.Items.AddRange(new object[] { "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00" });
                        break;
                    case "18:00-12:00":
                        cmbSelectieOra.Items.AddRange(new object[] { "18:00", "19:00", "20:00", "21:00", "22:00", "23:00", "00:00", "01:00" });
                        break;
                    case "12:00-06:00":
                        cmbSelectieOra.Items.AddRange(new object[] { "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "00:00", "01:00", "02:00", "03:00", "04:00", "05:00" });
                        break;
                }

                lblServ.Visible = true;
                cmbSelectieServiciu.Visible = true;
                lblOra.Visible = true;
                cmbSelectieOra.Visible = true;
                btnConfirmaProgramarea.Visible = true;
            }
        };

        panelProgramare.Controls.AddRange(new Control[] { lblDoc, cmbSelectieDoctor, lblServ, cmbSelectieServiciu, lblOra, cmbSelectieOra, btnConfirmaProgramarea });
    }

    // METODA DE PROCESARE ADAUGATA
    private void ProceseazaProgramareNoua()
    {
        if (cmbSelectieDoctor.SelectedItem == null || cmbSelectieServiciu.SelectedItem == null || cmbSelectieOra.SelectedItem == null)
        {
            MessageBox.Show("Vă rugăm selectați Doctorul, Serviciul și Ora!");
            return;
        }

        var medic = clinica.Medici.FirstOrDefault(m => m.Nume == cmbSelectieDoctor.SelectedItem.ToString());
        var serviciu = medic?.ServiciiOferite.FirstOrDefault(s => s.Denumire == cmbSelectieServiciu.SelectedItem.ToString());
        string oraSelectata = cmbSelectieOra.SelectedItem.ToString();

        if (pacient.CreereProgramare(medic, serviciu, oraSelectata))
        {
            MessageBox.Show("Programare realizată cu succes!");
            clinica.IncarcaProgramariDinFisier();
            ActualizareIstoric();
            ActualizareReprogramari();
            SchimbaPanel(panelCautare);
        }
        else
        {
            MessageBox.Show("Eroare la salvarea programării!");
        }
    }

    private void btnExecutaCautare_Click(object sender, EventArgs e)
    {
        string nume = txtCautareNume.Text.Trim();
        if (string.IsNullOrEmpty(nume)) { MessageBox.Show("Introduceți un nume!"); return; }
        txtRezultat.Text = pacient.CautareDoctorNume(nume);
    }

    private void btnExecutaCautareSpec_Click(object sender, EventArgs e)
    {
        if (cmbSelectieSpecializare.SelectedItem == null) return;
        txtRezultatSpecialitate.Text = pacient.CautareDoctorSpecializare(cmbSelectieSpecializare.SelectedItem.ToString());
    }

    private void UI_Istoric()
    {
        panelIstoric = new Panel { Dock = DockStyle.Fill, Visible = false };
        
        Label lblIstoric = new Label { Text = "Istoric Programari:", Top = 20, Left = 10, AutoSize = true };
        txtIstoric = new TextBox { Top = 80, Left = 10, Width = 450, Height = 250, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Font = new Font("Consolas", 10) };
        txtIstoric.AppendText(pacient.VeziProgramari(this.pacient.Email));
        
        panelIstoric.Controls.AddRange(new Control[] { lblIstoric, txtIstoric });
        ActualizareIstoric();
    }

    private void ActualizareIstoric()
    {
        if (txtIstoric != null)
        {
            txtIstoric.Clear();
            txtIstoric.Text = pacient.VeziProgramari(this.pacient.Email);
        }
    }

    private void UI_Modificare()
    {
        panelModificare = new Panel { Dock = DockStyle.Fill, Visible = false };
        Label lblReprogramare = new Label { Text = "Selectati o programare::", Top = 20, Left = 10, AutoSize = true };
        cmbSelectieReprogramare = new ComboBox { Top = 20, Left = 180, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        
        ActualizareReprogramari();
        
        btnStergeProgramare = new Button
        {
            Text = "Sterge", Top = 60, Left = 100, Width = 80, BackColor = Color.Red, FlatStyle = FlatStyle.Flat
        };
        btnReprogramare = new Button
        {
            Text = "Modifica", Top = 60, Left = 220, Width = 80, BackColor = Color.LightBlue, FlatStyle = FlatStyle.Flat
        };
        btnStergeProgramare.Click += (s, e) => StergeProgramare(cmbSelectieReprogramare.SelectedItem.ToString());
        btnReprogramare.Click += (s, e) => ModificaraReprogramare(cmbSelectieReprogramare.SelectedItem.ToString());
        panelModificare.Controls.AddRange(new Control[] { lblReprogramare, cmbSelectieReprogramare,btnStergeProgramare,btnReprogramare});
    }

    private void StergeProgramare(string programare)
    {
        if (pacient.StergeProgramare(programare))
        {
            MessageBox.Show("Programarea a fost stearsa cu succes!");
            ActualizareReprogramari();
            ActualizareIstoric();
            clinica.SalvareProgramariInFisier();
            clinica.IncarcaProgramariDinFisier();
        }
        else
        {
            MessageBox.Show("A aparut o eroare");
        }
        
    }

    private void ActualizareReprogramari()
    {
        cmbSelectieReprogramare.Items.Clear();
        foreach (var programare in clinica.programari)
        {
            if(programare.Pacient.Email == pacient.Email)
                cmbSelectieReprogramare.Items.Add($"{programare.DataOra} || {programare.Medic.Email}");
        }
    }

    private void ModificaraReprogramare(string programareSelectata)
    {
    if (string.IsNullOrEmpty(programareSelectata))
    {
        MessageBox.Show("Selectați o programare din listă!");
        return;
    }

    // 1. Curățăm/Resetăm controalele de modificare dacă există deja
    if (btnSalveazaModificare == null)
    {
        lblModifDoctor = new Label { Text = "Doctor Nou:", Top = 100, Left = 10, AutoSize = true };
        cmbModifDoctor = new ComboBox { Top = 100, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        
        lblModifServiciu = new Label { Text = "Serviciu Nou:", Top = 140, Left = 10, AutoSize = true };
        cmbModifServiciu = new ComboBox { Top = 140, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        
        lblModifOra = new Label { Text = "Ora Noua:", Top = 180, Left = 10, AutoSize = true };
        cmbModifOra = new ComboBox { Top = 180, Left = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

        btnSalveazaModificare = new Button { 
            Text = "Salvează Modificarea", Top = 220, Left = 120, Width = 200, 
            BackColor = Color.Gold, FlatStyle = FlatStyle.Flat 
        };

        // Event pentru schimbarea doctorului în procesul de modificare
        cmbModifDoctor.SelectedIndexChanged += (s, e) => {
            var medic = clinica.Medici.FirstOrDefault(m => m.Nume == cmbModifDoctor.SelectedItem.ToString());
            if (medic != null) {
                cmbModifServiciu.Items.Clear();
                foreach (var serv in medic.ServiciiOferite) cmbModifServiciu.Items.Add(serv.Denumire);
                
                cmbModifOra.Items.Clear();
                // Folosim logica ta de intervale orare
                if (medic.ProgramLucru == "10:00 - 18:00") cmbModifOra.Items.AddRange(new object[] { "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00" });
                else if (medic.ProgramLucru == "18:00-12:00") cmbModifOra.Items.AddRange(new object[] { "18:00", "19:00", "20:00", "21:00", "22:00", "23:00", "00:00", "01:00" });
                else cmbModifOra.Items.AddRange(new object[] { "12:00", "13:00", "14:00", "15:00", "16:00", "17:00" });
            }
        };

        panelModificare.Controls.AddRange(new Control[] { lblModifDoctor, cmbModifDoctor, lblModifServiciu, cmbModifServiciu, lblModifOra, cmbModifOra, btnSalveazaModificare });
    }

    // 2. Populăm lista de doctori
    cmbModifDoctor.Items.Clear();
    foreach (var m in clinica.Medici) cmbModifDoctor.Items.Add(m.Nume);

    // 3. Logica de salvare
    btnSalveazaModificare.Click += (s, e) => {
        if (cmbModifDoctor.SelectedItem == null || cmbModifServiciu.SelectedItem == null || cmbModifOra.SelectedItem == null) {
            MessageBox.Show("Selectați noile date!");
            return;
        }

        // Ștergem programarea veche folosind metoda ta existentă
        if (pacient.StergeProgramare(programareSelectata)) {
            // Creăm programarea nouă
            var medic = clinica.Medici.FirstOrDefault(m => m.Nume == cmbModifDoctor.SelectedItem.ToString());
            var serviciu = medic?.ServiciiOferite.FirstOrDefault(ser => ser.Denumire == cmbModifServiciu.SelectedItem.ToString());
            
            if (pacient.CreereProgramare(medic, serviciu, cmbModifOra.SelectedItem.ToString())) {
                MessageBox.Show("Programare modificată cu succes!");
                ActualizareReprogramari();
                ActualizareIstoric();
                // Ascundem controalele după succes
                lblModifDoctor.Visible = lblModifServiciu.Visible = lblModifOra.Visible = false;
                cmbModifDoctor.Visible = cmbModifServiciu.Visible = cmbModifOra.Visible = false;
                btnSalveazaModificare.Visible = false;
            }
        }
    };

    lblModifDoctor.Visible = lblModifServiciu.Visible = lblModifOra.Visible = true;
    cmbModifDoctor.Visible = cmbModifServiciu.Visible = cmbModifOra.Visible = true;
    btnSalveazaModificare.Visible = true;
    }
    private void btnLogout_Click(object sender, EventArgs e)
    {
        this.Hide();
        new MainForm().Show();
        this.Close();
    }
}