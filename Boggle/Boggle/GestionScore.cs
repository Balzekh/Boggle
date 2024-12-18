using System;
using System.Collections.Generic;

namespace BoggleGame
{
    /// <summary>
    /// Gère le score en fonction des lettres et de la longueur du mot. (Chui un fdp je crois j'ai mis 2 fois le meme truc mais nsm)
    /// </summary>
    public class GestionScore
    {
        private static readonly Dictionary<char, int> PoidsLettres = new Dictionary<char, int>
        {
            {'A', 1}, {'B', 3}, {'C', 3}, {'D', 2}, {'E', 1}, {'F', 4}, {'G', 2}, {'H', 4},
            {'I', 1}, {'J', 8}, {'K', 10}, {'L', 1}, {'M', 2}, {'N', 1}, {'O', 1}, {'P', 3},
            {'Q', 8}, {'R', 1}, {'S', 1}, {'T', 1}, {'U', 1}, {'V', 4}, {'W', 10}, {'X', 10},
            {'Y', 10}, {'Z', 10}
        };

        public static int CalculerScoreMot(string mot)
        {
            if (string.IsNullOrEmpty(mot)) return 0;
            int score = 0;
            foreach (char lettre in mot.ToUpper())
            {
                if (PoidsLettres.ContainsKey(lettre))
                {
                    score += PoidsLettres[lettre];
                }
            }
            score += mot.Length;
            return score;
        }
    }

    /// <summary>
    /// Gère le temps de la partie et des tours.
    /// </summary>
    public class GestionTemps
    {
        private DateTime debutPartie;
        private DateTime debutTour;
        private readonly int dureePartieMinutes;
        private readonly int dureeTourSecondes;

        public GestionTemps(int dureePartieMinutes, int dureeTourSecondes)
        {
            this.dureePartieMinutes = dureePartieMinutes;
            this.dureeTourSecondes = dureeTourSecondes;
        }

        public void DemarrerPartie()
        {
            debutPartie = DateTime.Now;
        }

        public void DemarrerTour()
        {
            debutTour = DateTime.Now;
        }

        public bool EstPartieTerminee()
        {
            return (DateTime.Now - debutPartie).TotalMinutes >= dureePartieMinutes;
        }

        public bool EstTourTermine()
        {
            return (DateTime.Now - debutTour).TotalSeconds >= dureeTourSecondes;
        }

        public int TempsRestantTour()
        {
            int tempsEcoule = (int)(DateTime.Now - debutTour).TotalSeconds;
            return Math.Max(0, dureeTourSecondes - tempsEcoule);
        }

        public int TempsRestantPartie()
        {
            int tempsEcoule = (int)(DateTime.Now - debutPartie).TotalMinutes;
            return Math.Max(0, dureePartieMinutes - tempsEcoule);
        }
    }
}
