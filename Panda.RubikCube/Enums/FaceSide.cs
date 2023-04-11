using System.ComponentModel.DataAnnotations;

namespace Panda.RubikCube.Enums
{
	/// <summary>
	///    Specifies the faces of a cube.
	/// </summary>
	public enum FaceSide
	{
		[Display(Name = "Front")]
		Front,
		[Display(Name = "Right")]
		Right,
		[Display(Name = "Up")]
		Up,
		[Display(Name = "Back")]
		Back,
		[Display(Name = "Left")]
		Left,
		[Display(Name = "Down")]
		Down,
	}

	public static class FaceSideExtensions
	{
		public static FaceSide GetBackFaceOf(this FaceSide face)
			=> face switch
			{
				FaceSide.Front => FaceSide.Back,
				FaceSide.Right => FaceSide.Left,
				FaceSide.Back => FaceSide.Front,
				FaceSide.Left => FaceSide.Right,
				FaceSide.Up => FaceSide.Down,
				_ => FaceSide.Up,
			};

		public static FaceSide GetRightFaceOf(this FaceSide face)
			=> face switch
			{
				FaceSide.Front => FaceSide.Right,
				FaceSide.Right => FaceSide.Back,
				FaceSide.Back => FaceSide.Left,
				FaceSide.Left => FaceSide.Front,
				_ => FaceSide.Right,
			};

		public static FaceSide GetLeftFaceOf(this FaceSide face)
			=> face switch
			{
				FaceSide.Front => FaceSide.Left,
				FaceSide.Right => FaceSide.Front,
				FaceSide.Back => FaceSide.Right,
				FaceSide.Left => FaceSide.Back,
				_ => FaceSide.Left,
			};

		public static FaceSide GetUpFaceOf(this FaceSide face)
			=> face switch
			{
				FaceSide.Up => FaceSide.Back,
				FaceSide.Down => FaceSide.Front,
				_ => FaceSide.Up,
			};

		public static FaceSide GetDownFaceOf(this FaceSide face)
			=> face switch
			{
				FaceSide.Up => FaceSide.Front,
				FaceSide.Down => FaceSide.Back,
				_ => FaceSide.Down,
			};
	}
}