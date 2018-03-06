using System.Collections.Generic;
using UnityEngine;

namespace Baldwin.AI
{
	public class Node
	{
		public readonly List<Connection> children = new List<Connection>();
		public float bias = 0.5f;

		public Node()
		{
			children = new List<Connection>();
			bias = Random.value;
		}

		public Node(List<Node> connectingNodes)
		{
			children = new List<Connection>();
			bias = Random.value;
			AddConnections(connectingNodes);
		}

		public Node(List<Node> connectingNodes, float bias)
		{
			children = new List<Connection>();
			this.bias = bias;
			AddConnections(connectingNodes);
		}

		public void AddConnection(Node connectingNode)
		{
			Connection connection = new Connection();
			connection.child = connectingNode;
			children.Add(connection);
		}

		public void AddConnections(List<Node> connectingNodes)
		{
			foreach(Node iNode in connectingNodes)
			{
				AddConnection(iNode);
			}
		}

		public float GetOutput(float value)
		{
			if(children.Count == 0) //This is an output node
			{
				return value * bias;
			}

			float result = 0;
			//This is either an input node or a hidden node
			foreach(Connection iConnection in children)
			{
				result += iConnection.GetOutput(value);
			}

			return result;
		}

		public void Mutate()
		{
			bias = Mathf.Clamp01(bias + Random.value);
		}
	}
}
