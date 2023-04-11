using Panda.RubikCube.Entities;
using Panda.RubikCube.Enums;

namespace Panda.RubikCube.Tests
{
	public class CubeletTests
	{
		[Fact]
		public void Constructor_SetsParameters_Correctly()
		{
			// Arrange
			var expectedPosition = ("1", "2");
			const CubeletColour expectedColour = CubeletColour.Green;

			// Act
			var cubelet = new Cubelet(expectedColour, (1, 2));

			// Assert
			Assert.Equal(expectedPosition, cubelet.OriginalPositon);
			Assert.Equal(expectedColour, cubelet.Colour);
		}

		[Theory]
		[InlineData(CubeletColour.Green, 1, 1)]
		[InlineData(CubeletColour.Blue, 1, 1)]
		[InlineData(CubeletColour.Red, 1, 1)]
		[InlineData(CubeletColour.Orange, 1, 1)]
		[InlineData(CubeletColour.White, 1, 1)]
		[InlineData(CubeletColour.Yellow, 1, 1)]
		public void StaticMethods_CreateCubeletCorrectly(CubeletColour colour, int row, int column)
		{
			// Arrange
			var position = (row, column);
			var expectedPosition = (row.ToString(), column.ToString());

			// Act
			var cubelet = colour switch
			{
				CubeletColour.Green => Cubelet.Green(position),
				CubeletColour.Blue => Cubelet.Blue(position),
				CubeletColour.Red => Cubelet.Red(position),
				CubeletColour.Orange => Cubelet.Orange(position),
				CubeletColour.White => Cubelet.White(position),
				_ => Cubelet.Yellow(position),
			};

			// Assert
			Assert.Equal(colour, cubelet.Colour);
			Assert.Equal(expectedPosition, cubelet.OriginalPositon);
		}

		[Fact]
		public void EqualityCheck_ShouldPass()
		{
			// Arrange
			var cubelet1 = new Cubelet(CubeletColour.Green, (1, 1));
			var cubelet2 = new Cubelet(CubeletColour.Green, (1, 1));

			// Act
			var areEqual = cubelet1.Equals(cubelet2);
			var hashCode1 = cubelet1.GetHashCode();
			var hashCode2 = cubelet2.GetHashCode();

			// Assert
			Assert.True(areEqual);
			Assert.Equal(hashCode1, hashCode2);
		}

		[Fact]
		public void EqualityCheckWithDifferentColour_ShouldFail()
		{
			// Arrange
			var cubelet1 = new Cubelet(CubeletColour.Orange, (1, 1));
			var cubelet2 = new Cubelet(CubeletColour.Green, (1, 1));

			// Act
			var areEqual = cubelet1.Equals(cubelet2);
			var hashCode1 = cubelet1.GetHashCode();
			var hashCode2 = cubelet2.GetHashCode();

			// Assert
			Assert.False(areEqual);
			Assert.NotEqual(hashCode1, hashCode2);
		}

		[Fact]
		public void EqualityCheckWithDifferentOriginalPosition_ShouldFail()
		{
			// Arrange
			var cubelet1 = new Cubelet(CubeletColour.Green, (1, 2));
			var cubelet2 = new Cubelet(CubeletColour.Green, (2, 1));

			// Act
			var areEqual = cubelet1.Equals(cubelet2);
			var hashCode1 = cubelet1.GetHashCode();
			var hashCode2 = cubelet2.GetHashCode();

			// Assert
			Assert.False(areEqual);
			Assert.NotEqual(hashCode1, hashCode2);
		}

		[Fact]
		public void EqualityCheckWithObjectOfDifferentType_ShouldFail()
		{
			// Arrange
			var cubelet = new Cubelet(CubeletColour.Green, (1, 1));
			var otherObject = new object();

			// Act
			var areEqual = cubelet.Equals(otherObject);
			var hashCode1 = cubelet.GetHashCode();
			var hashCode2 = otherObject.GetHashCode();

			// Assert
			Assert.False(areEqual);
			Assert.NotEqual(hashCode1, hashCode2);
		}

		[Fact]
		public void EqualityCheckWithNull_ShouldFail()
		{
			// Arrange
			var cubelet = new Cubelet(CubeletColour.Green, (1, 1));

			// Act
			var areEqual = cubelet.Equals(null);

			// Assert
			Assert.False(areEqual);
		}
	}
}