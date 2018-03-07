using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Baldwin
{
	[Serializable]
	public class Matrix
	{
		public readonly int numOfRows;
		public readonly int numOfColumns;
		public readonly List<float> values;

		public Matrix()
		{
			numOfRows = 0;
			numOfColumns = 0;
			values = new List<float>();
		}

		public Matrix(Matrix mat)
		{
			numOfRows = mat.numOfRows;
			numOfColumns = mat.numOfColumns;
			values = new List<float>(mat.values);
		}

		public Matrix(int numOfRows, int numOfColumns)
		{
			this.numOfRows = numOfRows;
			this.numOfColumns = numOfColumns;
			values = new List<float>(this.numOfRows * this.numOfColumns);

			for(int i = 0; i < this.numOfRows; i++)
			{
				for(int j = 0; j < this.numOfColumns; j++)
				{
					values.Add(0);
				}
			}
		}

		public Matrix(List<float> valueList)
		{
			numOfRows = valueList.Count;
			numOfColumns = 1;
			values = new List<float>(numOfRows * numOfColumns);

			for(int i = 0; i < numOfRows; i++)
			{
				for(int j = 0; j < numOfColumns; j++)
				{
					values.Add(valueList[i]);
				}
			}
		}

		public float this[int a, int b]
		{
			get { return values[a * numOfColumns + b]; }
			set { values[a * numOfColumns + b] = value; }
		}

		public void Randomize()
		{
			for(int i = 0; i < numOfRows; i++)
			{
				for(int j = 0; j < numOfColumns; j++)
				{
					this[i, j] = Random.Range(-1.0f, 1.0f);
				}
			}
		}

		public Matrix Sigmoid()
		{
			for(int i = 0; i < numOfRows; i++)
			{
				for(int j = 0; j < numOfColumns; j++)
				{
					this[i, j] = Extensions.Sigmoid(this[i, j]);
				}
			}

			return this;
		}

		public Matrix Mutate(float percentChance)
		{
			for(int i = 0; i < numOfRows; i++)
			{
				for(int j = 0; j < numOfColumns; j++)
				{
					if(Random.value <= percentChance)
					{
						this[i, j] = Mathf.Clamp(this[i, j] + Random.Range(-1.0f, 1.0f), -1.0f, 1.0f);
					}
				}
			}

			return this;
		}

		public static Matrix operator +(Matrix lhs, Matrix rhs)
		{
			Matrix result = new Matrix(lhs.numOfRows, lhs.numOfColumns);

			for(int i = 0; i < lhs.numOfRows; i++)
			{
				for(int j = 0; j < lhs.numOfColumns; j++)
				{
					result[i, j] = lhs[i, j] + rhs[0, j];
				}
			}

			return result;
		}

		public static Matrix operator -(Matrix lhs, Matrix rhs)
		{
			Matrix result = new Matrix(lhs.numOfRows, lhs.numOfColumns);

			for(int i = 0; i < lhs.numOfRows; i++)
			{
				for(int j = 0; j < lhs.numOfColumns; j++)
				{
					result[i, j] = lhs[i, j] - rhs[0, j];
				}
			}

			return result;
		}

		public static Matrix operator *(Matrix lhs, Matrix rhs)
		{
			Matrix result = new Matrix(lhs.numOfRows, rhs.numOfColumns);

			for(int i = 0; i < lhs.numOfRows; i++)
			{
				for(int j = 0; j < rhs.numOfColumns; j++)
				{
					for(int k = 0; k < rhs.numOfRows; k++)
					{
						result[i, j] += lhs[i, k] * rhs[k, j];
					}
				}
			}

			return result;
		}
	}
}
