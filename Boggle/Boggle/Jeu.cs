using BoggleGame;
using System;
using System.Collections.Generic;

public class Jeu
{
    private List<Joueur> joueurs;
    private Plateau plateau;
    private Dictionnaire dictionnaire;
    private int dureePartieMinutes;
    private int dureeTourSecondes;
    private readonly Dictionary<char, int> poidsLettres;
    private Random random;

    public Jeu(int dureePartieMinutes, int dureeTourSecondes, Random r)
    {
        this.joueurs = new List<Joueur>();
        this.dureePartieMinutes = dureePartieMinutes;
        this.dureeTourSecondes = dureeTourSecondes;
        this.random = r;
        this.poidsLettres = new Dictionary<char, int>
        {
            {'A', 1}, {'B', 3}, {'C', 3}, {'D', 2}, {'E', 1}, {'F', 4}, {'G', 2},
            {'H', 4}, {'I', 1}, {'J', 8}, {'K', 10}, {'L', 1}, {'M', 2}, {'N', 1},
            {'O', 1}, {'P', 3}, {'Q', 8}, {'R', 1}, {'S', 1}, {'T', 1}, {'U', 1},
            {'V', 4}, {'W', 10}, {'X', 10}, {'Y', 10}, {'Z', 10}
        };
    }

    public void InitialiserJeu(string langue, string fichierDictionnaire, int taillePlateau, List<De> des)
    {
        dictionnaire = new Dictionnaire(langue, fichierDictionnaire);
        plateau = new Plateau(taillePlateau);
        plateau.SetDictionnaire(dictionnaire);
        plateau.InitialiserPlateau(des, random);
    }

    public void AjouterJoueur(string nom)
    {
        joueurs.Add(new Joueur(nom));
    }

    public void Jouer()
    {
        DateTime debutPartie = DateTime.Now;
        bool partieTerminee = false;

        while (!partieTerminee)
        {
            TimeSpan tempsEcoule = DateTime.Now - debutPartie;
            if (tempsEcoule.TotalMinutes >= dureePartieMinutes)
            {
                // Temps de la partie écoulé, on arrête
                partieTerminee = true;
                break;
            }

            foreach (Joueur joueur in joueurs)
            {
                tempsEcoule = DateTime.Now - debutPartie;
                if (tempsEcoule.TotalMinutes >= dureePartieMinutes)
                {
                    partieTerminee = true;
                    break;
                }

                Console.Clear();
                JouerTour(joueur, debutPartie);
                if ((DateTime.Now - debutPartie).TotalMinutes >= dureePartieMinutes)
                {
                    // On re-vérifie après le tour
                    partieTerminee = true;
                    break;
                }

                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
                else
                {
                    // On attend une touche
                    Console.ReadKey(true);
                }
            }
        }

        Console.Clear();
        AfficherResultats();
        Console.WriteLine("\nAppuyez sur une touche pour quitter...");
        Console.ReadKey();
    }

    private void JouerTour(Joueur joueur, DateTime debutPartie)
    {
        // Relancer les dés
        for (int i = 0; i < plateau.Des.GetLength(0); i++)
        {
            for (int j = 0; j < plateau.Des.GetLength(1); j++)
            {
                plateau.Des[i, j].Lance(random);
            }
        }

        Console.WriteLine($"\nTour de {joueur.Nom}:");
        Console.WriteLine(plateau.toString());

        DateTime debutTour = DateTime.Now;
        bool tourTermine = false;
        string dernierMot = null;

        while (!tourTermine)
        {
            // Vérification du temps du tour
            TimeSpan tempsEcoule = DateTime.Now - debutTour;
            if (tempsEcoule.TotalSeconds >= dureeTourSecondes)
            {
                if (dernierMot != null)
                {
                    TraiterMot(dernierMot, joueur);
                }
                Console.WriteLine("\nTemps écoulé !");
                tourTermine = true;
                continue;
            }

            int tempsRestant = dureeTourSecondes - (int)tempsEcoule.TotalSeconds;
            Console.Write($"\rTemps restant : {tempsRestant} s | Entrez un mot (ou 'fin' pour terminer): ");

            string mot = LireMotAvecTimeout(debutTour, dureeTourSecondes);
            dernierMot = mot;

            if (mot == null)
            {
                // Le temps s'est écoulé pendant la saisie
                Console.WriteLine("\nTemps écoulé pendant la saisie !");
                tourTermine = true;
                continue;
            }

            if (string.IsNullOrEmpty(mot) || mot.ToLower() == "fin")
                break;

            if (mot.Length < 2)
            {
                Console.WriteLine("Le mot doit faire au moins 2 lettres.");
                continue;
            }

            if (joueur.ContientMot(mot))
            {
                Console.WriteLine("Vous avez déjà trouvé ce mot !");
                continue;
            }

            TraiterMot(mot, joueur);
        }
    }

    private string LireMotAvecTimeout(DateTime debutTour, int dureeTourSecondes)
    {
        List<char> buffer = new List<char>();
        while (true)
        {
            TimeSpan tempsEcoule = DateTime.Now - debutTour;
            if (tempsEcoule.TotalSeconds >= dureeTourSecondes)
            {
                // Temps du tour écoulé
                return null;
            }

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return new string(buffer.ToArray());
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (buffer.Count > 0)
                    {
                        buffer.RemoveAt(buffer.Count - 1);
                        ReafficherLigne(buffer, debutTour, dureeTourSecondes);
                    }
                }
                else
                {
                    buffer.Add(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                }
            }
            else
            {
                System.Threading.Thread.Sleep(50);
            }
        }
    }

    private void ReafficherLigne(List<char> buffer, DateTime debutTour, int dureeTourSecondes)
    {
        // Efface la ligne
        Console.Write("\r" + new string(' ', Console.BufferWidth - 1) + "\r");

        TimeSpan tempsEcoule = DateTime.Now - debutTour;
        int tempsRestant = Math.Max(0, dureeTourSecondes - (int)tempsEcoule.TotalSeconds);
        Console.Write($"Temps restant : {tempsRestant} s | Entrez un mot (ou 'fin' pour terminer): ");
        Console.Write(new string(buffer.ToArray()));
    }

    private void TraiterMot(string mot, Joueur joueur)
    {
        if (plateau.Test_Plateau(mot))
        {
            joueur.AjouterMot(mot);
            CalculerScore(joueur, mot);
            Console.WriteLine($"Mot valide! Score: {joueur.Score}");
        }
        else
        {
            Console.WriteLine("Mot invalide!");
        }
    }

    private void CalculerScore(Joueur joueur, string mot)
    {
        int points = 0;
        foreach (char lettre in mot.ToUpper())
        {
            if (poidsLettres.ContainsKey(lettre))
            {
                points += poidsLettres[lettre];
            }
        }
        points += mot.Length;
        joueur.Score += points;
    }

    private void AfficherResultats()
    {
        Console.WriteLine("\n=== Fin de la partie ===");
        joueurs.Sort((j1, j2) => j2.Score.CompareTo(j1.Score));

        foreach (Joueur joueur in joueurs)
        {
            Console.WriteLine($"\n{joueur.Nom}:");
            Console.WriteLine($"Score final: {joueur.Score}");
            Console.WriteLine("Mots trouvés:");
            foreach (string mot in joueur.MotsTrouves)
            {
                Console.WriteLine($"- {mot}");
            }

            try
            {
                var nuage = new NuageDeMots(joueur.MotsTrouves);
                string nomFichier = $"nuage_mots_{joueur.Nom}_{DateTime.Now:yyyyMMddHHmmss}.png";
                nuage.GenererNuage(nomFichier);
                Console.WriteLine($"\nNuage de mots généré : {nomFichier}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErreur lors de la génération du nuage de mots : {ex.Message}");
            }
        }

        Console.WriteLine($"\nLe gagnant est {joueurs[0].Nom} avec {joueurs[0].Score} points!");
    }
}
