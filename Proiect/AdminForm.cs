using System;
using System.Drawing; // Asigură-te că ai această referință pentru Color și Font
using System.Windows.Forms;

namespace Proiect;

public class AdminForm : Form
{
    private Admin admin;
    private Clinica clinica;
    
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
            BackColor = Color.FromArgb(45, 45, 48), // Un gri închis profesional
            Dock = DockStyle.Left,
            Width = 180
        };

        // Funcție ajutătoare pentru a crea butoane stilizate rapid
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

        // Adăugăm butoanele în panelul lateral
        sidePanel.Controls.Add(btnAdauga);
        sidePanel.Controls.Add(btnSterge);
        sidePanel.Controls.Add(btnModifica);

        // 4. Adăugăm panel-urile în form (Ordinea contează!)
        this.Controls.Add(sidePanel); // Adăugăm meniul lateral
        
        this.Controls.Add(headerPanel);

    }

    public AdminForm(Clinica clinica, Admin admin)
    {
        this.clinica = clinica;
        this.admin = admin;
        InitializeUI();
    }
}