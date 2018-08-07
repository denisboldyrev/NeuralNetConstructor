using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Neuron;

namespace NeuralNetCounstructor
{

    public partial class Form2 : Form
    {
        //public int numLayers; // число слоев для многослойного персептрона
        //public int [] numNeurons;
        //public int numIn;

        public Layer[] layers;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // numLayers = Convert.ToInt32(textBox1.Text);
            NeuralNetSettings.NumLayers = Convert.ToInt32(textBox1.Text);
            for (int i = 0; i < NeuralNetSettings.NumLayers; i++)
            {
                comboBox1.Items.Insert(i, "Слой " + (i + 1));
            }
            NeuralNetSettings.NumNeurons = new int[NeuralNetSettings.NumLayers];
            NeuralNetSettings.NumIn = Convert.ToInt32(textBox3.Text);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            NeuralNetSettings.NumNeurons[comboBox1.SelectedIndex] = Convert.ToInt32(textBox2.Text);

            // this.numNeurons[comboBox1.SelectedIndex] = Convert.ToInt32(textBox2.Text);


            //            numLayers = new int[Convert.ToInt32(textBox1.Text)];
            layers = new Layer[NeuralNetSettings.NumLayers]; // массив в котором хранятся объекты слоя сети. Число объектов равно числу слоев numLayers
                                                             //double[] x = { 0.1, 0.9 }; // Входы сети. Вектор подается на первый(входной) слой layers[0] для того, чтобы вычислить выход первого слоя.
                                                             //                           // Выход первого слоя необходим для того, чтобы подать его на вход второго слоя и т.д.
                                                             //int[] numNeurons = { 2, 2, 1 }; // массив, который хранить число нейронов для каждого слоя

            //layers[0] = new Layer(2, 2); // создаем первый входной слой. Подаем ему на вход вектор входов и число нейронов
            //layers[0].X = x;
            //layers[0].W[0, 0] = -2;
            //layers[0].W[0, 1] = -2;

            //layers[0].W[1, 0] = 3;
            //layers[0].W[1, 1] = 3;

            //layers[0].Bias[0] = 2;
            //layers[0].Bias[1] = 2;
            //double[] l1 = layers[0].GetLayerOut();

            //layers[1] = new Layer(l1.Length, 2);
            //layers[1].X = l1;
            //layers[1].W[0, 0] = -2;
            //layers[1].W[0, 1] = -4;
            //layers[1].W[1, 0] = 2;
            //layers[1].W[1, 1] = 2;

            //layers[1].Bias[0] = 3;
            //layers[1].Bias[1] = -2;
            //layers[1].GetLayerOut();
            //double[] l2 = layers[1].GetLayerOut();

            //layers[2] = new Layer(l2.Length, 1);
            //layers[2].X = l2;
            //layers[2].W[0, 0] = 3;
            //layers[2].W[0, 1] = 1;
            //layers[2].Bias[0] = -2;

            //layers[2].GetLayerOut();
            //double[] y = new double[1];
            //y[0] = 0.9;
            //double[] errr = layers[2].GetOutLayerError(y);
            //double[] errr2 = layers[2].GetHiddenLayerError(errr);
            //double[] errr3 = layers[1].GetHiddenLayerError(errr2);

            //layers[2].CalcWeightsOutLayer(0.8);
            ////            layers[2].CalcWeightsHiddenLayer(0.8, layers[2].OutHError);
            //layers[1].CalcWeightsHiddenLayer(0.8, layers[2].OutHError);
            //layers[0].CalcWeightsHiddenLayer(0.8, layers[1].OutHError);



            // **************************************************************************************


            // Далее создаем остальные скрытые слои вплоть до выходного слоя.
            // На вход каждого из них подается вектор выходов предудущего слоя, а также число нейронов, которое выбирается из массива numNeurons
            //using (XmlWriter xml = XmlWriter.Create("w.xml"))
            //{
            //    xml.WriteStartDocument();
            //    xml.WriteStartElement("w");
            //    for (int i = 0; i < numLayers; i++)
            //    {


            //        for (int j = 0; j < layers[i].NumNeurons; j++)
            //        {
            //            for (int q = 0; q < layers[i].X.Length; q++)
            //            {
            //                xml.WriteElementString("w" + (i+1) + (q+1), layers[i].W[j, q].ToString());
            //            }

            //        }


            //    }
            //    xml.WriteEndElement();

            //    xml.WriteEndDocument();
            //}

            //           Close();

            //for (int i = 0; i < numLayers - 1; i++)
            //{
            //    layers[i + 1] = new Layer(layers[i].GetLayerOut(), numNeurons[i + 1]);
            //}
            //double[] outNet = layers[numLayers - 1].GetLayerOut(); // получить выход сети
            //// выход сети будет необходим для вычисления ошибки - сначала выходного слоя, а позже ошибки каждого нейрона каждого слоя.
            //double[] d = new double[outNet.Length]; // эталон

            //double[] outError = new double[outNet.Length];
            //for (int i = 0; i < outNet.Length; i++)
            //{
            //    outError[i] = (d[i] - outNet[i]) * outNet[i] * (1 - outNet[0]);
            //}
            //d[0] = 0.1;

            //double[] outErr = layers[numLayers - 1].GetOutLayerError(d);
            //layers[numLayers - 2].GetHiddenLayerError(outErr);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "Линейный нейрон" || comboBox2.SelectedItem.ToString() == "Персептрон")
            {
                label1.Enabled = false;
                textBox1.Enabled = false;
                button1.Enabled = false;
            }
            else
            {
                label1.Enabled = true;
                textBox1.Enabled = true;
                button1.Enabled = true;
            }
        }
    }

}
