using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franck38rna.classes
{
    public class Position
    {
        public int r { get; set; }
        public int s { get; set; }
        public Position(int r, int s)
        {
            this.r = r;
            this.s = s;
        }
    }
    public class Jacobi
    {

        public static Position PositionMax(double[,] A)
        {
            double max = 0;
            Position pos = new Position(0, 0);
            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A.Length; j++)
                {
                    if (max < A[i, j])
                    {
                        max = A[i, j];
                        pos.r = i;
                        pos.s = j;
                    }
                }
            }
            Console.WriteLine("r: " + pos.r + " --- s:" + pos.s);
            return pos;
        }

        private double cos(double a, double b)
        {
            return (Math.Sqrt(0.5 * (1 + (b / Math.Sqrt(a * a + b * b)))));
        }
        private double sin(double a, double b)
        {
            return (a / (2 * cos(a, b) * Math.Sqrt(a * a + b * b)));
        }
        public double[,] diagonalization(double[,] m)
        {
            double c, d;
            Position pos = new Position(0, 0);
            int lmax = pos.r;
            int cmax = pos.s;
            double[,] P = new double[500, 500];
            double a = 0, b = 0;
            a = 2 * m[lmax, cmax];
            b = m[lmax, lmax] - m[cmax, cmax];
            if (m[lmax, lmax] != m[cmax, cmax])
            {
                d = sin(a, b);
                c = cos(a, b);
            }
            else
            {
                c = Math.Sqrt(2) / 2;
                d = Math.Sqrt(2) / 2;
            }
            for (int i = 0; i < 500; i++)
            {
                for (int j = 0; j < 500; j++)
                {
                    if ((i == j) && (j != lmax) && (j != cmax))
                        P[i, j] = 1;
                    else if ((i == cmax) && (j == lmax))
                        P[i, j] = -d;
                    else if ((i == lmax) && (j == lmax) || (i == cmax) && (j == cmax))
                        P[i, j] = c;
                    else if ((i == lmax) && (j == cmax))
                        P[i, j] = d;
                    else P[i, j] = 0;
                }
            }
            double[,] trans_P = new double[500, 500];
            trans_P = Matrice.transpose(P);
            double[,] M = new double[500, 500];
            M = Matrice.ProduitMatrice(Matrice.ProduitMatrice(trans_P, m), P);
            return M;
        }

    }
}
