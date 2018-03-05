using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids
{
	public class Matrix
	{
		public int numOfRows;
		public int numOfColumns;
		public List<float> values;

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
					this[i, j] = 1.0f / (1.0f + Mathf.Pow((float)Math.E, -this[i, j]));
				}
			}

			return this;
		}

		public void Mutate(float percentChance)
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
		}

		public static Matrix operator +(Matrix lhs, float[] rhs)
		{
			Matrix result = new Matrix(lhs.numOfRows, lhs.numOfColumns);

			for(int i = 0; i < lhs.numOfRows; i++)
			{
				for(int j = 0; j < lhs.numOfColumns; j++)
				{
					result[i, j] = lhs[i, j] + rhs[i];
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

		public static Matrix SingleColumnFromArray(float[] arr)
		{
			Matrix result = new Matrix(arr.Length, 1);

			for(int i = 0; i < arr.Length; i++)
			{
				result[i, 0] = arr[i];
			}

			return result;
		}

		/*public static Matrix AddBias(Matrix mat1)
		{
			Matrix result = new Matrix(mat1.numOfRows + 1, 1);
		}*/
	}
}
