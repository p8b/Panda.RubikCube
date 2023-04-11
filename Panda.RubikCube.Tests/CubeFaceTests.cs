using Panda.RubikCube.Entities;
using Panda.RubikCube.Enums;

namespace Panda.RubikCube.Tests
{
	public class CubeFaceTests
	{
		[Fact]
		public void Constructors_SetParameters_Correctly()
		{
			// Arrange
			const int expectedFaceSize = 3;
			const CubeletColour expectedColour = CubeletColour.Green;
			const FaceSide expectedOriginalFace = FaceSide.Front;
			var expectedCubelets = new Cubelet[expectedFaceSize, expectedFaceSize]
			{
				{ new Cubelet(expectedColour, (0, 0)), new Cubelet(expectedColour, (0, 1)), new Cubelet(expectedColour, (0, 2)) },
				{ new Cubelet(expectedColour, (1, 0)), new Cubelet(expectedColour, (1, 1)), new Cubelet(expectedColour, (1, 2)) },
				{ new Cubelet(expectedColour, (2, 0)), new Cubelet(expectedColour, (2, 1)), new Cubelet(expectedColour, (2, 2)) }
			};

			// Act
			var cubeFace1 = new CubeFace(expectedOriginalFace, expectedCubelets, expectedFaceSize);
			var cubeFace2 = new CubeFace(expectedOriginalFace, expectedColour, expectedFaceSize);
			var cubeFace3 = new CubeFace(expectedOriginalFace, new CubeSettings(3));

			// Assert
			Assert.Equal(expectedFaceSize, cubeFace1.FaceSize);
			Assert.Equal(expectedOriginalFace, cubeFace1.OriginalFace);
			Assert.Equal(expectedCubelets, cubeFace1.Cubelets);

			Assert.Equal(expectedFaceSize, cubeFace2.FaceSize);
			Assert.Equal(expectedOriginalFace, cubeFace2.OriginalFace);
			Assert.Equal(expectedCubelets, cubeFace2.Cubelets);

			Assert.Equal(expectedFaceSize, cubeFace3.FaceSize);
			Assert.Equal(expectedOriginalFace, cubeFace3.OriginalFace);
			Assert.Equal(expectedCubelets, cubeFace3.Cubelets);
		}

		[Fact]
		public void GetRow_ReturnsCorrectRow()
		{
			// Arrange
			const int faceSize = 3;
			var face = new CubeFace(FaceSide.Front, CubeletColour.Blue, faceSize);
			var expectedRow = new[] {
				Cubelet.Blue((0, 0)),
				Cubelet.Blue((0, 1)),
				Cubelet.Blue((0, 2))
			};

			// Act
			var actualRow = face.GetRow(0);

			// Assert
			Assert.Equal(expectedRow, actualRow);
		}

		[Fact]
		public void GetColumn_ReturnsCorrectColumn()
		{
			// Arrange
			var face = new CubeFace(
				FaceSide.Right,
				new[,]
				{
					{ Cubelet.Red((0, 0)), Cubelet.Red((0, 1)), Cubelet.Red((0, 2)) },
					{ Cubelet.Red((1, 0)), Cubelet.White((1, 1)), Cubelet.Red((1, 2)) },
					{ Cubelet.Red((2, 0)), Cubelet.Blue((2, 1)), Cubelet.Red((2, 2)) }
				},
				3);
			var expectedColumn = new[] { Cubelet.Red((0, 1)), Cubelet.White((1, 1)), Cubelet.Blue((2, 1)) };

			// Act
			var actualColumn = face.GetColumn(1);

			// Assert
			Assert.Equal(expectedColumn, actualColumn);
		}

		[Fact]
		public void SetRow_Clockwise_SetsRowCorrectly()
		{
			// Arrange
			const int faceSize = 3;
			var face = new CubeFace(FaceSide.Front, CubeletColour.Blue, faceSize);
			var rowToSet = new[] { Cubelet.Green((0, 0)), Cubelet.Red((0, 0)), Cubelet.White((0, 0)) };
			var expectedCubelets = new[,]
			{
				{ Cubelet.White((0, 0)), Cubelet.Red((0, 0)), Cubelet.Green((0, 0)) },
				{ Cubelet.Blue((1, 0)), Cubelet.Blue((1, 1)), Cubelet.Blue((1, 2)) },
				{ Cubelet.Blue((2, 0)), Cubelet.Blue((2, 1)), Cubelet.Blue((2, 2)) }
			};

			// Act
			face.SetRow(0, rowToSet, true);

			// Assert
			Assert.Equal(expectedCubelets, face.Cubelets);
		}

		[Fact]
		public void SetRow_CounterClockwise_SetsRowCorrectly()
		{
			// Arrange
			const int faceSize = 3;
			var face = new CubeFace(FaceSide.Front, CubeletColour.Blue, faceSize);
			var rowToSet = new[] {
				Cubelet.Green((1, 0)),
				Cubelet.Red((1, 1)),
				Cubelet.White((1, 2))
			};
			var expectedCubelets = new[,]
			{
				{ Cubelet.Blue((0, 0)), Cubelet.Blue((0, 1)), Cubelet.Blue((0, 2)) },
				{ Cubelet.Green((1, 0)), Cubelet.Red((1, 1)), Cubelet.White((1, 2)) },
				{ Cubelet.Blue((2, 0)), Cubelet.Blue((2, 1)), Cubelet.Blue((2, 2)) }
			};

			// Act
			face.SetRow(1, rowToSet, false);

			// Assert
			Assert.Equal(expectedCubelets, face.Cubelets);
		}

		[Fact]
		public void SetRowAndColumn_ThrowArgumentException_WhenLengthIsNotEqualToFaceSize()
		{
			// Arrange
			var face = new CubeFace(FaceSide.Left, CubeletColour.Orange, 2);
			var dataToSet = new[] {
				Cubelet.Orange((0, 0)),
				Cubelet.Orange((1, 0)),
				Cubelet.Orange((2, 0))
			};

			// Act & Assert
			Assert.Throws<ArgumentException>(() => face.SetRow(0, dataToSet, true));
			Assert.Throws<ArgumentException>(() => face.SetRow(0, dataToSet, false));
			Assert.Throws<ArgumentException>(() => face.SetColumn(0, dataToSet, true));
			Assert.Throws<ArgumentException>(() => face.SetColumn(0, dataToSet, false));
		}

		[Fact]
		public void SetColumn_Clockwise_SetsColumnCorrectly()
		{
			// Arrange
			var face = new CubeFace(
				FaceSide.Front,
				new Cubelet[,]
				{
					{ Cubelet.Blue((0, 0)), Cubelet.Blue((0, 1)) },
					{ Cubelet.Blue((1, 0)), Cubelet.Blue((1, 1)) },
				},
				2);
			var columnToSet = new Cubelet[] { Cubelet.Red((0, 0)), Cubelet.White((0, 0)) };
			var expectedCubelets = new[,]
			{
				{ Cubelet.Red((0, 0)), Cubelet.Blue((0, 1)) },
				{ Cubelet.White((0, 0)), Cubelet.Blue((1, 1)) }
			};

			// Act
			face.SetColumn(0, columnToSet, true);

			// Assert
			Assert.Equal(expectedCubelets, face.Cubelets);
		}

		[Fact]
		public void SetColumn_CounterClockwise_SetsColumnCorrectly()
		{
			// Arrange
			var face = new CubeFace(
				FaceSide.Front,
				new Cubelet[,]
				{
					{ Cubelet.Blue((0, 0)), Cubelet.Blue((0, 1)) },
					{ Cubelet.Blue((1, 0)), Cubelet.Blue((1, 1)) },
				},
				2);
			var columnToSet = new Cubelet[] { Cubelet.Red((0, 0)), Cubelet.Green((0, 1)) };

			var expectedCubelets = new[,]
			{
				{ Cubelet.Green((0, 1)), Cubelet.Blue((0, 1)) },
				{ Cubelet.Red((0, 0)), Cubelet.Blue((1, 1)) }
			};

			// Act
			face.SetColumn(0, columnToSet, false);

			// Assert
			Assert.Equal(expectedCubelets, face.Cubelets);
		}

		[Fact]
		public void Rotate90_Clockwise_RotatesCorrectly()
		{
			// Arrange
			var face = new CubeFace(
				FaceSide.Front,
				new[,]
				{
					{ Cubelet.Green((0, 0)), Cubelet.White((0, 1)), Cubelet.White((0, 2)) },
					{ Cubelet.White((1, 0)), Cubelet.White((1, 1)), Cubelet.White((1, 2)) },
					{ Cubelet.White((2, 0)), Cubelet.White((2, 1)), Cubelet.White((2, 2)) },
				},
				3);

			var expected = new Cubelet[,]
			{
				{ Cubelet.White((2, 0)), Cubelet.White((1, 0)), Cubelet.Green((0, 0)) },
				{ Cubelet.White((2, 1)), Cubelet.White((1, 1)), Cubelet.White((0, 1)) },
				{ Cubelet.White((2, 2)), Cubelet.White((1, 2)), Cubelet.White((0, 2)) },
			};

			// Act
			face.Rotate90(clockwise: true);

			// Assert
			Assert.Equal(expected, face.Cubelets);
		}

		[Fact]
		public void Rotate90_CounterClockwise_RotatesCorrectly()
		{
			// Arrange
			var face = new CubeFace(
				FaceSide.Front,
				new[,]
				{
					{Cubelet.Green((0, 0)), Cubelet.Red((0, 1)), Cubelet.Red((0, 2))},
					{Cubelet.Red((1, 0)), Cubelet.Red((1, 1)), Cubelet.Red((1, 2))},
					{Cubelet.Red((2, 0)), Cubelet.Red((2, 1)), Cubelet.Red((2, 2))},
				},
				3);

			// Act
			face.Rotate90(false);

			// Assert
			var expectedFace = new CubeFace(
				FaceSide.Front,
				new[,]
				{
					{Cubelet.Red((0, 2)), Cubelet.Red((1, 2)), Cubelet.Red((2, 2))},
					{Cubelet.Red((0, 1)), Cubelet.Red((1, 1)), Cubelet.Red((2, 1))},
					{Cubelet.Green((0, 0)), Cubelet.Red((1, 0)), Cubelet.Red((2, 0))},
				},
				3);

			Assert.Equal(expectedFace.Cubelets, face.Cubelets);
		}

		[Fact]
		public void Rotate180_Clockwise_RotatesCorrectly()
		{
			// Arrange
			var face = new CubeFace(
				FaceSide.Front,
				new[,]
				{
					{ Cubelet.Green((0, 0)), Cubelet.White((0, 1)), Cubelet.White((0, 2)) },
					{ Cubelet.White((1, 0)), Cubelet.White((1, 1)), Cubelet.White((1, 2)) },
					{ Cubelet.White((2, 0)), Cubelet.White((2, 1)), Cubelet.White((2, 2)) },
				},
				3);

			var expected = new Cubelet[,]
			{
				{ Cubelet.White((2, 2)), Cubelet.White((2, 1)), Cubelet.White((2, 0)) },
				{ Cubelet.White((1, 2)), Cubelet.White((1, 1)), Cubelet.White((1, 0)) },
				{ Cubelet.White((0, 2)), Cubelet.White((0, 1)), Cubelet.Green((0, 0)) },
			};

			// Act
			face.Rotate180();

			// Assert
			Assert.Equal(expected, face.Cubelets);
		}

		[Fact]
		public void Rotate180_CounterClockwise_RotatesCorrectly()
		{
			// Arrange
			var face = new CubeFace(
				FaceSide.Front,
				new[,]
				{
					{Cubelet.Green((0, 0)), Cubelet.Red((0, 1)), Cubelet.Red((0, 2))},
					{Cubelet.Red((1, 0)), Cubelet.Red((1, 1)), Cubelet.Red((1, 2))},
					{Cubelet.Red((2, 0)), Cubelet.Red((2, 1)), Cubelet.Red((2, 2))},
				},
				3);
			var expectedFace = new CubeFace(
				FaceSide.Front,
				new[,]
				{
					{Cubelet.Red((2, 2)), Cubelet.Red((2, 1)), Cubelet.Red((2, 0))},
					{Cubelet.Red((1, 2)), Cubelet.Red((1, 1)), Cubelet.Red((1, 0))},
					{Cubelet.Red((0, 2)), Cubelet.Red((0, 1)), Cubelet.Green((0, 0))},
				},
				3);

			// Act
			face.Rotate180();

			// Assert

			Assert.Equal(expectedFace.Cubelets, face.Cubelets);
		}

		[Fact]
		public void EqualityCheck_ShouldPass()
		{
			// Arrange
			var cubeFace1 = new CubeFace(FaceSide.Front, new CubeSettings(3));
			var cubeFace2 = new CubeFace(FaceSide.Front, new CubeSettings(3));

			// Act
			var areEqual = cubeFace1.Equals(cubeFace2);
			var hashCode1 = cubeFace1.GetHashCode();
			var hashCode2 = cubeFace2.GetHashCode();

			// Assert
			Assert.True(areEqual);
			Assert.Equal(hashCode1, hashCode2);
		}

		[Fact]
		public void EqualityCheckWithDifferentOriginalFace_AreNotEqual()
		{
			// Arrange
			var cubeFace1 = new CubeFace(FaceSide.Front, new CubeSettings(3));
			var cubeFace2 = new CubeFace(FaceSide.Back, new CubeSettings(3));

			// Act
			var areEqual = cubeFace1.Equals(cubeFace2);
			var hashCode1 = cubeFace1.GetHashCode();
			var hashCode2 = cubeFace2.GetHashCode();

			// Assert
			Assert.False(areEqual);
			Assert.NotEqual(hashCode1, hashCode2);
		}

		[Fact]
		public void EqualityCheckWithDifferentFaceSize_AreNotEqual()
		{
			// Arrange
			var cubeFace1 = new CubeFace(FaceSide.Front, new CubeSettings(1));
			var cubeFace2 = new CubeFace(FaceSide.Front, new CubeSettings(3));

			// Act
			var areEqual = cubeFace1.Equals(cubeFace2);
			var hashCode1 = cubeFace1.GetHashCode();
			var hashCode2 = cubeFace2.GetHashCode();

			// Assert
			Assert.False(areEqual);
			Assert.NotEqual(hashCode1, hashCode2);
		}

		[Fact]
		public void EqualityCheckWithDifferentCubelets_AreNotEqual()
		{
			// Arrange
			const int size = 3;
			var cubeFace1 = new CubeFace(FaceSide.Front,
				 new Cubelet[size, size]
				{
					{ Cubelet.Green((0, 0)), Cubelet.Green((0, 1)), Cubelet.Green((0, 2)) },
					{ Cubelet.Green((1, 0)), Cubelet.Green((1, 1)), Cubelet.Green((1, 2)) },
					{ Cubelet.Green((2, 0)), Cubelet.Green((2, 1)), Cubelet.Green((2, 2)) }
				},
				size);
			var cubeFace2 = new CubeFace(FaceSide.Front,
				 new Cubelet[size, size]
				{
					{ Cubelet.Red((0, 0)), Cubelet.Red((0, 1)), Cubelet.Red((0, 2)) },
					{ Cubelet.Red((1, 0)), Cubelet.Red((1, 1)), Cubelet.Red((1, 2)) },
					{ Cubelet.Red((2, 0)), Cubelet.Red((2, 1)), Cubelet.Red((2, 2)) }
				},
				size);

			// Act
			var areEqual = cubeFace1.Equals(cubeFace2);
			var hashCode1 = cubeFace1.GetHashCode();
			var hashCode2 = cubeFace2.GetHashCode();

			// Assert
			Assert.False(areEqual);
			Assert.NotEqual(hashCode1, hashCode2);
		}

		[Fact]
		public void EqualityCheckWithObjectOfDifferentType_ShouldFail()
		{
			// Arrange
			var cubeFace = new CubeFace(FaceSide.Front, new CubeSettings(3));
			var otherObject = new object();

			// Act
			var areEqual = cubeFace.Equals(otherObject);
			var hashCode1 = cubeFace.GetHashCode();
			var hashCode2 = otherObject.GetHashCode();

			// Assert
			Assert.False(areEqual);
			Assert.NotEqual(hashCode1, hashCode2);
		}

		[Fact]
		public void EqualityChecWithNull_ShouldFail()
		{
			// Arrange
			var cubeFace = new CubeFace(FaceSide.Front, new CubeSettings(3));

			// Act
			var areEqual = cubeFace.Equals(null);

			// Assert
			Assert.False(areEqual);
		}
	}
}