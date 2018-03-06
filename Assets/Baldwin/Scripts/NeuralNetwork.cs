using System;
using System.Collections.Generic;

namespace Baldwin
{
	public class NeuralNetwork : IComparable<NeuralNetwork>
	{
		public readonly Matrix inputToHidden;
		public readonly Matrix hiddenToHidden;
		public readonly Matrix hiddenToOutput;
		public float fitness;

		public NeuralNetwork(int numOfInputs, int numOfHiddenLayers, int numOfHiddenNodesPerlayer, int numOfOutputs)
		{
			//Add 1 to store the bias
			inputToHidden = new Matrix(numOfHiddenNodesPerlayer, numOfInputs + 1);
			hiddenToHidden = new Matrix(numOfHiddenNodesPerlayer, numOfHiddenNodesPerlayer + 1);
			hiddenToOutput = new Matrix(numOfOutputs, numOfHiddenNodesPerlayer + 1);
			fitness = 0;
			inputToHidden.Randomize();
			hiddenToHidden.Randomize();
			hiddenToOutput.Randomize();
		}

		public NeuralNetwork(NeuralNetwork neuralNetwork)
		{
			inputToHidden = new Matrix(neuralNetwork.inputToHidden);
			hiddenToHidden = new Matrix(neuralNetwork.hiddenToHidden);
			hiddenToOutput = new Matrix(neuralNetwork.hiddenToOutput);
			fitness = 0;
			inputToHidden.Mutate(0.15f);
			hiddenToHidden.Mutate(0.15f);
			hiddenToOutput.Mutate(0.15f);
		}

		public List<float> Process(List<float> inputValues)
		{
			Matrix inputs = new Matrix(inputValues.ToArray());
			Matrix i2h = (inputToHidden * inputs).Sigmoid();
			Matrix h2h = (hiddenToHidden * i2h).Sigmoid();
			Matrix h2o = (hiddenToOutput * h2h).Sigmoid();
			return h2o.values;
		}

		public int CompareTo(NeuralNetwork other)
		{
			if(fitness > other.fitness)
			{
				return 1;
			}

			if(fitness < other.fitness)
			{
				return -1;
			}

			return 0;
		}
	}
}
