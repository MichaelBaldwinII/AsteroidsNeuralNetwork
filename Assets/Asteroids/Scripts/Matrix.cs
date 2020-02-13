using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids
{
	[Serializable]
	public class Matrix
	{
		public int RowCount { get; private set; }
		public int ColumnCount { get; private set; }
		private readonly List<float> values;

		public Matrix()
		{
			RowCount = 0;
			ColumnCount = 0;
			values = new List<float>();
		}

		public Matrix(Matrix mat)
		{
			RowCount = mat.RowCount;
			ColumnCount = mat.ColumnCount;
			values = new List<float>(mat.values);
		}

		public Matrix(int rowCount, int columnCount)
		{
			RowCount = rowCount;
			ColumnCount = columnCount;
			values = new List<float>(RowCount * ColumnCount);
			for(var i = 0; i < RowCount * ColumnCount; i++)
				values.Add(0);
		}

		public Matrix(List<float> valueList)
		{
			RowCount = valueList.Count;
			ColumnCount = 1;
			values = new List<float>(RowCount * ColumnCount);

			for(var i = 0; i < ColumnCount; i++)
			{
				for(var j = 0; j < RowCount; j++)
				{
					values.Add(valueList[i]);
				}
			}
		}

		public void SetValues(List<float> valueList)
		{
			for(var i = 0; i < ColumnCount; i++)
			{
				for(var j = 0; j < RowCount; j++)
				{
					values.Add(valueList[i]);
				}
			}
		}

		public float this[int i, int j]
		{
			get => values[j * RowCount + i];
			set => values[j * RowCount + i] = value;
		}

		public List<float> GetRow(int i)
		{
			var row = new List<float>(ColumnCount);
			for(var j = 0; j < ColumnCount; j++)
				row.Add(this[i, j]);
			return row;
		}

		public List<float> GetColumn(int j)
		{
			var row = new List<float>(RowCount);
			for(var i = 0; i < RowCount; i++)
				row.Add(this[i, j]);
			return row;
		}

		public void Randomize()
		{
			for(var i = 0; i < ColumnCount; i++)
			{
				for(var j = 0; j < RowCount; j++)
				{
					this[i, j] = Random.Range(-1.0f, 1.0f);
				}
			}
		}

		public void Sigmoid()
		{
			for(var i = 0; i < ColumnCount; i++)
			{
				for(var j = 0; j < RowCount; j++)
				{
					this[i, j] = Extensions.Sigmoid(this[i, j]);
				}
			}
		}

		public void Mutate(float percentChance)
		{
			for(var i = 0; i < ColumnCount; i++)
			{
				for(var j = 0; j < RowCount; j++)
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
			var result = new Matrix(rhs.RowCount, rhs.ColumnCount);

			for(var i = 0; i < rhs.RowCount; i++)
			{
				for(var j = 0; j < rhs.ColumnCount; j++)
				{
					result[i, j] = -rhs[i, j];
				}
			}

			return result;
		}

		public static Matrix operator +(Matrix lhs, Matrix rhs)
		{
			if(lhs.RowCount != rhs.RowCount || lhs.ColumnCount != rhs.ColumnCount)
			{
				Debug.LogError("Cannot subtract two matrices of unequal size!");
				return lhs;
			}

			var result = new Matrix(lhs.RowCount, lhs.ColumnCount);

			for(var i = 0; i < lhs.ColumnCount; i++)
			{
				for(var j = 0; j < lhs.RowCount; j++)
				{
					result[i, j] = lhs[i, j] + rhs[i, j];
				}
			}

			return result;
		}

		public static Matrix operator -(Matrix lhs, Matrix rhs)
		{
			if(lhs.RowCount != rhs.RowCount || lhs.ColumnCount != rhs.ColumnCount)
			{
				Debug.LogError("Cannot subtract two matrices of unequal size!");
				return lhs;
			}

			return lhs + -rhs;
		}

		public static Matrix operator *(Matrix lhs, Matrix rhs)
		{
			if(lhs.RowCount != rhs.ColumnCount)
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

			var result = new Matrix(lhs.RowCount, rhs.ColumnCount);

			for(var i = 0; i < lhs.RowCount; i++)
			{
				for(var j = 0; j < rhs.ColumnCount; j++)
				{
					var row = lhs.GetColumn(i);
					var column = rhs.GetRow(j);
					result[i, j] = Dot(row, column);
				}
			}

			return result;
		}
	}
}
