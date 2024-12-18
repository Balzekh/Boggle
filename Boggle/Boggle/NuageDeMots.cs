using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Collections.Generic;

namespace BoggleGame
{
    public class NuageDeMots
    {
        private readonly List<string> mots;
        private readonly Dictionary<string, int> frequenceMots;
        private readonly Random random;
        private readonly int largeur;
        private readonly int hauteur;
        private readonly Color[] couleurs = {
            Color.FromArgb(41, 128, 185),
            Color.FromArgb(39, 174, 96),
            Color.FromArgb(142, 68, 173),
            Color.FromArgb(211, 84, 0),
            Color.FromArgb(192, 57, 43)
        };

        public NuageDeMots(List<string> mots, int largeur = 800, int hauteur = 600)
        {
            this.mots = mots ?? new List<string>();
            this.largeur = largeur;
            this.hauteur = hauteur;
            this.random = new Random();
            this.frequenceMots = CalculerFrequence(this.mots);
        }

        private Dictionary<string, int> CalculerFrequence(List<string> mots)
        {
            return mots.GroupBy(m => m)
                      .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// Génère un nuage de mots plus lisible en plaçant les mots sur une spirale
        /// en évitant les chevauchements.
        /// </summary>
        public void GenererNuage(string nomFichier)
        {
            if (string.IsNullOrEmpty(nomFichier))
                throw new ArgumentException("Le nom du fichier ne peut pas être null ou vide");

            using var bitmap = new Bitmap(largeur, hauteur);
            using var graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.White);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Tri des mots par fréquence décroissante
            var motsTries = frequenceMots.OrderByDescending(kv => kv.Value).ToList();

            int centerX = largeur / 2;
            int centerY = hauteur / 2;

            // Liste des rectangles déjà occupés pour éviter les chevauchements
            List<RectangleF> emplacements = new List<RectangleF>();

            foreach (var kvp in motsTries)
            {
                if (string.IsNullOrEmpty(kvp.Key)) continue;

                // Taille de la police en fonction de la fréquence
                float fontSize = Math.Max(12, Math.Min(48, 12 + kvp.Value * 4));
                using var font = new Font("Arial", fontSize, FontStyle.Bold);
                Color couleur = couleurs[random.Next(couleurs.Length)];
                using var brush = new SolidBrush(couleur);

                // Mesure du mot
                var size = graphics.MeasureString(kvp.Key, font);

                // Paramètres de la spirale
                double angle = 0;
                double radius = 10;
                bool placeTrouve = false;
                RectangleF rectMot = RectangleF.Empty;

                // On tente de placer le mot en augmentant progressivement angle et rayon
                // jusqu’à trouver un emplacement sans chevauchement.
                while (!placeTrouve && radius < Math.Max(largeur, hauteur))
                {
                    double x = centerX + radius * Math.Cos(angle);
                    double y = centerY + radius * Math.Sin(angle);

                    rectMot = new RectangleF((float)(x - size.Width / 2), (float)(y - size.Height / 2), size.Width, size.Height);

                    if (!Chevauche(rectMot, emplacements))
                    {
                        placeTrouve = true;
                    }
                    else
                    {
                        // On avance sur la spirale
                        angle += 0.3;  // incrément d'angle
                        radius += 5;   // incrément de rayon pour s'éloigner du centre
                    }
                }

                // On dessine le mot à l’emplacement trouvé
                if (placeTrouve)
                {
                    graphics.DrawString(kvp.Key, font, brush, rectMot.X, rectMot.Y);
                    emplacements.Add(rectMot);
                }
                else
                {
                }
            }

            bitmap.Save(nomFichier, ImageFormat.Png);
        }

        /// <summary>
        /// Vérifie si rect chevauche un des rectangles dans la liste
        /// </summary>
        private bool Chevauche(RectangleF rect, List<RectangleF> emplacements)
        {
            foreach (var r in emplacements)
            {
                if (r.IntersectsWith(rect))
                    return true;
            }
            return false;
        }
    }
}
