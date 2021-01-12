using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DigitNeuralNetwork
{
    class NeuralNetwork
    {
        int inputNodes, hiddenNodes, outputNodes;
        double learningRate;
        byte[] data;
        public List<List<double>> weightsIH = new List<List<double>>();
        List<List<double>> weightsHO = new List<List<double>>();
        

        
        
        public NeuralNetwork(int inputNodes, int hiddenNodes, int outputNodes, double learningRate)
        {
            this.inputNodes = inputNodes;
            this.hiddenNodes = hiddenNodes;
            this.outputNodes = outputNodes;

            this.learningRate = learningRate;
            InitializeWeights();
        }


        private void InitializeWeights()
        {
            Random rnd = new Random();
            for(int i = 0; i < hiddenNodes; i++)
            {
                weightsIH.Add(new List<double>());
                for(int j = 0; j < inputNodes; j++)
                {
                    weightsIH.Last().Add(rnd.NextDouble() - 0.5);
                }
            }


            for (int i = 0; i < outputNodes; i++)
            {
                weightsHO.Add(new List<double>());
                for (int j = 0; j < hiddenNodes; j++)
                {
                    weightsHO.Last().Add(rnd.NextDouble() - 0.5);
                }
            }
        }

        public void LoadData(byte[] data)
        {
            this.data = data;
        }

       
    }
}
