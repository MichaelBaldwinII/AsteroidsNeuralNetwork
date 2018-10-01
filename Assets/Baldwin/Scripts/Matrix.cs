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
			for(var i = 0; i < this.numOfRows * this.numOfColumns; i++)
				values.Add(0);
		}

		public Matrix(List<float> valueList)
		{
			numOfRows = valueList.Count;
			numOfColumns = 1;
			values = new List<float>(numOfRows * numOfColumns);

			for(var i = 0; i < numOfColumns; i++)
			{
				for(var j = 0; j < numOfRows; j++)
				{
					values.Add(valueList[i]);
				}
			}
		}

		public float this[int i, int j]
		{
			get { return values[j * numOfRows + i]; }
			set { values[j * numOfRows + i] = value; }
		}

		public List<float> GetRow(int i)
		{
			List<float> row = new List<float>(numOfColumns);
			for(var j = 0; j < numOfColumns; j++)
				row.Add(this[i, j]);
			return row;
		}

		public List<float> GetColumn(int j)
		{
			List<float> row = new List<float>(numOfRows);
			for(var i = 0; i < numOfRows; i++)
				row.Add(this[i, j]);
			return row;
		}

		public void Randomize()
		{
			for(var i = 0; i < numOfColumns; i++)
			{
				for(var j = 0; j < numOfRows; j++)
				{
					this[i, j] = Random.Range(-1.0f, 1.0f);
				}
			}
		}

		public void Sigmoid()
		{
			for(var i = 0; i < numOfColumns; i++)
			{
				for(var j = 0; j < numOfRows; j++)
				{
					this[i, j] = Extensions.Sigmoid(this[i, j]);
				}
			}
		}

		public void Mutate(float percentChance)
		{
			for(var i = 0; i < numOfColumns; i++)
			{
				for(var j = 0; j < numOfRows; j++)
				{
					if(Random.value <= percentChance)
					{
						this[i, j] = this[i, j] * Random.Range(-1.5f, 1.5f);
					}
				}
			}
		}

		public static float Dot(List<float> listA, List<float> listB)
		{
			if(listA.Count != listB.Count)
			{
				Debug.LogError("List sizes must match to get the dot product!");
				return 0;
			}

			float result = 0;
			for(var i = 0; i < listA.Count; i++)
				result += listA[i] * listB[i];
			return result;
		}

		public static Matrix operator -(Matrix rhs)
		{
			Matrix result = new Matrix(rhs.numOfRows, rhs.numOfColumns);

			for(var i = 0; i < rhs.numOfRows; i++)
			{
				for(var j = 0; j < rhs.numOfColumns; j++)
				{
					result[i, j] = -rhs[i, j];
				}
			}

			return result;
		}

		public static Matrix operator +(Matrix lhs, Matrix rhs)
		{
			if(lhs.numOfRows != rhs.numOfRows || lhs.numOfColumns != rhs.numOfColumns)
			{
				Debug.LogError("Cannot subtract two matrices of unequal size!");
				return lhs;
			}

			Matrix result = new Matrix(lhs.numOfRows, lhs.numOfColumns);

			for(var i = 0; i < lhs.numOfColumns; i++)
			{
				for(var j = 0; j < lhs.numOfRows; j++)
				{
					result[i, j] = lhs[i, j] + rhs[i, j];
				}
			}

			return result;
		}

		public static Matrix operator -(Matrix lhs, Matrix rhs)
		{
			if(lhs.numOfRows != rhs.numOfRows || lhs.numOfColumns != rhs.numOfColumns)
			{
				Debug.LogError("Cannot subtract two matrices of unequal size!");
				return lhs;
			}

			return lhs + -rhs;
		}

		public static Matrix operator *(Matrix lhs, Matrix rhs)
		{
			if(lhs.numOfRows != rhs.numOfColumns)
			{
				Debug.LogError("To multiply matrices, numOfRows must match numOfColumns!");
				return lhs;
			}

			/*Matrix result = new Matrix(lhs.numOfRows, rhs.numOfColumns);

			for(var i = 0; i < lhs.numOfRows; i++)
			{
				for(var j = 0; j < rhs.numOfColumns; j++)
				{
					for(var k = 0; k < rhs.numOfRows; k++)
					{
						result[i, j] += lhs[k, i] * rhs[j, k];
					}
				}
			}

			return result;*/

			Matrix result = new Matrix(lhs.numOfRows, rhs.numOfColumns);

			for(var i = 0; i < lhs.numOfRows; i++)
			{
				for(var j = 0; j < rhs.numOfColumns; j++)
				{
					List<float> row = lhs.GetRow(i);
					List<float> column = rhs.GetColumn(j);
					result[i, j] = Dot(row, column);
				}
			}

			return result;
		}
	}
}
