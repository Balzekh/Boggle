using System;
using System.Collections.Generic;
using System.IO;
using BoggleGame;

namespace Boggle
{
    public class TestsBoggle
    {
        public void TestJoueurAjoutMot()
        {
            Console.WriteLine("=== TestJoueurAjoutMot ===");
            Joueur j = new Joueur("Test");
            j.AjouterMot("TEST");

            bool condition = j.ContientMot("TEST");
            if (condition)
                Console.WriteLine("Test OK : Le joueur contient le mot 'TEST'.");
            else
                Console.WriteLine("Test ÉCHEC : Le joueur ne contient pas le mot 'TEST'.");
        }

        public void TestDictionnaireRechDicho()
        {
            Console.WriteLine("=== TestDictionnaireRechDicho ===");
            Dictionnaire d = new Dictionnaire("FR", "MOTS_FRANCAIS.txt");
            bool motConnu = d.MotExiste("AFFECTIONNEE");
            bool motInconnu = d.MotExiste("BANIE");

            if (motConnu && !motInconnu)
                Console.WriteLine("Test OK : 'AFFECTIONNEE' trouvé, 'BANIE' non trouvé.");
            else
                Console.WriteLine("Test ÉCHEC : Le dictionnaire ne se comporte pas comme attendu.");
        }

        public void TestDeLance()
        {
            Console.WriteLine("=== TestDeLance ===");
            De de = new De(new char[] { 'A', 'B', 'C', 'D', 'E', 'F' });
            Random r = new Random(0);
            de.Lance(r);
            // On vérifie que la face visible est parmi ABCDEF
            bool condition = "ABCDEF".Contains(de.FaceVisible);
            if (condition)
                Console.WriteLine("Test OK : Le dé a une face visible valide.");
            else
                Console.WriteLine("Test ÉCHEC : Le dé n'affiche pas une face valide.");
        }

        public void TestPlateauMotExistePas()
        {
            Console.WriteLine("=== TestPlateauMotExistePas ===");
            Plateau p = new Plateau(2);
            p.SetDictionnaire(new Dictionnaire("FR", "MOTS_FRANCAIS.txt"));
            var des = new List<De> {
                new De(new char[]{'A','A','A','A','A','A'}),
                new De(new char[]{'A','A','A','A','A','A'}),
                new De(new char[]{'B','B','B','B','B','B'}),
                new De(new char[]{'C','C','C','C','C','C'})
            };
            Random r = new Random(0);
            p.InitialiserPlateau(des, r);

            bool condition = !p.Test_Plateau("ZZZ"); // mot impossible
            if (condition)
                Console.WriteLine("Test OK : Le mot 'ZZZ' n'est pas trouvé, comme attendu.");
            else
                Console.WriteLine("Test ÉCHEC : Le mot 'ZZZ' a été trouvé alors qu'il ne devrait pas.");
        }

        public void TestScoreMot()
        {
            Console.WriteLine("=== TestScoreMot ===");
            int score = GestionScore.CalculerScoreMot("AZ");
            // A=1, Z=10, longueur=2 => total = 1+10+2=13
            bool condition = (score == 13);
            if (condition)
                Console.WriteLine("Test OK : Le score du mot 'AZ' est 13, comme attendu.");
            else
                Console.WriteLine($"Test ÉCHEC : Score obtenu {score}, attendu 13.");
        }

        public void TestNuageDeMots()
        {
            Console.WriteLine("=== TestNuageDeMots ===");
            var mots = new List<string>
            {
                "TEST", "TEST",
                "BABE", "IM", "CHASINGAGHOST",
                "OUIBAHOUI", "JEU",
                "CSharp", "BOGGYDANCE",
                "PROGRAMMATION",
                "CODER", "CODER", "CODER",
                "TEST", "CAILLOUX",
                "AZ", "AZ", "CHEVALMORT",
                "BOGGLE", "JEU",
                "CSharp", "ROCHER",
                "PROGRAMMATION",
                "GARE", "CODER", "CODER",
                                "SALUTCESTMOI", "TEST",
                "QUOI", "QOU", "BEH",
                "BOGGLE", "JEU",
                "CSharp", "CSharp",
                "PROGRAMMATION",
                "ZIZIDECHAMAUX", "CHEVAL33", "ROCHERROCHER",
                "DOTNET"
            };

            NuageDeMots nuage = new NuageDeMots(mots, 800, 600);
            string nomFichier = "test_nuage_mots.png";
            nuage.GenererNuage(nomFichier);

            // On vérifie que le fichier a été créé.
            if (File.Exists(nomFichier))
            {
                Console.WriteLine($"Test OK : Le nuage de mots a été généré dans {nomFichier}.");
            }
            else
            {
                Console.WriteLine("Test ÉCHEC : Le fichier du nuage de mots n'a pas été créé.");
            }
        }

        /// <summary>
        /// Méthode pour lancer tous les tests manuellement.
        /// </summary>
        public void LancerTousLesTests()
        {
            TestJoueurAjoutMot();
            TestDictionnaireRechDicho();
            TestDeLance();
            TestPlateauMotExistePas();
            TestScoreMot();
            TestNuageDeMots();
        }
    }
}
