using System;
using System.Drawing; 
using System.Windows.Forms;

namespace Proiect;

public class MedicForm : Form
{
    private Medic medic;
    private Clinica clinica;
    
    public void InitializeUI()
    {
        this.Text = "Medic Panel";
        this.Width = 500; 
        this.Height = 400;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(240, 248, 255);
    }

    public MedicForm(Clinica clinica, Medic medic)
    {
        this.clinica = clinica;
        this.medic = medic;
        InitializeUI();
    }
    
}