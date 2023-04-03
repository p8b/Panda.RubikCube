using System.Threading.Tasks;

using Panda.RubikCube.Enums;

namespace Panda.RubikCube.Entities
{
    public class RubiksCube
    {
        public CubeFace[] Faces { get; } = new CubeFace[6];
        public int Size { get; }

        /// <summary>
        ///    Create a new Rubik cube.
        /// </summary>
        /// <param name="settings">If null default <see cref="CubeSettings"/> will be used.</param>
        public RubiksCube(CubeSettings? settings = null)
        {
            settings ??= new CubeSettings();

            Size = settings.Size;

            void CreateCubeFace(FaceSide faceIndex)
                => Faces[(int)faceIndex] = new CubeFace(faceIndex, settings);

            CreateCubeFace(FaceSide.Front);
            CreateCubeFace(FaceSide.Right);
            CreateCubeFace(FaceSide.Back);
            CreateCubeFace(FaceSide.Left);
            CreateCubeFace(FaceSide.Up);
            CreateCubeFace(FaceSide.Down);
        }

        public void Execute(FaceSide side, Rotation rotation)
            => Execute(new CubeMove(side, rotation));

        public void Execute(CubeMove move)
        {
            // Take a copy with the parameter face as the front
            var rotatedCube = new CubeFace[6];

            CubeFace GetCubeFace(FaceSide face) => Faces[(int)face];

            rotatedCube[(int)FaceSide.Front] = GetCubeFace(move.Side);
            rotatedCube[(int)FaceSide.Up] = GetCubeFace(move.Side.GetUpFaceOf());
            rotatedCube[(int)FaceSide.Left] = GetCubeFace(move.Side.GetLeftFaceOf());
            rotatedCube[(int)FaceSide.Right] = GetCubeFace(move.Side.GetRightFaceOf());
            rotatedCube[(int)FaceSide.Back] = GetCubeFace(move.Side.GetBackFaceOf());
            rotatedCube[(int)FaceSide.Down] = GetCubeFace(move.Side.GetDownFaceOf());

            CubeFace GetRotatedCubeFace(FaceSide face) => rotatedCube[(int)face];

            // Correct the rotation other faces if front face change from original side
            if (move.Side == FaceSide.Left)
            {
                GetRotatedCubeFace(FaceSide.Up).Rotate90(false);
                GetRotatedCubeFace(FaceSide.Down).Rotate90(true);
            }
            else if (move.Side == FaceSide.Right)
            {
                GetRotatedCubeFace(FaceSide.Up).Rotate90(true);
                GetRotatedCubeFace(FaceSide.Down).Rotate90(false);
            }
            else if (move.Side == FaceSide.Back)
            {
                GetRotatedCubeFace(FaceSide.Up).Rotate180();
                GetRotatedCubeFace(FaceSide.Down).Rotate180();
            }
            else if (move.Side == FaceSide.Up)
            {
                GetRotatedCubeFace(FaceSide.Right).Rotate90(false);
                GetRotatedCubeFace(FaceSide.Left).Rotate90(true);
                GetRotatedCubeFace(FaceSide.Up).Rotate180();
            }
            else if (move.Side == FaceSide.Down)
            {
                GetRotatedCubeFace(FaceSide.Right).Rotate90(true);
                GetRotatedCubeFace(FaceSide.Left).Rotate90(false);
                GetRotatedCubeFace(FaceSide.Down).Rotate180();
            }

            // Set the rotation effect of current front face and on other faces.
            GetRotatedCubeFace(FaceSide.Front).Rotate90(move.Rotation == Rotation.Clockwise);
            if (move.Rotation == Rotation.Clockwise)
            {
                Cubelet[] upRow = GetRotatedCubeFace(FaceSide.Up).GetRow(Size - 1);

                GetRotatedCubeFace(FaceSide.Up)
                    .SetRow(Size - 1, GetRotatedCubeFace(FaceSide.Left).GetColumn(Size - 1), true);

                GetRotatedCubeFace(FaceSide.Left)
                    .SetColumn(Size - 1, GetRotatedCubeFace(FaceSide.Down).GetRow(0), true);

                GetRotatedCubeFace(FaceSide.Down)
                    .SetRow(0, GetRotatedCubeFace(FaceSide.Right).GetColumn(0), true);

                GetRotatedCubeFace(FaceSide.Right)
                    .SetColumn(0, upRow, true);
            }
            else if (move.Rotation == Rotation.CounterClockwise)
            {
                Cubelet[] upRow = GetRotatedCubeFace(FaceSide.Up).GetRow(Size - 1);

                GetRotatedCubeFace(FaceSide.Up)
                    .SetRow(Size - 1, GetRotatedCubeFace(FaceSide.Right).GetColumn(0), false);

                GetRotatedCubeFace(FaceSide.Right)
                    .SetColumn(0, GetRotatedCubeFace(FaceSide.Down).GetRow(0), false);

                GetRotatedCubeFace(FaceSide.Down)
                    .SetRow(0, GetRotatedCubeFace(FaceSide.Left).GetColumn(Size - 1), false);

                GetRotatedCubeFace(FaceSide.Left)
                    .SetColumn(Size - 1, upRow, false);
            }

            // Reverse cube rotations;
            if (move.Side == FaceSide.Left)
            {
                GetRotatedCubeFace(FaceSide.Up).Rotate90(true);
                GetRotatedCubeFace(FaceSide.Down).Rotate90(false);
            }
            else if (move.Side == FaceSide.Right)
            {
                GetRotatedCubeFace(FaceSide.Up).Rotate90(false);
                GetRotatedCubeFace(FaceSide.Down).Rotate90(true);
            }
            else if (move.Side == FaceSide.Back)
            {
                GetRotatedCubeFace(FaceSide.Up).Rotate180();
                GetRotatedCubeFace(FaceSide.Down).Rotate180();
            }
            else if (move.Side == FaceSide.Up)
            {
                GetRotatedCubeFace(FaceSide.Right).Rotate90(true);
                GetRotatedCubeFace(FaceSide.Left).Rotate90(false);
                GetRotatedCubeFace(FaceSide.Up).Rotate180();
            }
            else if (move.Side == FaceSide.Down)
            {
                GetRotatedCubeFace(FaceSide.Right).Rotate90(false);
                GetRotatedCubeFace(FaceSide.Left).Rotate90(true);
                GetRotatedCubeFace(FaceSide.Down).Rotate180();
            }

            // Put back the changed cube.
            Parallel.ForEach(rotatedCube, item => Faces[(int)item.OriginalFace] = item);
        }
    }
}