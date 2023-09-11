using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franck38rna.classes
{
    public class Matrice
    {
        public static double[,] ProduitMatrice(double[,] M1, double[,] M2)
        {
            double[,] PM = new double[500, 500];
            for (int i = 0; i < 500; i++)
            {
                for (int j = 0; j < 500; j++)
                {
                    PM[i, j] = 0;
                    for (int k = 0; k < 500; k++)
                    {
                        PM[i, j] = PM[i, j] + M1[i, k] * M2[k, j];
                    }
                }
            }

            return PM;
        }
        public static double[,] transpose(double[,] a)
        {
            double[,] resultat = new double[500, 500];
            for (int i = 0; i < 500; i++)
            {
                for (int j = 0; j < 500; j++)
                {
                    resultat[j, i] = a[i, j];
                }
            }
            return resultat;
        }
        public static double[,] ProduitVecteurColLig(double[] vectCol, double[] vectLig, int dimension)
        {
            double[,] C = new double[dimension, dimension];
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    C[i, j] = (vectCol[i] * vectLig[j]);
                }
            }
            return C;
        }

    }
}
