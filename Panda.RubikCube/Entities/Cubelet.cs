using System;

using Panda.RubikCube.Enums;

namespace Panda.RubikCube.Entities
{
	/// <summary>
	///    Cubelet information.
	/// </summary>
	public class Cubelet : IEquatable<Cubelet?>
	{
		public CubeletColour Colour { get; }
		public (string row, string column) OriginalPositon { get; }

		public Cubelet(
			CubeletColour colour,
			(int row, int column) position)
		{
			Colour = colour;
			OriginalPositon = (position.row.ToString(), position.column.ToString());
		}

		public override bool Equals(object? obj)
		{
			return Equals(obj as Cubelet);
		}

		public bool Equals(Cubelet? other)
		{
			return !(other is null) &&
				   Colour == other.Colour &&
				   OriginalPositon.row.Equals(other.OriginalPositon.row) &&
				   OriginalPositon.column.Equals(other.OriginalPositon.column);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Colour, OriginalPositon);
		}

		public static Cubelet Green((int row, int column) originalPositon)
			=> new Cubelet(CubeletColour.Green, originalPositon);

		public static Cubelet Blue((int row, int column) originalPositon)
			=> new Cubelet(CubeletColour.Blue, originalPositon);

		public static Cubelet Red((int row, int column) originalPositon)
			=> new Cubelet(CubeletColour.Red, originalPositon);

		public static Cubelet Orange((int row, int column) originalPositon)
			=> new Cubelet(CubeletColour.Orange, originalPositon);

		public static Cubelet White((int row, int column) originalPositon)
			=> new Cubelet(CubeletColour.White, originalPositon);

		public static Cubelet Yellow((int row, int column) originalPositon)
			=> new Cubelet(CubeletColour.Yellow, originalPositon);
	}
}