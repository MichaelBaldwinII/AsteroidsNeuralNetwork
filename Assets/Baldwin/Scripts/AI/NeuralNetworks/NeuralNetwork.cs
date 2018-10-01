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
		public Layer inputLayer;
		public List<Layer> hiddenLayers;
		public Layer outputLayer;
		public float fitness;

		public NeuralNetwork(int numOfInputs, int numOfHiddenLayers, int numOfNodesPerHiddenLayer, int numOfOutputs)
		{
			inputLayer = new Layer(numOfInputs);
			hiddenLayers = new List<Layer>(numOfHiddenLayers);
			for(var i = 0; i < numOfHiddenLayers; i++)
			{
				hiddenLayers.Add(new Layer(numOfNodesPerHiddenLayer));
				foreach(var iLayer in hiddenLayers)
					iLayer.Randomize();
			}
			outputLayer = new Layer(numOfOutputs);
			outputLayer.Randomize();
		}

		public NeuralNetwork(NeuralNetwork otherNetwork)
		{
			inputLayer = new Layer(otherNetwork.inputLayer);
			hiddenLayers = new List<Layer>(otherNetwork.hiddenLayers.Count);
			for(var i = 0; i < otherNetwork.hiddenLayers.Count; i++)
				hiddenLayers.Add(new Layer(otherNetwork.hiddenLayers[i]));
			outputLayer = new Layer(otherNetwork.outputLayer);
		}

		public List<float> Process(List<float> inputValues)
		{
			inputLayer.Process(inputValues);
			for(var i = 0; i < hiddenLayers.Count; i++)
			{
				if(i == 0)
					hiddenLayers[i].Process(inputLayer.values);
				else
					hiddenLayers[i].Process(hiddenLayers[i - 1].values);
			}
			outputLayer.Process(hiddenLayers[hiddenLayers.Count - 1].values);
			outputLayer.Sigmoid();
			return outputLayer.values;
		}

		public void Mutate(float mutationPercent)
		{
			foreach(var iLayer in hiddenLayers)
			{
				iLayer.Mutate(mutationPercent);
			}
			outputLayer.Mutate(mutationPercent);
		}

		#region IComparable

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

		#endregion

		public static void SaveToFile(NeuralNetwork network, string fileName)
		{
			var destination = Application.persistentDataPath + "/" + fileName + ".NN";
			FileStream file = File.Exists(destination) ? File.OpenWrite(destination) : File.Create(destination);
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(file, network);
			file.Close();
		}

		public static NeuralNetwork LoadFromFile(string fileName)
		{
			var destination = Application.persistentDataPath + "/" + fileName + ".NN";
			FileStream file = File.Exists(destination) ? File.OpenRead(destination) : File.Create(destination);
			BinaryFormatter bf = new BinaryFormatter();
			NeuralNetwork result = (NeuralNetwork)bf.Deserialize(file);
			file.Close();
			return result;
		}
	}
}
