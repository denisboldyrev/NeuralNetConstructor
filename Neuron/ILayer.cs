using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron
{
   public  interface ILayer
    {
        double[] LocalGrad { get; set; } // локальный градиент
        double[,] W { get; set; } // двумерный массив весовых коэффициентов
        double[] X { get; set; } // массив входов
        double[] Output { get; set; } // выход слоя
        int NumNeurons { get; set; } // число нейронов
        double[] Bias { get; set; } // массив смещений
        double[] Error { get; set; }
        double[] LayerOutput();
        void CalcWeights(double learningRate, double[] localGrad);
    }
}
