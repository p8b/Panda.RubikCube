using System;
using System.Linq;
using System.Threading.Tasks;

using Panda.RubikCube.Enums;

namespace Panda.RubikCube.Entities
{
	/// <summary>
	///    A face of a cube.
	/// </summary>
	public class CubeFace : IEquatable<CubeFace?>
	{
		public int FaceSize { get; }
		public FaceSide OriginalFace { get; }
		public Cubelet[,] Cubelets { get; }

		public CubeFace(FaceSide face, Cubelet[,] cubelets, int faceSize)
		{
			OriginalFace = face;
			FaceSize = faceSize;
			Cubelets = cubelets;
		}

		public CubeFace(FaceSide face, CubeletColour cubeletColour, int faceSize)
		{
			OriginalFace = face;
			FaceSize = faceSize;
			Cubelets = new Cubelet[faceSize, faceSize];

			foreach (int rowIndex in Enumerable.Range(0, faceSize))
			{
				foreach (int columnIndex in Enumerable.Range(0, faceSize))
				{
					Cubelets[rowIndex, columnIndex] = new Cubelet(cubeletColour, (rowIndex, columnIndex));
				}
			}
		}

		public CubeFace(FaceSide face, CubeSettings settings)
			: this(face, settings.CubeletColours[(int)face], settings.Size)
		{ }

		public Cubelet[] GetRow(int rowIndex)
		{
			var rowCubelets = new Cubelet[FaceSize];
			for (int columnIndex = 0; columnIndex < FaceSize; columnIndex++)
			{
				rowCubelets[columnIndex] = Cubelets[rowIndex, columnIndex];
			}
			return rowCubelets;
		}

		public Cubelet[] GetColumn(int columnIndex)
		{
			var columnCubelets = new Cubelet[FaceSize];
			for (int rowIndex = 0; rowIndex < FaceSize; rowIndex++)
			{
				columnCubelets[rowIndex] = Cubelets[rowIndex, columnIndex];
			}
			return columnCubelets;
		}

		public void SetRow(int rowIndex, Cubelet[] columns, bool clockwise)
		{
			if (columns.Length != FaceSize)
				throw new ArgumentException($"The row does not contain the correct number of columns. Size must be {FaceSize}");

			for (int columnIndex = 0; columnIndex < FaceSize; columnIndex++)
			{
				if (clockwise)
					Cubelets[rowIndex, columnIndex] = columns[FaceSize - 1 - columnIndex];
				else
					Cubelets[rowIndex, columnIndex] = columns[columnIndex];
			}
		}

		public void SetColumn(int columnIndex, Cubelet[] rows, bool clockwise)
		{
			if (rows.Length != FaceSize)
				throw new ArgumentException($"The columns does not contain the correct number of rows. Size must be {FaceSize}");

			for (int rowIndex = 0; rowIndex < FaceSize; rowIndex++)
			{
				if (clockwise)
					Cubelets[rowIndex, columnIndex] = rows[rowIndex];
				else
					Cubelets[rowIndex, columnIndex] = rows[FaceSize - rowIndex - 1];
			}
		}

		/// <summary>
		///    Rotate the current face by 90 degree depending on <paramref name="clockwise"/>.
		/// </summary>
		/// <param name="clockwise"></param>
		public void Rotate90(bool clockwise)
		{
			for (int rowIndex = 0; rowIndex < FaceSize / 2; rowIndex++)
			{
				for (int colIndex = rowIndex; colIndex < FaceSize - rowIndex - 1; colIndex++)
				{
					var temp = Cubelets[rowIndex, colIndex];
					if (clockwise)
					{
						Cubelets[rowIndex, colIndex] = Cubelets[FaceSize - colIndex - 1, rowIndex];
						Cubelets[FaceSize - colIndex - 1, rowIndex] = Cubelets[FaceSize - rowIndex - 1, FaceSize - colIndex - 1];
						Cubelets[FaceSize - rowIndex - 1, FaceSize - colIndex - 1] = Cubelets[colIndex, FaceSize - rowIndex - 1];
						Cubelets[colIndex, FaceSize - rowIndex - 1] = temp;
					}
					else
					{
						Cubelets[rowIndex, colIndex] = Cubelets[colIndex, FaceSize - rowIndex - 1];
						Cubelets[colIndex, FaceSize - rowIndex - 1] = Cubelets[FaceSize - rowIndex - 1, FaceSize - colIndex - 1];
						Cubelets[FaceSize - rowIndex - 1, FaceSize - colIndex - 1] = Cubelets[FaceSize - colIndex - 1, rowIndex];
						Cubelets[FaceSize - colIndex - 1, rowIndex] = temp;
					}
				}
			}
		}

		/// <summary>
		///    Rotate the current face by 180 degree.
		/// </summary>
		public void Rotate180()
		{
			var temp = new Cubelet[FaceSize, FaceSize];

			// copy the original array to temp array
			for (int i = 0; i < FaceSize; i++)
			{
				for (int j = 0; j < FaceSize; j++)
				{
					temp[i, j] = Cubelets[i, j];
				}
			}

			// use temp array to rotate and set the original array
			for (int i = 0; i < FaceSize; i++)
			{
				for (int j = 0; j < FaceSize; j++)
				{
					Cubelets[FaceSize - i - 1, FaceSize - j - 1] = temp[i, j];
				}
			}
		}

		public override bool Equals(object? obj)
		{
			return Equals(obj as CubeFace);
		}

		public bool Equals(CubeFace? other)
		{
			if (other == null
			 || FaceSize != other.FaceSize
			 || OriginalFace != other.OriginalFace)
			{
				return false;
			}
			var isEqual = true;
			Parallel.For(0, FaceSize - 1, rowIndex =>
			{
				Parallel.For(0, FaceSize - 1, (columnIndex, state) =>
				{
					if (!Cubelets[rowIndex, columnIndex].Equals(other.Cubelets[rowIndex, columnIndex]))
					{
						isEqual = false;
						state.Stop();
					}
				});
			});
			return isEqual;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				foreach (Cubelet cubelet in Cubelets)
				{
					hash = (hash * 23) + cubelet.GetHashCode();
				}
				return hash;
			}
		}
	}
}