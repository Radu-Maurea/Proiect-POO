using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Proiect;

    public class Clinica
    {
        private const string FileName = @"F:\info\an_2\Proiect POO\Proiect\Proiect\utilizatori.json";

        public List<User> utilizatori = new List<User>();
        
        public IReadOnlyList<User> UtilizatoriReadOnly => utilizatori.AsReadOnly();
    
        public Clinica()
        {
            IncarcaDinFisier();
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

        private void SalvareInFisier()
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
                utilizatori = JsonSerializer.Deserialize<List<User>>(jsonString);
            }
            catch (Exception ex)
            {
                utilizatori = new List<User>();
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
    }
