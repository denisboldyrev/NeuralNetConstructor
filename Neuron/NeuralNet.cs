using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron
{
    class NeuralNet
    {
        public NeuralNet(params ILayer[] layers)
        {
            ILayer[] l = layers;
        }
        public double[] GetOutput(double[] input)
        {
            double[] Output = { };
            return Output;
        }
        public void Learn()
        {

            // ******************** for backprop learn
            // if input layer
            
                //l[0].LocalGrad = new double[X.Length];
                //for (int i = 0; i < NumNeurons; i++)
                //{
                //    switch (actFTyp)
                //    {
                //        case ActivationFunctionType.Sigmoid:
                //            Error[i] = Output[i] * (1 - Output[i]) * localGrad[i];
                //            break;
                //        case ActivationFunctionType.HyperbolicTangens:
                //            Error[i] = (1 - Output[i]) * (1 + Output[i]) * localGrad[i];
                //            break;
                //        case ActivationFunctionType.BipolarSigmoid:
                //            Error[i] = 0;
                //            break;
                //    }
                //    Bias[i] = Bias[i] + learningRate * Error[i];
                //    for (int j = 0; j < X.Length; j++)
                //    {
                //        W[i, j] += learningRate * Error[i] * X[j];
                //    }
               
                // if hidden layer

                // if output layer

                // ****************************************
            }
    }
}
