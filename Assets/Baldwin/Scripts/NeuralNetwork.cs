using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Baldwin
{
	[Serializable]
	public class NeuralNetwork : IComparable<NeuralNetwork>
	{
		public readonly Matrix inputToHidden;
		public readonly Matrix inputBias;
		public readonly Matrix hiddenToHidden;
		public readonly Matrix hiddenBias;
		public readonly Matrix outputBias;
		public readonly Matrix hiddenToOutput;
		public float fitness;

		public NeuralNetwork(int numOfInputs, int numOfHiddenLayers, int numOfHiddenNodesPerlayer, int numOfOutputs)
		{
			//Add 1 to store the bias
			inputToHidden = new Matrix(numOfHiddenNodesPerlayer, numOfInputs);
			inputBias = new Matrix(numOfInputs, 1);
			hiddenToHidden = new Matrix(numOfHiddenNodesPerlayer, numOfHiddenNodesPerlayer);
			hiddenBias = new Matrix(numOfHiddenNodesPerlayer, 1);
			outputBias = new Matrix(numOfHiddenNodesPerlayer, 1);
			hiddenToOutput = new Matrix(numOfOutputs, numOfHiddenNodesPerlayer);
			fitness = 0;
			inputToHidden.Randomize();
			inputBias.Randomize();
			hiddenToHidden.Randomize();
			hiddenBias.Randomize();
			outputBias.Randomize();
			hiddenToOutput.Randomize();
		}

		public NeuralNetwork(NeuralNetwork neuralNetwork)
		{
			inputToHidden = new Matrix(neuralNetwork.inputToHidden);
			inputBias = new Matrix(neuralNetwork.inputBias);
			hiddenToHidden = new Matrix(neuralNetwork.hiddenToHidden);
			hiddenBias = new Matrix(neuralNetwork.hiddenBias);
			outputBias = new Matrix(neuralNetwork.outputBias);
			hiddenToOutput = new Matrix(neuralNetwork.hiddenToOutput);
			fitness = 0;
			inputToHidden.Mutate(0.05f);
			inputBias.Mutate(0.05f);
			hiddenToHidden.Mutate(0.05f);
			hiddenBias.Mutate(0.05f);
			outputBias.Mutate(0.05f);
			hiddenToOutput.Mutate(0.05f);
		}

		public List<float> Process(List<float> inputValues)
		{
			Matrix inputs = new Matrix(inputValues.ToArray()) + inputBias;
			Matrix i2h = (inputToHidden * inputs).Sigmoid() + hiddenBias;
			Matrix h2h = (hiddenToHidden * i2h).Sigmoid() + outputBias;
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

		public void SaveToFile(string fileName)
		{
			string destination = Application.persistentDataPath + "/" + fileName + ".NN";
			FileStream file;
			if(File.Exists(destination))
			{
				file = File.OpenWrite(destination);
			}
			else
			{
				file = File.Create(destination);
			}

			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(file, this);
			file.Close();
		}
	}
}
