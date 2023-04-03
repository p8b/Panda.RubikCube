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
            var cube = new CubeMove(faceSide, rotation);

            // Act
            string result = cube.GetShortString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetMove_ReturnsCorrectMove(bool clockwise)
        {
            // Arrange
            const FaceSide face = FaceSide.Front;
            var expectedResult = clockwise ? Rotation.Clockwise : Rotation.CounterClockwise;

            // Act
            var result = face.GetMove(clockwise);

            // Assert
            Assert.Equal(face, result.Side);
            Assert.Equal(expectedResult, result.Rotation);
        }
    }
}