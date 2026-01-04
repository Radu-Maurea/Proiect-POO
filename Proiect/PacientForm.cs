using System;
using System.Drawing; 
using System.Windows.Forms;

namespace Proiect;

public class PacientForm : Form
{
    private Pacient pacient;
    private Clinica clinica;
    
    public void InitializeUI()
    {
        this.Text = "Medic Panel";
        this.Width = 500; 
        this.Height = 400;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(240, 248, 255);
    }

    public PacientForm(Clinica clinica, Pacient pacient)
    {
        this.clinica = clinica;
        this.pacient = pacient;
        InitializeUI();
    }
}