using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace DigitNeuralNetwork
{
    //class ActivationFunction
    //{
    //    double func,dfunc;


    //    public ActivationFunction(double func, double dfunc)
    //    {
    //        this.func = func;
    //        this.dfunc = dfunc;
    //    }
    //}

    class NeuralNetwork
    {
        int inputNodes, hiddenNodes, outputNodes;
        double learningRate;
        
        Func<double,double> activationFunction = x => 1 / (1 + Math.Exp(-x)); 
        Func<double,double> activationFunction2 = x => x * (1 - x); 
        //public List<List<double>> weightsIH = new List<List<double>>();
        //List<List<double>> weightsHO = new List<List<double>>();

        //double[,] weightsIH;
        //double[,] weightsHO;
        Matrix<double> weightsIHMatrix;
        Matrix<double> weightsHOMatrix;
        Matrix<double> biasH;
        Matrix<double> biasO;
        Matrix<double> data;

        public NeuralNetwork(int inputNodes, int hiddenNodes, int outputNodes, double learningRate)
        {
            this.inputNodes = inputNodes;
            this.hiddenNodes = hiddenNodes;
            this.outputNodes = outputNodes;

            this.learningRate = learningRate;
            InitializeWeights();
            InitializeBias();
            
        }


        public double[,] Query(double[,] inputArray)
        {
            Matrix<double> inputs = Matrix.Build.DenseOfArray(inputArray);
            Matrix<double> hidden = weightsIHMatrix * inputs;
            //Matrix<double> hidden = Matrix.op_DotMultiply(weightsIHMatrix, inputs);
            //Matrix<double> hidden = weightsIHMatrix.Multiply(inputs);

            hidden += biasH;

            hidden = hidden.Map(activationFunction);

            //Matrix<double> outputs = Matrix.op_DotMultiply(weightsHOMatrix, hidden);
            Matrix<double> outputs = weightsHOMatrix * hidden;

            outputs += biasO;

            outputs = outputs.Map(activationFunction);
            return outputs.ToArray();
        }

        public void Train(double[,] inputArray, double[,] targetArray)
        {
            Matrix<double> inputs = DenseMatrix.OfArray(inputArray);
            //Matrix<double> hidden = Matrix.op_DotMultiply(weightsIHMatrix, inputs);
            Matrix<double> hidden = weightsIHMatrix * inputs;

            hidden += biasH;

            hidden = hidden.Map(activationFunction);

            //Matrix<double> outputs = Matrix.op_DotMultiply(weightsHOMatrix, hidden);
            Matrix<double> outputs = weightsHOMatrix * hidden;

            outputs += biasO;

            outputs = outputs.Map(activationFunction);


            Matrix<double> targets = DenseMatrix.OfArray(targetArray);
            Matrix<double> outputErrors = targets - outputs;
            Matrix<double> gradients = outputs;
            gradients = gradients.Map(activationFunction2);
            //gradients.Multiply(outputErrors);
            //gradients.Multiply(learningRate);

            //Console.WriteLine("inputs - R:" + inputs.RowCount + ", C: " + inputs.ColumnCount);
            //Console.WriteLine("weightsIHMatrix - R:" + weightsIHMatrix.RowCount + ", C: " + weightsIHMatrix.ColumnCount);
            //Console.WriteLine("hidden - R:" + hidden.RowCount + ", C: " + hidden.ColumnCount);
            //Console.WriteLine("weightsHOMatrix - R:" + weightsHOMatrix.RowCount + ", C: " + weightsHOMatrix.ColumnCount);
            //Console.WriteLine("outputs - R:" + outputs.RowCount + ", C: " + outputs.ColumnCount);
            //Console.WriteLine("targets - R:" + targets.RowCount + ", C: " + targets.ColumnCount);
            //Console.WriteLine("outputErrors - R:" + outputErrors.RowCount + ", C: " + outputErrors.ColumnCount);
            //Console.WriteLine("gradients - R:" + gradients.RowCount + ", C: " + gradients.ColumnCount);

            //gradients *= outputErrors;
            gradients = Matrix.op_DotMultiply(gradients, outputErrors);
            gradients *= learningRate;


            var hidden_T = hidden.Transpose();
            var weight_ho_deltas =gradients*hidden_T;

            //this.weightsHOMatrix.Add(weight_ho_deltas);
            weightsHOMatrix += weight_ho_deltas;
            biasO += gradients;

            var weightsHOT = weightsHOMatrix.Transpose();
            //var hiddenErrors = Matrix.op_DotMultiply(weightsHOT, outputErrors);
            var hiddenErrors = weightsHOT * outputErrors;

            var hiddenGradient = hidden.Map(activationFunction2);
            //hiddenGradient.Multiply(hiddenErrors);
            //hiddenGradient.Multiply(this.learningRate);
            hiddenGradient = Matrix.op_DotMultiply(hiddenGradient, hiddenErrors);
            //hiddenGradient.Multiply(learningRate);
            hiddenGradient *= learningRate;
            //hiddenGradient *= learningRate;

            var inputsT = inputs.Transpose();
            var weightIHDeltas = hiddenGradient * inputsT;

            //weightsIHMatrix.Add(weightIHDeltas);
            weightsIHMatrix += weightIHDeltas;
            biasH += hiddenGradient;
            //this.weightsIHMatrix.Add(hiddenGradient);

        }

        //TODO zmienic random na thread safe
        private void InitializeWeights()
        {
            double[,] weightsIH = new double[hiddenNodes, inputNodes];

            Random rnd = new Random();
            for(int i = 0; i < hiddenNodes; i++)
            {
                //weightsIH.Add(new List<double>());
                for(int j = 0; j < inputNodes; j++)
                {
                    //weightsIH.Last().Add(rnd.NextDouble() - 0.5);
                    weightsIH[i,j] = rnd.NextDouble() - 0.5;
                }
            }


            double[,] weightsHO = new double[outputNodes, hiddenNodes];

            for (int i = 0; i < outputNodes; i++)
            {
                //weightsHO.Add(new List<double>());
                for (int j = 0; j < hiddenNodes; j++)
                {
                    //weightsHO.Last().Add(rnd.NextDouble() - 0.5);
                    weightsHO[i, j] = rnd.NextDouble() - 0.5;
                }
            }

            this.weightsIHMatrix = DenseMatrix.OfArray(weightsIH);
            this.weightsHOMatrix = DenseMatrix.OfArray(weightsHO);
        }

        private void InitializeBias()
        {
            double[,] biasHArr = new double[hiddenNodes, 1];
            double[,] biasOArr = new double[outputNodes, 1];
            Random rnd = new Random();
            for (int i = 0; i < hiddenNodes; i++)
            {
                biasHArr[i, 0] = rnd.NextDouble() - 0.5;
            }
            for (int i = 0; i < outputNodes; i++)
            {
                biasOArr[i, 0] = rnd.NextDouble() - 0.5;
            }
            this.biasH = DenseMatrix.OfArray(biasHArr);
            this.biasO = DenseMatrix.OfArray(biasOArr);

        }

        public void LoadData(double[,] data)
        {
            this.data = DenseMatrix.OfArray(data);
        }

        public double[,] GetWeightsIH()
        {
            return this.weightsIHMatrix.ToArray();
        }

        public double[,] GetWeightsHO()
        {
            return this.weightsHOMatrix.ToArray();
        }
    }
}
