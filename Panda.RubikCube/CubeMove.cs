using Panda.RubikCube.Enums;

namespace Panda.RubikCube
{
    /// <summary>
    ///    Represents a single rubiks cube movement.
    /// </summary>
    public class CubeMove
    {
        /// <summary>
        ///    Gets the side corresponding to the move.
        /// </summary>
        public FaceSide Side { get; }

        /// <summary>
        ///    Gets the move direction.
        /// </summary>
        public Rotation Rotation { get; }

        public CubeMove(FaceSide side, Rotation rotation)
        {
            Side = side;
            Rotation = rotation;
        }

        public string GetShortString()
            => Side switch
            {
                FaceSide.Front => Rotation == Rotation.Clockwise ? " F " : " F' ",
                FaceSide.Right => Rotation == Rotation.Clockwise ? " R " : " R' ",
                FaceSide.Back => Rotation == Rotation.Clockwise ? " B " : " B' ",
                FaceSide.Left => Rotation == Rotation.Clockwise ? " L " : " L' ",
                FaceSide.Up => Rotation == Rotation.Clockwise ? " U " : " U' ",
                FaceSide.Down => Rotation == Rotation.Clockwise ? " D " : " D' ",
                _ => "error"
            };
    }
}