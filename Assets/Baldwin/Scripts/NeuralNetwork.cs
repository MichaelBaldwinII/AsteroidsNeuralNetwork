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

		public NeuralNetwork()
		{
			inputToHidden = new Matrix(0, 0);
			inputBias = new Matrix(0, 0);
			hiddenToHidden = new Matrix(0, 0);
			hiddenBias = new Matrix(0, 0);
			outputBias = new Matrix(0, 0);
			hiddenToOutput = new Matrix(0, 0);
			fitness = 0;
		}

		public NeuralNetwork(int numOfInputs, int numOfHiddenNodesPerlayer, int numOfOutputs)
		{
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

		/// <summary>
		/// Mutate this NeuralNetworks matrix values based upon a small mutation percentange.
		/// </summary>
		public void Mutate(float mutationPercent)
		{
			inputToHidden.Mutate(mutationPercent);
			inputBias.Mutate(mutationPercent);
			hiddenToHidden.Mutate(mutationPercent);
			hiddenBias.Mutate(mutationPercent);
			outputBias.Mutate(mutationPercent);
			hiddenToOutput.Mutate(mutationPercent);
		}

		public static void SaveToFile(NeuralNetwork network, string fileName)
		{
			string destination = Application.persistentDataPath + "/" + fileName + ".NN";
			FileStream file = File.Exists(destination) ? File.OpenWrite(destination) : File.Create(destination);
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(file, network);
			file.Close();
		}

		public static NeuralNetwork LoadFromFile(string fileName)
		{
			string destination = Application.persistentDataPath + "/" + fileName + ".NN";
			FileStream file = File.Exists(destination) ? File.OpenRead(destination) : File.Create(destination);
			BinaryFormatter bf = new BinaryFormatter();
			NeuralNetwork result = (NeuralNetwork)bf.Deserialize(file);
			file.Close();
			return result;
		}
	}
}
