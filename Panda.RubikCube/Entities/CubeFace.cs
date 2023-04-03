using System;
using System.Linq;

using Panda.RubikCube.Enums;

namespace Panda.RubikCube.Entities
{
    /// <summary>
    ///    A face of a cube.
    /// </summary>
    public class CubeFace
    {
        public int FaceSize { get; }
        public FaceSide OriginalFace { get; }
        public Cubelet[,] Cubelets { get; }

        public CubeFace(FaceSide face, Cubelet[,] cubelets)
        {
            OriginalFace = face;
            Cubelets = cubelets;
        }

        public CubeFace(FaceSide face, CubeSettings settings)
            : this(face, settings.CubeletColours[(int)face], settings.Size)
        { }

        public CubeFace(FaceSide face, CubeletColour cubeletColour, int faceSize)
        {
            OriginalFace = face;
            FaceSize = faceSize;
            Cubelets = new Cubelet[faceSize, faceSize];

            foreach (int rowIndex in Enumerable.Range(0, Cubelets.GetLength(0)))
            {
                foreach (int columnIndex in Enumerable.Range(0, Cubelets.GetLength(1)))
                {
                    Cubelets[rowIndex, columnIndex] = new Cubelet(cubeletColour, (rowIndex, columnIndex));
                }
            }
        }

        public Cubelet[] GetRow(int rowIndex)
        {
            var rowSize = Cubelets.GetLength(0);
            var rowCubelets = new Cubelet[rowSize];
            for (int columnIndex = 0; columnIndex < rowSize; columnIndex++)
            {
                rowCubelets[columnIndex] = Cubelets[rowIndex, columnIndex];
            }
            return rowCubelets;
        }

        public Cubelet[] GetColumn(int columnIndex)
        {
            var columnsSize = Cubelets.GetLength(1);
            var columnCubelets = new Cubelet[columnsSize];
            for (int rowIndex = 0; rowIndex < columnsSize; rowIndex++)
            {
                columnCubelets[rowIndex] = Cubelets[rowIndex, columnIndex];
            }
            return columnCubelets;
        }

        public void SetRow(int rowIndex, Cubelet[] columns, bool clockwise)
        {
            int columnSize = Cubelets.GetLength(1);

            if (columns.Length != columnSize)
                throw new ArgumentException($"The row does not contain the correct number of columns. Size must be {FaceSize}");

            for (int columnIndex = 0; columnIndex < columnSize; columnIndex++)
            {
                if (clockwise)
                    Cubelets[rowIndex, columnIndex] = columns[columnSize - 1 - columnIndex];
                else
                    Cubelets[rowIndex, columnIndex] = columns[columnIndex];
            }
        }

        public void SetColumn(int columnIndex, Cubelet[] rows, bool clockwise)
        {
            int rowSize = Cubelets.GetLength(0);

            if (rows.Length != rowSize)
                throw new ArgumentException($"The columns does not contain the correct number of rows. Size must be {FaceSize}");

            for (int rowIndex = 0; rowIndex < rowSize; rowIndex++)
            {
                if (clockwise)
                    Cubelets[rowIndex, columnIndex] = rows[rowIndex];
                else
                    Cubelets[rowIndex, columnIndex] = rows[rowSize - 1 - rowIndex];
            }
        }

        /// <summary>
        ///    Rotate the current face by 90 degree depending on <paramref name="clockwise"/>.
        /// </summary>
        /// <param name="clockwise"></param>
        public void Rotate90(bool clockwise)
        {
            int size = Cubelets.GetLength(clockwise ? 0 : 1);
            var temp = new Cubelet[size, size];

            // copy the original array to temp array
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    temp[i, j] = Cubelets[i, j];
                }
            }

            // use temp array to rotate and set the original array
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (clockwise)
                        Cubelets[j, size - i - 1] = temp[i, j];
                    else
                        Cubelets[size - j - 1, i] = temp[i, j];
                }
            }
        }

        /// <summary>
        ///    Rotate the current face by 180 degree.
        /// </summary>
        public void Rotate180()
        {
            int size = Cubelets.GetLength(0);
            var temp = new Cubelet[size, size];

            // copy the original array to temp array
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    temp[i, j] = Cubelets[i, j];
                }
            }

            // use temp array to rotate and set the original array
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Cubelets[size - i - 1, size - j - 1] = temp[i, j];
                }
            }
        }
    }
}