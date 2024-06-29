using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EAC2_Exercici1
{
    public partial class Form1 : Form
    {
        private List<Soci> llistaSocis;
        public Form1()
        {
            InitializeComponent();
            // Configurem la finestra perquè s'obri al centre i no es pugui redimensionar
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            llistaSocis = new List<Soci>();
            CarregarDades();
        }

        public void CarregarDades()
        {
            // Mètode per llegir les dades del fitxer i carregar-les en llistaSocis
            try
            {
                using (StreamReader sr = new StreamReader("dades.txt"))
                {
                    string linia;
                    while ((linia = sr.ReadLine()) != null)
                    {
                        string[] dades = linia.Split(':');
                        string[] activitats = dades[5].Split(',');
                        Soci nouSoci = new Soci(dades[0], dades[1], dades[2], dades[3], DateTime.Parse(dades[4]), activitats);
                        llistaSocis.Add(nouSoci);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en carregar les dades: " + ex.Message);
            }
        }

        public void DesarDades()
        {
            // Mètode per desar les dades dels socis al fitxer
            try
            {
                using (StreamWriter sw = new StreamWriter("dades.txt"))
                {
                    foreach (Soci soci in llistaSocis)
                    {
                        string linia = $"{soci.Nom}:{soci.Cognoms}:{soci.Telefon}:{soci.Email}:{soci.Matriculacio.ToShortDateString()}:{string.Join(",", soci.Activitats)}";
                        sw.WriteLine(linia);
                    }
                }
                MessageBox.Show("Les dades s'han emmagatzemat amb èxit!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en desar les dades: " + ex.Message);
            }
        }

        public void MostrarDadesSoci(Soci soci)
        {
            // Mètode per mostrar les dades d'un soci en els controls de la finestra
            textBoxNom.Text = soci.Nom;
            textBoxCognoms.Text = soci.Cognoms;
            textBoxTelefon.Text = soci.Telefon;
            textBoxEmail.Text = soci.Email;
            textBoxData.Text = soci.Matriculacio.ToShortDateString();
            listBoxActivitats.Items.Clear();
            foreach (string activitat in soci.Activitats)
            {
                listBoxActivitats.Items.Add(activitat);
            }
        }

        public void AfegirSoci()
        {
            // Mètode per afegir un nou soci a la llista
            string nom = textBoxANom.Text;
            string cognoms = textBoxACognoms.Text;
            string telefon = textBoxATelefon.Text;
            string email = textBoxAEmail.Text;
            DateTime matriculacio = dateTimePickerData.Value;

            List<string> activitatsSeleccionades = new List<string>();
            foreach (CheckBox checkBox in groupBoxActivitats.Controls.OfType<CheckBox>())
            {
                if (checkBox.Checked)
                {
                    activitatsSeleccionades.Add(checkBox.Text);
                }
            }
            string[] activitats = activitatsSeleccionades.ToArray();
            Soci nouSoci = new Soci(nom, cognoms, telefon, email, matriculacio, activitats);
            llistaSocis.Add(nouSoci);
            MessageBox.Show($"Soci {nom} afegit amb èxit!");
            DesarDades();
        }

        public void EliminarSoci()
        {
            // Mètode per eliminar el soci actualment mostrat
            string nomSociAEliminar = textBoxNom.Text;
            string cognomsSociAEliminar = textBoxCognoms.Text;
            Soci sociAEliminar = llistaSocis.Find(soci => soci.Nom == nomSociAEliminar && soci.Cognoms == cognomsSociAEliminar);
            if (sociAEliminar != null)
            {
                llistaSocis.Remove(sociAEliminar);
                MessageBox.Show($"Soci {nomSociAEliminar} {cognomsSociAEliminar} eliminat amb èxit!");
                DesarDades();
            }
        }

        private void buttonAfegir_Click(object sender, EventArgs e)
        {
            // Validar que els camps requerits no estiguin buits
            if (string.IsNullOrWhiteSpace(textBoxANom.Text) ||
                string.IsNullOrWhiteSpace(textBoxACognoms.Text) ||
                string.IsNullOrWhiteSpace(textBoxATelefon.Text) ||
                string.IsNullOrWhiteSpace(textBoxAEmail.Text))
            {
                MessageBox.Show("Si us plau, completi tots els camps obligatoris", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                AfegirSoci();
                //Netejar els camps del formulari
                textBoxANom.Text = "";
                textBoxACognoms.Text = "";
                textBoxATelefon.Text = "";
                textBoxAEmail.Text = "";
                dateTimePickerData.Value = DateTime.Now;

                //Desmarcar tots els CheckBox
                foreach (CheckBox checkBox in groupBoxActivitats.Controls.OfType<CheckBox>())
                {
                    checkBox.Checked = false;
                }
            }
        }

        private void buttonSeguent_Click(object sender, EventArgs e)
        {
            // Verifica si hi ha socis a la llista
            if (llistaSocis.Count == 0)
            {
                MessageBox.Show("No hi ha socis a la llista.");
            }
            else
            {
                int indexActual = llistaSocis.FindIndex(soci => soci.Nom == textBoxNom.Text && soci.Cognoms == textBoxCognoms.Text);
                // Augmenta l'índex actual si no estem al final de la llista
                if (indexActual < llistaSocis.Count - 1)
                {
                    MostrarDadesSoci(llistaSocis[indexActual + 1]);
                }
                else
                {
                    MessageBox.Show("Ja es troba al final de la llista.");
                }
            }
        }

        private void buttonAnterior_Click(object sender, EventArgs e)
        {
            // Verifica si hi ha socis a la llista
            if (llistaSocis.Count == 0)
            {
                MessageBox.Show("No hi ha socis a la llista.");
            }
            else
            {
                int indexActual = llistaSocis.FindIndex(soci => soci.Nom == textBoxNom.Text && soci.Cognoms == textBoxCognoms.Text);
                // Disminueix l'índex actual si no estem al principi de la llista
                if (indexActual > 0)
                {
                    MostrarDadesSoci(llistaSocis[indexActual - 1]);
                }
                else
                {
                    MessageBox.Show("Ja es troba al principi de la llista.");
                }
            }
        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            EliminarSoci();
        }

        private void buttonDesar_Click(object sender, EventArgs e)
        {
            DesarDades();
        }
    }

    public class Soci
    {
        private string nom;
        private string cognoms;
        private string telefon;
        private string email;
        private DateTime matriculacio;
        private string[] activitats;

        public Soci(string nom, string cognoms, string telefon, string email, DateTime matriculacio, string[] activitats)
        {
            this.nom = nom;
            this.cognoms = cognoms;
            this.telefon = telefon;
            this.email = email;
            this.matriculacio = matriculacio;
            this.activitats = activitats;
        }

        public string Nom { get { return nom; } }
        public string Cognoms { get { return cognoms; } }
        public string Telefon { get { return telefon; } }
        public string Email { get { return email; } }
        public DateTime Matriculacio { get { return matriculacio; } }
        public string[] Activitats { get { return activitats; } }
    }
}