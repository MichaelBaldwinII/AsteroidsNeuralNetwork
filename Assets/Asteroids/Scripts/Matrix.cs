using System;
using System.Collections.Generic;
using System.Linq;
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
			for (var i = 0; i < RowCount * ColumnCount; i++)
				values.Add(0);
		}

		public Matrix(List<float> valueList)
		{
			RowCount = valueList.Count;
			ColumnCount = 1;
			values = new List<float>(valueList);
		}

		public float this[int i, int j]
		{
			get => values[j * RowCount + i];
			set => values[j * RowCount + i] = value;
		}

		public List<float> GetRow(int i)
		{
			return values.GetRange(i * ColumnCount, ColumnCount);
		}

		public List<float> GetColumn(int j)
		{
			var row = new List<float>(RowCount);
			for (var i = 0; i < RowCount; i++)
				row.Add(this[i, j]);
			return row;
		}

		public void Randomize()
		{
			for (var i = 0; i < RowCount; i++)
				for (var j = 0; j < ColumnCount; j++)
					this[i, j] = Random.Range(-1.0f, 1.0f);
		}

		public void Mutate(float percentChance)
		{
			for (var i = 0; i < RowCount; i++)
				for (var j = 0; j < ColumnCount; j++)
					if (Random.value <= percentChance)
						this[i, j] = Random.Range(-1.0f, 1.0f);
		}

		public static float Dot(List<float> listA, List<float> listB)
		{
			if (listA.Count != listB.Count)
			{
				Debug.LogError("List sizes must match to get the dot product!");
				return 0;
			}

			return listA.Select((t, i) => t * listB[i]).Sum();
		}

		public static Matrix operator *(Matrix lhs, Matrix rhs)
		{
			if (lhs.ColumnCount != rhs.RowCount)
			{
				Debug.LogError("To multiply matrices, the column count must match the row count!");
				return lhs;
			}

			var result = new Matrix(lhs.RowCount, rhs.ColumnCount);

			for (var i = 0; i < lhs.RowCount; i++)
			{
				for (var j = 0; j < rhs.ColumnCount; j++)
				{
					var row = lhs.GetRow(i);
					var column = rhs.GetColumn(j);
					result[i, j] = Dot(row, column);
				}
			}

			return result;
		}
	}
}
