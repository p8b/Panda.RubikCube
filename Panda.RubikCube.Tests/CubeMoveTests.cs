using Panda.RubikCube.Enums;

namespace Panda.RubikCube.Tests
{
	public class CubeMoveTests
	{
		[Theory]
		[InlineData(FaceSide.Front, Rotation.Clockwise, " F ")]
		[InlineData(FaceSide.Front, Rotation.CounterClockwise, " F' ")]
		[InlineData(FaceSide.Right, Rotation.Clockwise, " R ")]
		[InlineData(FaceSide.Right, Rotation.CounterClockwise, " R' ")]
		[InlineData(FaceSide.Back, Rotation.Clockwise, " B ")]
		[InlineData(FaceSide.Back, Rotation.CounterClockwise, " B' ")]
		[InlineData(FaceSide.Left, Rotation.Clockwise, " L ")]
		[InlineData(FaceSide.Left, Rotation.CounterClockwise, " L' ")]
		[InlineData(FaceSide.Up, Rotation.Clockwise, " U ")]
		[InlineData(FaceSide.Up, Rotation.CounterClockwise, " U' ")]
		[InlineData(FaceSide.Down, Rotation.Clockwise, " D ")]
		[InlineData(FaceSide.Down, Rotation.CounterClockwise, " D' ")]
		[InlineData((FaceSide)100, Rotation.Clockwise, "error")]
		public void GetShortString_ReturnsCorrectString(FaceSide faceSide, Rotation rotation, string expected)
		{
			// Arrange
			var cube = new CubeMoveRequest(faceSide, rotation);

			// Act
			string result = cube.GetShortString();

			// Assert
			Assert.Equal(expected, result);
		}
	}
}