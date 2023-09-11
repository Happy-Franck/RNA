using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Franck38rna
{
    public partial class Form1 : Form
    {
        public int NBUE { get; set; }
        public int NBUC { get; set; }
        public static double[] NMSE = new Double[50];
        public classes.SerieDeHenon srh;
        public void Generation()
        {
            srh = new classes.SerieDeHenon(500, 1.4, 0.3);
            double[] dX = srh.serieX;
            double[] dY = srh.serieY;
            NBUE = srh.CalculNUE();
            NBUC = srh.CalculNUC();
            if (chart1.Series.Count != 1)
            {
                while (chart1.Series.Count != 0)
                {
                    chart1.Series.RemoveAt(0);
                }
                chart1.Series.Add("Yn en fonction de Xn");
            }

            chart1.Series[0].Color = Color.Blue;
            chart1.Series[0].ChartType = SeriesChartType.Point;


            for (int i = 1; i < srh.n; i++)
            {
                listBox1.Items.Add("x" + i + "= " + Math.Round(dX[i], 8));
                listBox2.Items.Add("y" + i + "= " + Math.Round(dY[i], 8));

                chart1.Series[0].Points.AddXY(Math.Round(dX[i], 2), Math.Round(dY[i], 2));
            }

            label9.Text = "" + NBUE + "";
            label10.Text = "" + NBUC + "";
            label11.Text = "" + 1 + "";

        }
        private void Button2_Click(object sender, EventArgs e)
        {
            Generer.Enabled = false;

            double[,,] W = new double[50, 50, 50];
            double nmse;
            String[] wentree = new String[100];
            String[] wsortie = new String[100];
            int k = 0;
            listBox3.Items.Clear();
            listBox4.Items.Clear();

            while (chart1.Series.Count != 0)
            {
                chart1.Series.RemoveAt(0);
            }
            chart1.Series.Add("NMSE");
            int iN = chart1.Series.IndexOf("NMSE");
            chart1.Series[0].Color = Color.Red;
            chart1.Series[0].ChartType = SeriesChartType.Line;


            for (int i = 1; i < 50; i++)
            {
                classes.Apprentissage dg = new classes.Apprentissage(NBUE, i, srh.serieX);
                nmse = classes.Apprentissage.NMSE();
                NMSE[i] = nmse;
                chart1.Series[0].Points.AddXY(Math.Round(NMSE[i]), i);
            }
            classes.Apprentissage ddg = new classes.Apprentissage(NBUE, NBUC, srh.serieX);
            W = ddg.ApprentissageR();


            for (int i = 1; i <= classes.Apprentissage.NombreUniteCachee; i++)
            {
                for (int j = 1; j <= classes.Apprentissage.NombreUniteEntree; j++)
                {
                    wentree[k] = "W(2," + i + "," + j + ")=" + W[2, i, j];
                    listBox3.Items.Add(wentree[k]);
                    k++;


                }
            }
            k = 0;
            for (int i = 1; i <= 1; i++)
            {
                for (int j = 1; j <= classes.Apprentissage.NombreUniteCachee; j++)
                {
                    wsortie[k] = "W(3," + i + "," + j + ")=" + W[3, i, j];
                    listBox4.Items.Add(wsortie[k]);
                    k++;

                }
            }

        }
        private void Button3_Click_1(object sender, EventArgs e)
        {
            int nb = int.Parse(comboBox1.Text);
            double[] res = new double[100];
            double[,,] W = new double[50, 50, 50];
            double[] s = srh.serieX;
            classes.Apprentissage ddg = new classes.Apprentissage(NBUE, NBUC, s);
            W = ddg.ApprentissageR();
            if (nb == 1)
            {
                res = ddg.PredictionAUnpas(W);
            }
            else
            {
                res = ddg.PredictionAPlusieursPas(nb, W);
            }

            List<classes.Resultat> list = new List<classes.Resultat>();
            for (int i = 0; i < res.Count(x => x != 0); i++)
            {
                classes.Resultat resultat = new classes.Resultat(i, classes.Apprentissage.ValeurAttendue[i], Math.Round(res[i], 8));
                list.Add(resultat);
            }
            dataGridView1.DataSource = list;

            DrawChartLs2(res, classes.Apprentissage.ValeurAttendue, list.Count);
        }
        private void DrawChartLs2(double[] Xpredite, double[] Xattendue, int n)
        {
            while (chart1.Series.Count != 0)
            {
                chart1.Series.RemoveAt(0);
            }

            chart1.Series.Add("Prédiction");
            chart1.Series.Add("Attendue");
            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[1].ChartType = SeriesChartType.Line;
            chart1.Series[0].Color = Color.Green;
            chart1.Series[1].Color = Color.Blue;
            int iP = chart1.Series.IndexOf("Prédiction");
            int iA = chart1.Series.IndexOf("Attendue");
            for (int i = 0; i < n; i++)
            {
                chart1.Series[iP].Points.AddXY(i + NBUE, Xpredite[i]);
            }

            for (int i = 0; i < n + NBUE; i++)
            {
                if (i < NBUE)
                {
                    chart1.Series[iA].Points.AddXY(i, srh.serieX[i]);
                }
                else
                {
                    chart1.Series[iA].Points.AddXY(i, Xattendue[i - NBUE]);
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Generer_Click(object sender, EventArgs e)
        {
            Generation();
            btnApprentissage.Enabled = true;
            comboBox1.Enabled = true;
        }

        private void btnApprentissage_Click(object sender, EventArgs e)
        {
            Generer.Enabled = false;

            double[,,] W = new double[50, 50, 50];
            double nmse;
            String[] wentree = new String[100];
            String[] wsortie = new String[100];
            int k = 0;
            listBox3.Items.Clear();
            listBox4.Items.Clear();

            while (chart1.Series.Count != 0)
            {
                chart1.Series.RemoveAt(0);
            }
            chart1.Series.Add("NMSE");
            int iN = chart1.Series.IndexOf("NMSE");
            chart1.Series[0].Color = Color.Red;
            chart1.Series[0].ChartType = SeriesChartType.Line;


            for (int i = 1; i < 50; i++)
            {
                classes.Apprentissage dg = new classes.Apprentissage(NBUE, i, srh.serieX);
                nmse = classes.Apprentissage.NMSE();
                NMSE[i] = nmse;
                chart1.Series[0].Points.AddXY(Math.Round(NMSE[i]), i);
            }
            classes.Apprentissage ddg = new classes.Apprentissage(NBUE, NBUC, srh.serieX);
            W = ddg.ApprentissageR();


            for (int i = 1; i <= classes.Apprentissage.NombreUniteCachee; i++)
            {
                for (int j = 1; j <= classes.Apprentissage.NombreUniteEntree; j++)
                {
                    wentree[k] = "W(2," + i + "," + j + ")=" + W[2, i, j];
                    listBox3.Items.Add(wentree[k]);
                    k++;


                }
            }
            k = 0;
            for (int i = 1; i <= 1; i++)
            {
                for (int j = 1; j <= classes.Apprentissage.NombreUniteCachee; j++)
                {
                    wsortie[k] = "W(3," + i + "," + j + ")=" + W[3, i, j];
                    listBox4.Items.Add(wsortie[k]);
                    k++;

                }
            }
        }

        private void btnPredict_Click(object sender, EventArgs e)
        {
            int nb = int.Parse(comboBox1.Text);
            double[] res = new double[100];
            double[,,] W = new double[50, 50, 50];
            double[] s = srh.serieX;
            classes.Apprentissage ddg = new classes.Apprentissage(NBUE, NBUC, s);
            W = ddg.ApprentissageR();
            if (nb == 1)
            {
                res = ddg.PredictionAUnpas(W);
            }
            else
            {
                res = ddg.PredictionAPlusieursPas(nb, W);
            }

            List<classes.Resultat> list = new List<classes.Resultat>();
            for (int i = 0; i < res.Count(x => x != 0); i++)
            {
                classes.Resultat resultat = new classes.Resultat(i, classes.Apprentissage.ValeurAttendue[i], Math.Round(res[i], 8));
                list.Add(resultat);
            }
            dataGridView1.DataSource = list;

            DrawChartLs2(res, classes.Apprentissage.ValeurAttendue, list.Count);
        }
    }
}
