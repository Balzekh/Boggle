using System;
using System.Collections.Generic;

namespace BoggleGame
{
    /// <summary>
    /// Représente un joueur avec son nom, score, et les mots trouvés.
    /// </summary>
    public class Joueur
    {
        private string nom;
        private int score;
        private List<string> motsTrouves;

        public Joueur(string nom)
        {
            if (string.IsNullOrEmpty(nom))
                throw new ArgumentException("Le nom du joueur ne peut pas être vide");

            this.nom = nom;
            this.score = 0;
            this.motsTrouves = new List<string>();
        }

        public bool ContientMot(string mot)
        {
            return motsTrouves.Contains(mot.ToUpper());
        }

        public void AjouterMot(string mot)
        {
            if (!string.IsNullOrEmpty(mot) && !ContientMot(mot))
                motsTrouves.Add(mot.ToUpper());
        }

        public override string ToString()
        {
            return $"Joueur: {nom}, Score: {score}, Mots trouvés: {motsTrouves.Count}";
        }

        public string toString()
        {
            return this.ToString();
        }

        public string Nom { get => nom; }
        public int Score { get => score; set => score = value; }
        public List<string> MotsTrouves { get => motsTrouves; }
    }
}
