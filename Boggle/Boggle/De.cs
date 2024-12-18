using System;

namespace BoggleGame
{
    /// <summary>
    /// Représente un dé avec 6 faces (6 lettres).
    /// </summary>
    public class De
    {
        private char[] faces;
        private char faceVisible;

        /// <summary>
        /// Constructeur d'un dé avec 6 lettres.
        /// </summary>
        /// <param name="faces">Tableau de 6 lettres</param>
        public De(char[] faces)
        {
            if (faces.Length != 6)
                throw new ArgumentException("Un dé doit avoir exactement 6 faces");

            this.faces = faces;
            this.faceVisible = faces[0];
        }

        /// <summary>
        /// Lance le dé et choisit une face aléatoirement.
        /// </summary>
        public void Lance(Random r)
        {
            faceVisible = faces[r.Next(6)];
        }

        public override string ToString()
        {
            return faceVisible.ToString();
        }

        public string toString()
        {
            return this.ToString();
        }

        public char FaceVisible { get => faceVisible; }
        public char[] Faces { get => faces; }
    }
}
