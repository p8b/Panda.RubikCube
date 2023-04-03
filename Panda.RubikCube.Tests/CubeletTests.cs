using Panda.RubikCube.Entities;
using Panda.RubikCube.Enums;

namespace Panda.RubikCube.Tests
{
    public class CubeletTests
    {
        [Fact]
        public void Constructor_DefaultPosition_IsCorrect()
        {
            // Arrange
            var expectedPosition = ("x", "x");

            // Act
            var cubelet = new Cubelet(CubeletColour.Green);

            // Assert
            Assert.Equal(expectedPosition, cubelet.OriginalPositon);
        }

        [Fact]
        public void Constructor_CustomPosition_IsCorrect()
        {
            // Arrange
            var expectedPosition = ("1", "2");

            // Act
            var cubelet = new Cubelet(CubeletColour.Green, (1, 2));

            // Assert
            Assert.Equal(expectedPosition, cubelet.OriginalPositon);
        }

        [Theory]
        [InlineData(CubeletColour.Green)]
        [InlineData(CubeletColour.Blue)]
        [InlineData(CubeletColour.Red)]
        [InlineData(CubeletColour.Orange)]
        [InlineData(CubeletColour.White)]
        [InlineData(CubeletColour.Yellow)]
        public void StaticMethods_CreateCubeletWithCorrectColor(CubeletColour colour)
        {
            // Act
            var cubelet = colour switch
            {
                CubeletColour.Green => Cubelet.Green(),
                CubeletColour.Blue => Cubelet.Blue(),
                CubeletColour.Red => Cubelet.Red(),
                CubeletColour.Orange => Cubelet.Orange(),
                CubeletColour.White => Cubelet.White(),
                _ => Cubelet.Yellow(),
            };

            // Assert
            Assert.Equal(colour, cubelet.Colour);
        }

        [Fact]
        public void CubeletsWithSameColourAreEqual()
        {
            // Arrange
            var cubelet1 = new Cubelet(CubeletColour.Green);
            var cubelet2 = new Cubelet(CubeletColour.Green);

            // Act
            var areEqual = cubelet1 == cubelet2;

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void CubeletsWithSameColourAreNotEqual()
        {
            // Arrange
            var cubelet1 = new Cubelet(CubeletColour.Orange);
            var cubelet2 = new Cubelet(CubeletColour.Green);

            // Act
            var areNotEqual = cubelet1 != cubelet2;

            // Assert
            Assert.True(areNotEqual);
        }

        [Fact]
        public void CubeletsWithDifferentColourAreNotEqual()
        {
            // Arrange
            var cubelet1 = new Cubelet(CubeletColour.Green);
            var cubelet2 = new Cubelet(CubeletColour.Blue);

            // Act
            var areEqual = cubelet1 == cubelet2;

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void CubeletsWithSameColourHaveSameHashCode()
        {
            // Arrange
            var cubelet1 = new Cubelet(CubeletColour.Green);
            var cubelet2 = new Cubelet(CubeletColour.Green);

            // Act
            var hashCode1 = cubelet1.GetHashCode();
            var hashCode2 = cubelet2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void CubeletsWithDifferentColourHaveDifferentHashCode()
        {
            // Arrange
            var cubelet1 = new Cubelet(CubeletColour.Green);
            var cubelet2 = new Cubelet(CubeletColour.Blue);

            // Act
            var hashCode1 = cubelet1.GetHashCode();
            var hashCode2 = cubelet2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }

        [Fact]
        public void CubeletsWithSameOriginalPositionAreEqual()
        {
            // Arrange
            var position = (row: 1, column: 2);
            var cubelet1 = new Cubelet(CubeletColour.Green, position);
            var cubelet2 = new Cubelet(CubeletColour.Green, position);

            // Act
            var areEqual = cubelet1 == cubelet2;

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void CubeletsWithDifferentOriginalPositionAreEqual()
        {
            // Arrange
            var position1 = (row: 1, column: 2);
            var position2 = (row: 2, column: 1);
            var cubelet1 = new Cubelet(CubeletColour.Green, position1);
            var cubelet2 = new Cubelet(CubeletColour.Green, position2);

            // Act
            var areEqual = cubelet1 == cubelet2;

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void CubeletsAreNotEqualToObjectOfDifferentType()
        {
            // Arrange
            var cubelet = new Cubelet(CubeletColour.Green);
            var otherObject = new object();

            // Act
            var areEqual = cubelet.Equals(otherObject);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void CubeletsAreNotEqualToNull()
        {
            // Arrange
            var cubelet = new Cubelet(CubeletColour.Green);

            // Act
            var areEqual = cubelet.Equals(null);

            // Assert
            Assert.False(areEqual);
        }
    }
}