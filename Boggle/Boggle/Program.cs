using System;
using System.Collections.Generic;
using System.IO;
using BoggleGame;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("=== Jeu de Boggle ===\n");

            Console.Write("Nombre de joueurs (2 minimum): ");
            if (!int.TryParse(Console.ReadLine(), out int nbJoueurs) || nbJoueurs < 2)
                throw new ArgumentException("Il faut au moins 2 joueurs !");

            Console.Write("Durée de la partie en minutes (par défaut 6): ");
            int dureePartie = LireEntierAvecDefaut(6);
            Console.Write("Durée d'un tour en secondes (par défaut 60): ");
            int dureeTour = LireEntierAvecDefaut(60);

            Random r = new Random();
            Jeu jeu = new Jeu(dureePartie, dureeTour, r);
            GestionTemps gestionTemps = new GestionTemps(dureePartie, dureeTour);

            for (int i = 0; i < nbJoueurs; i++)
            {
                Console.Write($"Nom du joueur {i + 1}: ");
                string? nom = Console.ReadLine();
                if (!string.IsNullOrEmpty(nom))
                    jeu.AjouterJoueur(nom);
            }

            List<De> des = CreerDes(r);

            Console.Write("Choisissez la langue (FR/EN, défaut FR): ");
            string langue = Console.ReadLine()?.ToUpper() ?? "FR";
            string fichierDictionnaire = langue == "EN" ? "MOTS_ANGLAIS.txt" : "MOTS_FRANCAIS.txt";

            jeu.InitialiserJeu(langue, fichierDictionnaire, 4, des);

            Console.WriteLine("\nLa partie commence !");
            Console.WriteLine($"Durée de la partie: {dureePartie} minutes");
            Console.WriteLine($"Durée d'un tour: {dureeTour} secondes");
            Console.WriteLine("\nAppuyez sur une touche pour commencer...");
            Console.ReadKey(true);

            gestionTemps.DemarrerPartie();
            jeu.Jouer();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur: {ex.Message}");
        }
    }

    private static int LireEntierAvecDefaut(int defaut)
    {
        string? input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int resultat) ? defaut : resultat;
    }

    static Dictionary<char, (int Nombre, int Poids)> LireFichierLettres()
    {
        var lettres = new Dictionary<char, (int Nombre, int Poids)>();

        string[] lignes = File.ReadAllLines("Lettres.txt");
        foreach (string ligne in lignes)
        {
            if (string.IsNullOrWhiteSpace(ligne) || ligne.StartsWith("//")) continue; // Si y'a des commentaires au début du fichier bg (si tu captes pas pourquoi j'ai mis "//")

            string[] parts = ligne.Split(',');
            if (parts.Length == 3)
            {
                char lettre = parts[0][0];
                int nombre = int.Parse(parts[1]);
                int poids = int.Parse(parts[2]);
                lettres.Add(lettre, (nombre, poids));
            }
        }

        return lettres;
    }

    static List<De> CreerDes(Random random)
    {
        var lettresConfig = LireFichierLettres();
        var lettresDisponibles = new Dictionary<char, int>();
        foreach (var kvp in lettresConfig)
        {
            lettresDisponibles[kvp.Key] = kvp.Value.Nombre;
        }

        List<De> des = new List<De>();
        int taillePlateau = 4;
        int nbDes = taillePlateau * taillePlateau;

        for (int i = 0; i < nbDes; i++)
        {
            char[] faces = new char[6];
            for (int j = 0; j < 6; j++)
            {
                var lettresValides = new List<char>();
                foreach (var kvp in lettresDisponibles)
                {
                    if (kvp.Value > 0) lettresValides.Add(kvp.Key);
                }

                if (lettresValides.Count == 0)
                {
                    var lettresCommunes = lettresConfig
                        .OrderByDescending(l => l.Value.Poids)
                        .Take(7)
                        .Select(l => l.Key)
                        .ToArray();
                    faces[j] = lettresCommunes[random.Next(lettresCommunes.Length)];
                }
                else
                {
                    int totalPoids = 0;
                    foreach (var l in lettresValides)
                        totalPoids += lettresConfig[l].Poids;

                    int selection = random.Next(totalPoids);
                    int accumulateur = 0;

                    foreach (char lettre in lettresValides)
                    {
                        accumulateur += lettresConfig[lettre].Poids;
                        if (selection < accumulateur)
                        {
                            faces[j] = lettre;
                            lettresDisponibles[lettre]--;
                            break;
                        }
                    }
                }
            }
            des.Add(new De(faces));
        }

        return des;
    }
}
