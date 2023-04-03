using System;
using System.Collections.Generic;

using Panda.RubikCube.Enums;

namespace Panda.RubikCube.Entities
{
    /// <summary>
    ///    Cubelet information.
    /// </summary>
    public class Cubelet : IEquatable<Cubelet?>
    {
        public CubeletColour Colour { get; }
        /// <summary>
        ///    This values is only use as information and it would not be use for equality check.
        /// </summary>
        /// <remarks>If not provided the value will be ("x", "x").</remarks>
        public (string row, string column) OriginalPositon { get; }

        public Cubelet(
            CubeletColour colour,
            (int row, int column)? position = null)
        {
            Colour = colour;
            OriginalPositon = position.HasValue
                ? (position.Value.row.ToString(), position.Value.column.ToString())
                : ("x", "x");
        }

        public static Cubelet Green((int row, int column)? originalPositon = null)
            => new Cubelet(CubeletColour.Green, originalPositon);

        public static Cubelet Blue((int row, int column)? originalPositon = null)
            => new Cubelet(CubeletColour.Blue, originalPositon);

        public static Cubelet Red((int row, int column)? originalPositon = null)
            => new Cubelet(CubeletColour.Red, originalPositon);

        public static Cubelet Orange((int row, int column)? originalPositon = null)
            => new Cubelet(CubeletColour.Orange, originalPositon);

        public static Cubelet White((int row, int column)? originalPositon = null)
            => new Cubelet(CubeletColour.White, originalPositon);

        public static Cubelet Yellow((int row, int column)? originalPositon = null)
            => new Cubelet(CubeletColour.Yellow, originalPositon);

        public static bool operator ==(Cubelet? left, Cubelet? right)
            => EqualityComparer<Cubelet?>.Default.Equals(left, right);

        public static bool operator !=(Cubelet? left, Cubelet? right)
            => !(left == right);

        public override bool Equals(object? obj)
            => Equals(obj as Cubelet);

        public bool Equals(Cubelet? other)
            => !(other is null) && Colour == other.Colour;

        public override int GetHashCode()
            => HashCode.Combine(Colour);
    }
}