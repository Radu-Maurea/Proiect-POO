using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Proiect;

    public class Clinica
    {
        private const string FileName = @"F:\info\an_2\Proiect POO\Proiect\Proiect\utilizatori.json";
        private const string ServiciiFileName = @"F:\info\an_2\Proiect POO\Proiect\Proiect\servicii.json";
        private const string ProgramariFileName = @"F:\info\an_2\Proiect POO\Proiect\Proiect\programari.json";
        
        public List<User> utilizatori = new List<User>();
        public List<ServiciuMedical> servicii = new List<ServiciuMedical>();
        public List<Programare> programari = new List<Programare>();
        public IReadOnlyList<User> UtilizatoriReadOnly => utilizatori.AsReadOnly();
        
        public List<Medic> Medici => utilizatori.Where(u => u is Medic).Cast<Medic>().ToList();
        public List<Pacient> Pacienti => utilizatori.Where(u => u is Pacient).Cast<Pacient>().ToList();
        
        public Clinica()
        {
            IncarcaDinFisier();
            IncarcaServiciiDinFisier();
        }
        
        public void SalvareServiciiInFisier()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(servicii, options);
                File.WriteAllText(ServiciiFileName, jsonString);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Eroare la salvarea serviciilor: " + ex.Message);
            }
        }

        private void IncarcaServiciiDinFisier()
        {
            if (!File.Exists(ServiciiFileName)) return;
            try
            {
                string jsonString = File.ReadAllText(ServiciiFileName);
                servicii = JsonSerializer.Deserialize<List<ServiciuMedical>>(jsonString) ?? new List<ServiciuMedical>();
            }
            catch (Exception)
            {
                servicii = new List<ServiciuMedical>();
            }
        }
        
        public void AdaugaUtilizator(User user)
        {
            utilizatori.Add(user);
            SalvareInFisier();
        }

        public bool ExistaEmail(string email)
        {
            email = email.Trim().ToLower();
            foreach (var user in utilizatori)
            {
                if (user.Email.Trim().ToLower() == email)
                    return true;
            }
            return false;
        }

        public bool VerificareLogin(string email, string password)
        {
            email = email.Trim().ToLower();
            foreach (var user in utilizatori)
            {
                if (user.Email.Trim().ToLower() == email && user.Password == password)
                    return true;
            }
            return false;
        }

        public void SalvareInFisier()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(utilizatori, options);
                File.WriteAllText(FileName, jsonString);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            
        }

        private void IncarcaDinFisier()
        {
            if (!File.Exists(FileName)) return;
            try
            {
                string jsonString = File.ReadAllText(FileName);
                var options = new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                };
                
                // Atributele [JsonDerivedType] din clasa User fac toata treaba aici:
                utilizatori = JsonSerializer.Deserialize<List<User>>(jsonString, options) ?? new List<User>();
            }
            catch (Exception ex)
            {
                utilizatori = new List<User>();
                System.Windows.Forms.MessageBox.Show("Eroare la încărcare JSON: " + ex.Message);
            }
        }
        
        
        public User GetUserLogin(string email, string password)
        {
            email = email.Trim().ToLower();
            foreach (var user in utilizatori)
            {
                if (user.Email.Trim().ToLower() == email && user.Password == password)
                    return user;
            }
            return null;
        }
        
        public bool StergeUtilizatorDupaEmail(string email)
        {
            email = email.Trim().ToLower();
    
           
            var user = utilizatori.FirstOrDefault(u => u.Email.Trim().ToLower() == email && u is Medic);

            if (user != null)
            {
                utilizatori.Remove(user); 
                SalvareInFisier();       
                return true;
            }
    
            return false;
        }
        
        
        

        public void AdaugaProgramare(Programare p)
        {
            programari.Add(p);
            SalvareProgramariInFisier();
        }

        public void SalvareProgramariInFisier()
        {
            try
            {
                var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
                string json = System.Text.Json.JsonSerializer.Serialize(programari, options);
                System.IO.File.WriteAllText(ProgramariFileName, json);
            }
            catch (Exception ex) { MessageBox.Show("Eroare salvare programare: " + ex.Message); }
        }

        public void IncarcaProgramariDinFisier()
        {
            if (!File.Exists(ProgramariFileName)) return;
            try
            {
                string jsonString = File.ReadAllText(ProgramariFileName);
                var options = new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                };
                
                programari = JsonSerializer.Deserialize<List<Programare>>(jsonString, options) ?? new List<Programare>();
            }
            catch (Exception ex)
            {
                programari = new List<Programare>();
                System.Windows.Forms.MessageBox.Show("Eroare la încărcare JSON: " + ex.Message);
            }
        }
        
    }

