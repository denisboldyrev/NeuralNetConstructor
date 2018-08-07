using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Neuron;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Layer layer1 = new Layer(2, 2);
            Layer layer2 = new Layer(2, 2);
            Layer layer3 = new Layer(2, 1);

            double[] x = { 0.1, 0.9 };
            double[,] w1 = { { -2, -2 }, { 3, 3 } };
            double[] b1 = { 2, 2 };
            double[,] w2 = { { -2, -4 }, { 2, 2 } };
            double[] b2 = { 3, -2 };
            double[,] w3 = { { 3, 1 }};
            double[] b3 = { -2 };

            layer1.W = w1;
            layer1.Bias = b1;
            layer2.W = w2;
            layer2.Bias = b2;
            layer3.W = w3;
            layer3.Bias = b3;

            layer1.X = x;
            layer1.GetLayerOut(1);
            layer2.X = layer1.Output;
            layer2.GetLayerOut(1);
            layer3.X = layer2.Output;
            layer3.GetLayerOut(1);
            double[] d = { 0.9 };

            layer3.CalcWeightsOutLayer(0.8, d, 1);
            layer2.CalcWeightsHiddenLayer(0.8, layer3.LocalGrad, 1);
            layer1.CalcWeightsHiddenLayer(0.8, layer2.LocalGrad, 1);

        }

        private void button2_Click(object sender, EventArgs e)
        {

            //INeuron[] inLayer = new INeuron[5];

            //for (int i = 0; i < inLayer.Length; i++)
            //{
            //    inLayer[i] = new Perceptron(2, Perceptron.ActivationFunctionType.Sigmoid);
            //}

            //INeuron[] inL = { new Perceptron(2, Perceptron.ActivationFunctionType.Sigmoid), new Perceptron(2, Perceptron.ActivationFunctionType.Sigmoid) };
            //INeuron[] hidL = { new Perceptron(2, Perceptron.ActivationFunctionType.Sigmoid), new Perceptron(2, Perceptron.ActivationFunctionType.Sigmoid) };
            //INeuron[] oL = { new Perceptron(2, Perceptron.ActivationFunctionType.Sigmoid) };

            double[] input = { 0.1, 0.9 };
            double[] etalon = { 0.9 };

         

            //hidL[0].W = new double[2];
            //hidL[0].W[0] = -2; hidL[0].W[1] = -4;
            //hidL[0].Bias = 3;
            //hidL[1].W = new double[2];
            //hidL[1].W[0] = 2; hidL[1].W[1] = 2;
            //hidL[1].Bias = -2;

            //oL[0].W = new double[2];
            //oL[0].W[0] = 3; oL[0].W[1] = 1;
            //oL[0].Bias = -2;
            InputLayer inputLayer = new InputLayer(2, 2, InputLayer.ActivationFunctionType.Sigmoid);
            inputLayer.X = input;
            inputLayer.W[0,0] = -2; inputLayer.W[0,1] = -2;
            inputLayer.Bias[0] = 2;
            inputLayer.W[1,0] = 3; inputLayer.W[1, 1] = 3;
            inputLayer.Bias[1] = 2;
            inputLayer.LayerOutput();
            double[] output = inputLayer.Output;
            HiddenLayer hiddenLayer = new HiddenLayer(2, 2, HiddenLayer.ActivationFunctionType.Sigmoid);
            hiddenLayer.X = inputLayer.Output;
            hiddenLayer.W[0, 0] = -2; hiddenLayer.W[0, 1] = -4;
            hiddenLayer.Bias[0] = 3;
            hiddenLayer.W[1, 0] = 2; hiddenLayer.W[1, 1] = 2;
            hiddenLayer.Bias[1] = -2;
            output = hiddenLayer.LayerOutput();

            OutputLayer outputLayer = new OutputLayer(2, 1, OutputLayer.ActivationFunctionType.Sigmoid);
            outputLayer.X = output;

            outputLayer.W[0, 0] = 3; outputLayer.W[0, 1] = 1;
            outputLayer.Bias[0] = -2;

            output = outputLayer.LayerOutput();

            outputLayer.CalcWeights(0.8, etalon);
            hiddenLayer.CalcWeights(0.8, outputLayer.LocalGrad);
            inputLayer.CalcWeights(0.8, hiddenLayer.LocalGrad);
        }
    }
}
