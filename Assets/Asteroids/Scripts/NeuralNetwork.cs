using System.Collections.Generic;

namespace Asteroids
{
	public class NeuralNetwork
	{
		private const int numOfInputs = 16;
		private const int numOfHiddenNodes = 16;
		private const int numOfOutputs = 4;

		public Matrix inputToHidden;
		public Matrix hiddenToHidden;
		public Matrix hiddenToOutput;
		public float fitness;

		public NeuralNetwork()
		{
			//Add 1 to store the bias
			inputToHidden = new Matrix(numOfHiddenNodes, numOfInputs + 1);
			hiddenToHidden = new Matrix(numOfHiddenNodes, numOfHiddenNodes + 1);
			hiddenToOutput = new Matrix(numOfOutputs, numOfHiddenNodes + 1);
			fitness = 0;
			inputToHidden.Randomize();
			hiddenToHidden.Randomize();
			hiddenToOutput.Randomize();
		}

		public NeuralNetwork(NeuralNetwork copyThis)
		{
			inputToHidden = new Matrix(copyThis.inputToHidden);
			hiddenToHidden = new Matrix(copyThis.hiddenToHidden);
			hiddenToOutput = new Matrix(copyThis.hiddenToOutput);
			fitness = 0;
			inputToHidden.Mutate(0.15f);
			hiddenToHidden.Mutate(0.15f);
			hiddenToOutput.Mutate(0.15f);
		}

		public List<float> Process(List<float> inputValues)
		{
			Matrix inputs = Matrix.SingleColumnFromArray(inputValues.ToArray());
			Matrix i2h = (inputToHidden * inputs).Sigmoid();
			Matrix h2h = (hiddenToHidden * i2h).Sigmoid();
			Matrix h2o = (hiddenToOutput * h2h).Sigmoid();
			return h2o.values;
		}
	}
}
