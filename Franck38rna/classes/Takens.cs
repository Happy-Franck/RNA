using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franck38rna.classes
{
    public class Takens
    {

        private static int Dimension = 500;
        private double[] serieTemporelle;
        public Takens(double[] serie)
        {
            serieTemporelle = serie;
        }

        private double[,] MatriceDeCovariance
        {
            get
            {
                double[] value = new double[500];
                value = serieTemporelle;
                return Matrice.ProduitVecteurColLig(value, value, Dimension);
            }

        }
        private double[] TrieDecroissant(double[] tab)
        {
            double tmp;
            for (int i = 0; i < tab.Length; i++)
            {
                for (int j = 0; j < tab.Length; j++)
                {
                    if (tab[i] < tab[j])
                    {
                        tmp = tab[i];
                        tab[i] = tab[j];
                        tab[j] = tmp;
                    }
                }
            }
            return tab;
        }

        public double[] ValeursPropre()
        {
            double[,] temp = new double[500, 500];
            temp = MatriceDeCovariance;
            double[] valprop = new double[500];
            double[,] matrix = new double[500, 500];
            Jacobi jc = new Jacobi();
            matrix = jc.diagonalization(temp);
            for (int i = 0; i < 500; i++)
            {
                for (int j = 0; j < 500; j++)
                {
                    if (i == j)
                    {
                        valprop[i] = matrix[i, i];
                    }
                }
            }
            return TrieDecroissant(valprop);
        }

        public double[] ErreurApproximationMoyenne
        {
            get
            {
                double[] res = new double[Dimension];
                double[] vp = new double[Dimension];
                vp = ValeursPropre();
                for (int i = 0; i < vp.Length - 1; i++)
                {

                    res[i] = Math.Sqrt(vp[i + 1]);
                    Console.WriteLine(res[i]);
                }
                return res;
            }
        }
    }
}
