using System;
using System.Drawing; 
using System.Windows.Forms;

namespace Proiect;

public class AdminForm : Form
{
    private Admin admin;
    private Clinica clinica;
    private TextBox txtDisplay; //Aici afisam cerintele 
    public void InitializeUI()
    {
        this.Text = "Admin Panel";
        this.Width = 500; 
        this.Height = 400;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(240, 248, 255);
        
        
        Panel headerPanel = new Panel
        {
            BackColor = Color.DarkBlue,
            Dock = DockStyle.Top,
            Height = 80 
        };

        Label lbl = new Label
        {
            Text = $"Autentificat ca: {admin.Email}",
            ForeColor = Color.White, 
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            Dock = DockStyle.Fill, 
            TextAlign = ContentAlignment.MiddleCenter 
        };
        
        headerPanel.Controls.Add(lbl);
        
        Panel sidePanel = new Panel
        {
            BackColor = Color.FromArgb(45, 45, 48), 
            Dock = DockStyle.Left,
            Width = 180
        };
        
        Panel mainContentPanel = new Panel {
            Dock = DockStyle.Fill,
            Padding = new Padding(20)
        };

        txtDisplay = new TextBox {
            Multiline = true,
            Dock = DockStyle.Fill,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Font = new Font("Consolas", 10), // Font mono-spaced pentru aliniere frumoasa
            BackColor = Color.White
        };
        mainContentPanel.Controls.Add(txtDisplay);
        
        
        Button CreateMenuButton(string text, int top)
        {
            return new Button
            {
                Text = text,
                Top = top,
                Left = 10,
                Width = 160,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(63, 63, 70),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
        }

        // 3. Crearea celor 3 butoane
        Button btnAdauga = CreateMenuButton("Adaugă Medici", 20);
        btnAdauga.Click += (s, e) => MessageBox.Show("Deschide formular adăugare...");

        Button btnSterge = CreateMenuButton("Șterge Medici", 70);
        btnSterge.Click += (s, e) => MessageBox.Show("Deschide logica ștergere...");

        Button btnModifica = CreateMenuButton("Modifică Medici", 120);
        btnModifica.Click += (s, e) => MessageBox.Show("Deschide logica modificare...");

        Button btnAfisare = CreateMenuButton("Afisare Conturi", 170);
        btnAfisare.Click += btnAfisare_Click;
        
        Button btnLogout = CreateMenuButton("Logout", 220);
        btnLogout.Click += btnLogout_Click;
        // Adăugăm butoanele în panelul lateral
        sidePanel.Controls.Add(btnAdauga);
        sidePanel.Controls.Add(btnSterge);
        sidePanel.Controls.Add(btnModifica);
        sidePanel.Controls.Add(btnAfisare);
        sidePanel.Controls.Add(btnLogout);
        // 4. Adăugăm panel-urile în form (Ordinea contează!)
        this.Controls.Add(mainContentPanel);
        this.Controls.Add(sidePanel); // Adăugăm meniul lateral
        
        this.Controls.Add(headerPanel);

    }
    private void btnAfisare_Click(object sender, EventArgs e)
    {
        txtDisplay.Clear();
        txtDisplay.AppendText("Conturile de utilizator:" + Environment.NewLine);
        txtDisplay.AppendText("=========================" + Environment.NewLine);
        
        foreach (var utilizator in clinica.UtilizatoriReadOnly)
        {
            txtDisplay.AppendText($"[{utilizator.Rol()}] - {utilizator.Email}" + Environment.NewLine);
        }
    }

    private void btnLogout_Click(object sender, EventArgs e)
    {
        this.Hide();
        MainForm mainForm = new MainForm();
        mainForm.ShowDialog();
    }
    public AdminForm(Clinica clinica, Admin admin)
    {
        this.clinica = clinica;
        this.admin = admin;
        InitializeUI();
    }
}