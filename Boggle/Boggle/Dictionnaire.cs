using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BoggleGame
{
    /// <summary>
    /// Gère le dictionnaire de mots (FR ou EN), 
    /// et permet de vérifier l'existence d'un mot.
    /// </summary>
    public class Dictionnaire
    {
        private string langue;
        private HashSet<string> motsSet;
        private string[] motsTries;

        public Dictionnaire(string langue, string fichier)
        {
            this.langue = langue;
            this.motsSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            ChargerMots(fichier);
        }

        private void ChargerMots(string fichier)
        {
            try
            {
                motsSet.Clear();
                List<string> listeMots = new List<string>();

                foreach (var ligne in File.ReadLines(fichier))
                {
                    var mots = ligne.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var mot in mots)
                    {
                        string motNettoye = mot.Trim().ToUpper();
                        if (!string.IsNullOrWhiteSpace(motNettoye))
                        {
                            motsSet.Add(motNettoye);
                            listeMots.Add(motNettoye);
                        }
                    }
                }

                motsTries = listeMots.Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(m => m).ToArray();

                Console.WriteLine($"Dictionnaire chargé : {motsSet.Count} mots");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement du dictionnaire : {ex.Message}");
                throw;
            }
        }

        public bool MotExiste(string mot)
        {
            if (string.IsNullOrWhiteSpace(mot)) return false;
            mot = mot.ToUpper().Trim();
            return RechDichoRecursif(mot, 0, motsTries.Length - 1);
        }

        /// <summary>
        /// Recherche dichotomique récursive d'un mot dans le tableau motsTries.
        /// </summary>
        public bool RechDichoRecursif(string mot, int debut, int fin)
        {
            if (debut > fin) return false;
            int milieu = (debut + fin) / 2;
            int cmp = string.Compare(mot, motsTries[milieu], StringComparison.OrdinalIgnoreCase);
            if (cmp == 0) return true;
            else if (cmp < 0) return RechDichoRecursif(mot, debut, milieu - 1);
            else return RechDichoRecursif(mot, milieu + 1, fin);
        }

        public override string ToString()
        {
            return $"Dictionnaire {langue} : {motsTries.Length} mots";
        }

        public string toString()
        {
            return this.ToString();
        }
    }
}
