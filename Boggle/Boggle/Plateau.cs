using System;
using System.Text;

namespace BoggleGame
{
    /// <summary>
    /// Représente le plateau de Boggle et teste si un mot y est présent.
    /// </summary>
    public class Plateau
    {
        private De[,] des;
        private int taille;
        private readonly int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
        private readonly int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };
        private Dictionnaire dictionnaire;

        public Plateau(int taille)
        {
            this.taille = taille;
            this.des = new De[taille, taille];
        }

        public void SetDictionnaire(Dictionnaire d)
        {
            this.dictionnaire = d;
        }

        public void InitialiserPlateau(System.Collections.Generic.List<De> listeDes, Random r)
        {
            int index = 0;
            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    des[i, j] = listeDes[index++];
                    des[i, j].Lance(r);
                }
            }
        }



        public bool Test_Plateau(string mot)
        {
            if (string.IsNullOrEmpty(mot) || mot.Length < 2)
                return false;

            if (!dictionnaire.MotExiste(mot))
                return false;

            mot = mot.ToUpper();

            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    if (des[i, j].FaceVisible == mot[0])
                    {
                        bool[,] visite = new bool[taille, taille];
                        if (ChercherMot(mot, 0, i, j, visite))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool ChercherMot(string mot, int index, int x, int y, bool[,] visite)
        {
            if (index == mot.Length) return true;
            if (x < 0 || x >= taille || y < 0 || y >= taille || visite[x, y]) return false;
            if (des[x, y].FaceVisible != mot[index]) return false;

            visite[x, y] = true;
            for (int dir = 0; dir < 8; dir++)
            {
                int newX = x + dx[dir];
                int newY = y + dy[dir];
                if (ChercherMot(mot, index + 1, newX, newY, visite))
                    return true;
            }

            visite[x, y] = false;
            return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    sb.Append(des[i, j].FaceVisible);
                    sb.Append(" ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public string toString()
        {
            return this.ToString();
        }

        public De[,] Des { get => des; }
    }
}
