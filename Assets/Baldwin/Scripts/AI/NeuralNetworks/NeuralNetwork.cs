using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Baldwin.AI
{
	[Serializable]
	public class NeuralNetwork : IComparable<NeuralNetwork>
	{
		public readonly Matrix inputMatrix;
		public readonly List<Matrix> hiddenMatrices = new List<Matrix>();
		public readonly Matrix outputMatrix;
		public readonly Matrix inputBiases;
		public readonly List<Matrix> hiddenBiases = new List<Matrix>();
		public readonly Matrix outputBiases;
		public float fitness;

		public NeuralNetwork(int numOfInputNodes, int numOfHiddenMatrices, int numOfHiddenNodes, int numOfOutputNodes)
		{
			inputMatrix = new Matrix(numOfHiddenNodes, numOfInputNodes);
			inputBiases = new Matrix(numOfInputNodes, 1);
			for(int i = 0; i < numOfHiddenMatrices; i++)
			{
				hiddenMatrices.Add(new Matrix(numOfHiddenNodes, numOfHiddenNodes));
				hiddenBiases.Add(new Matrix(numOfHiddenNodes, 1));
			}
			outputMatrix = new Matrix(numOfOutputNodes, numOfHiddenNodes);
			outputBiases = new Matrix(numOfOutputNodes, 1);
			fitness = 0;

			inputMatrix.Randomize();
			inputBiases.Randomize();
			for(int i = 0; i < numOfHiddenMatrices; i++)
			{
				hiddenMatrices[i].Randomize();
				hiddenBiases[i].Randomize();
			}
			outputMatrix.Randomize();
			outputBiases.Randomize();
		}

		public NeuralNetwork(NeuralNetwork otherNetwork)
		{
			inputMatrix = new Matrix(otherNetwork.inputMatrix);
			inputBiases = new Matrix(otherNetwork.inputBiases);
			for(int i = 0; i < otherNetwork.hiddenMatrices.Count; i++)
			{
				hiddenMatrices.Add(new Matrix(otherNetwork.hiddenMatrices[i]));
				hiddenBiases.Add(new Matrix(otherNetwork.hiddenBiases[i]));
			}
			outputMatrix = new Matrix(otherNetwork.outputMatrix);
			outputBiases = new Matrix(otherNetwork.outputBiases);
			fitness = 0;
		}

		public List<float> Process(List<float> inputValues)
		{
			Matrix inputToHidden = inputMatrix * new Matrix(inputValues);
			inputToHidden -= inputBiases;
			inputToHidden.Sigmoid();

			Matrix hiddenToOutput = new Matrix(inputToHidden);
			for(int i = 0; i < hiddenMatrices.Count; i++)
			{
				hiddenToOutput = hiddenMatrices[i] * hiddenToOutput;
				hiddenToOutput -= hiddenBiases[i];
				hiddenToOutput.Sigmoid();
			}

			Matrix outputToResult = outputMatrix * hiddenToOutput;
			outputToResult -= outputBiases;
			outputToResult.Sigmoid();

			return outputToResult.values;
		}

		public void Mutate(float mutationPercent)
		{
			inputMatrix.Mutate(mutationPercent);
			inputBiases.Mutate(mutationPercent);
			for(int i = 0; i < hiddenMatrices.Count; i++)
			{
				hiddenMatrices[i].Mutate(mutationPercent);
				hiddenBiases[i].Mutate(mutationPercent);
			}
			outputMatrix.Mutate(mutationPercent);
			outputBiases.Mutate(mutationPercent);
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
