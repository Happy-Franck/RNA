using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Franck38rna.classes
{
    public class Apprentissage
    {
        public static double[] serieTemporelle = new double[500];
        //public static double[] serieTemporelle = new double[] { 0.2, 0.4, 0.1, 0.1 };
        public static double[,] V = new double[50, 50];
        public static double[,,] W = new double[50, 50, 50];
        private static double[,] H = new double[100, 100];
        private static double[,] Delta = new double[100, 100];
        private static double[,,] DeltaW = new double[50, 50, 50];
        public static double[] ValeurAttendue;
        public static double[] xChapeau = new double[100];
        private static int decal = 0;
        private static int NbPrototype = 10;
        public static int NombreUniteEntree { get; set; }
        public static int NombreUniteCachee { get; set; }

        public Apprentissage(int nbCE, int nbCC)
        {
            NombreUniteEntree = nbCE;
            NombreUniteCachee = nbCC;
            InitialisationPoids();
        }
        public Apprentissage(int nbCE, int nbCC, double[] serie)
        {
            NombreUniteEntree = nbCE;
            NombreUniteCachee = nbCC;
            serieTemporelle = serie;
            InitialisationPoids();
        }
        public Apprentissage(int nbCE, int nbCC, double[,,] poids)
        {
            NombreUniteEntree = nbCE;
            NombreUniteCachee = nbCC;
            W = poids;
        }


        //fonction de transfert sigmoide
        private static double g(double x)
        {
            return (1 / (1 + Math.Exp(-x)));
        }
        private static double gprim(double x)
        {
            return (Math.Exp(-x) / Math.Pow((1 + Math.Exp(-x)), 2));
        }
        private static double identity(double x)
        {
            return x;
        }
        public static double[] Xchapeau()
        {
            double[] res = new double[50];
            double[] prototype = new double[50];
            for (int i = 0; i < NbPrototype; i++)//NB prototype
            {
                for (int j = 0; j < NombreUniteEntree; j++)
                {
                    prototype[j] = serieTemporelle[i + j];
                }
                res[i] = PropagationEnAvant(prototype, W);
            }
            return res;
        }
        private double[,,] InitW()
        {
            double[,,] poids = new double[50, 50, 50];
            for (int i = 1; i <= NombreUniteCachee; i++)
            {
                for (int j = 1; j <= NombreUniteEntree; j++)
                {
                    poids[2, i, j] = 0.2;
                }
            }
            for (int j = 1; j <= NombreUniteCachee; j++)
            {
                poids[3, 1, j] = 0.1;
            }
            return poids;
        }
        private static void InitialisationPoids()
        {
            //Initialisation des poids
            for (int i = 1; i <= NombreUniteCachee; i++)
            {
                for (int j = 1; j <= NombreUniteEntree; j++)
                {
                    W[2, i, j] = 0.1;
                }
            }
            for (int j = 1; j <= NombreUniteCachee; j++)
            {
                W[3, 1, j] = 0.1;
            }
        }
        public static double PropagationEnAvant(double[] prototype, double[,,] poids)
        {

            //Initialisation de V[1,i]
            for (int i = 1; i <= NombreUniteEntree; i++)
            {
                V[1, i] = prototype[i - 1];
            }
            //Calcul de H[2,i]
            double somme = 0;
            for (int i = 1; i <= NombreUniteCachee; i++)
            {
                for (int j = 1; j <= NombreUniteEntree; j++)
                {
                    somme += poids[2, i, j] * V[1, j];
                }
                H[2, i] = somme;
                somme = 0;
            }
            //propagation du signal vers l'avant entre CE et CC
            for (int i = 1; i <= NombreUniteCachee; i++)
            {
                V[2, i] = g(H[2, i]);
            }
            //Calcul de H[3,1]
            for (int i = 1; i <= 1; i++)/// Nb unite de sortie
            {
                for (int j = 1; j <= NombreUniteCachee; j++)
                {
                    somme += poids[3, i, j] * V[2, j];
                }
                H[3, i] = somme;
                somme = 0;
            }
            //propagation du signal vers l'avant entre CC et CS
            V[3, 1] = identity(H[3, 1]);
            double res = V[3, 1];
            return res;
        }
        public static double NMSE()
        {
            double res, somme = 0;
            double[] xCh = new double[500];
            xCh = Xchapeau();
            
            for (int i = 0; i < NbPrototype; i++)
            {
                somme += (serieTemporelle[i + NombreUniteEntree] - xCh[i]) * (serieTemporelle[i + NombreUniteEntree] - xCh[i]);
            }
            res = somme / (NbPrototype * Variance(serieTemporelle) * Variance(serieTemporelle));
            Console.WriteLine(res);
            return res;
        }
        private static double Variance(double[] serie)
        {
            double variance, moyenne, somme1 = 0, somme2 = 0;
            int n = NbPrototype;
            for (int i = 0; i < n; i++)
            {
                somme1 += serie[i];
            }
            moyenne = somme1 / n;
            for (int i = 0; i < n; i++)
            {
                somme2 += Math.Pow((serie[i] - moyenne), 2);
            }
            variance = somme2 / n;
            return Math.Round(variance, 8);
        }

        public double[] PredictionAUnpas(double[,,] poids)
        {
            double[] res = new double[10];
            double[] prototype = new double[NombreUniteEntree];
            ValeurAttendue = new double[50];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < NombreUniteEntree; j++)
                {
                    prototype[j] = serieTemporelle[decal + j + i];
                }
                ValeurAttendue[i] = serieTemporelle[decal + i + NombreUniteEntree];
                res[i] = PropagationEnAvant(prototype, poids);
            }
            return res;
        }
        public double[] PredictionAPlusieursPas(int nb, double[,,] poids)
        {
            double[] res = new double[100];
            double s;
            ValeurAttendue = new double[50];
            double[] prototype = new double[100];
            for (int i = 0; i < NombreUniteEntree; i++)
            {
                prototype[i] = serieTemporelle[decal + i];
            }
            for (int i = 0; i < nb; i++)
            {
                s = PropagationEnAvant(prototype, poids);
                for (int j = NombreUniteEntree - 1; j >= 1; j--)
                {
                    double temp;
                    temp = prototype[j - 1];
                    prototype[j] = temp;
                }
                prototype[0] = s;
                ValeurAttendue[i] = serieTemporelle[decal + i + NombreUniteEntree];
                res[i] = s;
            }
            return res;
        }
        public double[,,] ApprentissageR()
        {
            double[] prototype = new double[10];
            double pas = 35;
            double[,,] w;
            w = InitW();
            // double min = double.MaxValue;
            for (int i = 0; i < NbPrototype; i++)
            {
                for (int j = 0; j < NombreUniteEntree; j++)
                {
                    prototype[j] = Math.Round(serieTemporelle[i + j], 2);
                }
                double result = PropagationEnAvant(prototype, w);

                //retro 
                Delta[3, 1] = gprim(H[3, 1]) * (serieTemporelle[i + NombreUniteEntree] - V[3, 1]);
                // entre cc et cs

                for (int l = 1; l <= NombreUniteCachee; l++)
                {
                    Delta[2, l] = gprim(H[2, l]) * w[3, 1, l] * Delta[3, 1];

                }

                //Apprentissage poids caché
                for (int k = 1; k <= NombreUniteCachee; k++)
                {
                    for (int l = 1; l <= NombreUniteEntree; l++)
                    {
                        DeltaW[2, k, l] = pas * Delta[2, k] * V[1, l];
                    }
                }
                for (int l = 1; l <= NombreUniteCachee; l++)
                {
                    DeltaW[3, 1, l] = pas * Delta[3, 1] * V[2, l];
                }
                //Apprentissage poids d'entree
                for (int k = 1; k <= NombreUniteCachee; k++)
                {
                    for (int l = 1; l <= NombreUniteEntree; l++)
                    {
                        w[2, k, l] += Math.Round(DeltaW[2, k, l], 8);

                    }

                }
                for (int k = 1; k <= NombreUniteCachee; k++)
                {
                    w[3, 1, k] += Math.Round(DeltaW[2, 1, k], 8);

                }

            }
            return w;
        }
    }
}
