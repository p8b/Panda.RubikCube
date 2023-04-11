using Moq;

using Panda.RubikCube.Entities;
using Panda.RubikCube.Enums;
using Panda.RubikCube.Services;

namespace Panda.RubikCube.Tests
{
	public class RubiksCubeTests
	{
		private readonly Mock<IRubiksCubeSolver> _solverMock;

		public RubiksCubeTests()
		{
			_solverMock = new Mock<IRubiksCubeSolver>();

			_solverMock.Setup(x => x.Solve(
					It.IsAny<RubiksCube>(),
					It.IsAny<Func<Task>?>()
				))
				.Returns(() => Task.FromResult(true));
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public void Cube_HasCorrectSize(int size)
		{
			// Arrange
			var settings = new CubeSettings(size);
			// Arrange
			var cube = new RubiksCube(_solverMock.Object, settings);

			// Assert
			Assert.Equal(settings.Size, cube.Settings.Size);
			Assert.True(cube.Faces.All(x => x.FaceSize == settings.Size));
		}

		[Fact]
		public void Cube_Has3AsDefaultSize()
		{
			// Arrange
			var cube = new RubiksCube(_solverMock.Object);

			// Assert
			Assert.Equal(3, cube.Settings.Size);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(4)] // and above
		public void CubeThrows_WhenIncorrectSizeIsPassed(int size)
		{
			// Arrange
			RubiksCube createCube() => new(_solverMock.Object, new(size));

			// Assert
			Assert.Throws<ArgumentException>((Func<RubiksCube>)createCube);
		}

		[Theory]
		[InlineData(FaceSide.Up)]
		[InlineData(FaceSide.Left)]
		[InlineData(FaceSide.Front)]
		[InlineData(FaceSide.Right)]
		[InlineData(FaceSide.Back)]
		[InlineData(FaceSide.Down)]
		public void RotateAnyFaceClockwiseFourTimes_ShouldSetCubeToOriginalState(FaceSide face)
		{
			// Arrange
			var expectedCube = new RubiksCube(_solverMock.Object, new(3));
			var cube = new RubiksCube(_solverMock.Object, new(3));
			var moveRequest = new CubeMoveRequest(face, Rotation.Clockwise);

			// Act
			foreach (var _ in Enumerable.Range(1, 4))
			{
				cube.Execute(moveRequest);
			}

			//Assert
			Assert.Equal(expectedCube.Faces[(int)FaceSide.Up].Cubelets, cube.Faces[(int)FaceSide.Up].Cubelets);
			Assert.Equal(expectedCube.Faces[(int)FaceSide.Left].Cubelets, cube.Faces[(int)FaceSide.Left].Cubelets);
			Assert.Equal(expectedCube.Faces[(int)FaceSide.Front].Cubelets, cube.Faces[(int)FaceSide.Front].Cubelets);
			Assert.Equal(expectedCube.Faces[(int)FaceSide.Right].Cubelets, cube.Faces[(int)FaceSide.Right].Cubelets);
			Assert.Equal(expectedCube.Faces[(int)FaceSide.Back].Cubelets, cube.Faces[(int)FaceSide.Back].Cubelets);
			Assert.Equal(expectedCube.Faces[(int)FaceSide.Down].Cubelets, cube.Faces[(int)FaceSide.Down].Cubelets);
		}

		[Fact]
		public void RotateFaceClockwise_ShouldWork_AfterFrontAndRightMoves()
		{
			// Arrange
			var expectedFrontSide = new Cubelet[,]
				{
				{ Cubelet.Green((2,0)), Cubelet.Green((1,0)), Cubelet.Red((0,0)) },
				{ Cubelet.Green((2,1)), Cubelet.Green((1,1)), Cubelet.Yellow((1,2)) },
				{ Cubelet.Green((2,2)), Cubelet.Green((1,2)), Cubelet.Yellow((2,2)) }
				};
			var expectedRightSide = new Cubelet[,]
				{
				{ Cubelet.White((2,2)), Cubelet.White((2,1)), Cubelet.White((2,0)) },
				{ Cubelet.Red((2,1)), Cubelet.Red((1,1)), Cubelet.Red((0,1)) },
				{ Cubelet.Red((2,2)), Cubelet.Red((1,2)), Cubelet.Red((0,2)) }
				};
			var expectedBackSide = new Cubelet[,]
				{
				{ Cubelet.Orange((0,2)), Cubelet.Blue((0,1)), Cubelet.Blue((0,2)) },
				{ Cubelet.White((1,2)), Cubelet.Blue((1,1)), Cubelet.Blue((1,2)) },
				{ Cubelet.White((0,2)), Cubelet.Blue((2,1)), Cubelet.Blue((2,2)) }
				};
			var expectedLeftSide = new Cubelet[,]
				{
				{ Cubelet.Orange((0,0)), Cubelet.Orange((0,1)), Cubelet.Yellow((0,0)) },
				{ Cubelet.Orange((1,0)), Cubelet.Orange((1,1)), Cubelet.Yellow((0,1)) },
				{ Cubelet.Orange((2,0)), Cubelet.Orange((2,1)), Cubelet.Yellow((0,2)) }
				};
			var expectedUpSide = new Cubelet[,]
				{
				{ Cubelet.White((0,0)), Cubelet.White((0,1)), Cubelet.Green((0,0)) },
				{ Cubelet.White((1,0)), Cubelet.White((1,1)), Cubelet.Green((0,1)) },
				{ Cubelet.Orange((2,2)), Cubelet.Orange((1,2)), Cubelet.Green((0,2)) }
				};
			var expectedDownSide = new Cubelet[,]
				{
				{ Cubelet.Red((2,0)), Cubelet.Red((1,0)), Cubelet.Blue((2,0)) },
				{ Cubelet.Yellow((1,0)), Cubelet.Yellow((1,1)), Cubelet.Blue((1,0)) },
				{ Cubelet.Yellow((2,0)), Cubelet.Yellow((2,1)), Cubelet.Blue((0,0)) }
				};

			var cube = new RubiksCube(_solverMock.Object, new(3));

			// Act
			cube.Execute(FaceSide.Front, Rotation.Clockwise);
			cube.Execute(FaceSide.Right, Rotation.Clockwise);

			// Assert
			Assert.Equal(expectedFrontSide, cube.Faces[(int)FaceSide.Front].Cubelets);
			Assert.Equal(expectedRightSide, cube.Faces[(int)FaceSide.Right].Cubelets);
			Assert.Equal(expectedBackSide, cube.Faces[(int)FaceSide.Back].Cubelets);
			Assert.Equal(expectedLeftSide, cube.Faces[(int)FaceSide.Left].Cubelets);
			Assert.Equal(expectedUpSide, cube.Faces[(int)FaceSide.Up].Cubelets);
			Assert.Equal(expectedDownSide, cube.Faces[(int)FaceSide.Down].Cubelets);
		}

		[Fact]
		public void RotateFaceClockwise_ShouldWork_AfterFrontAndRightAndFrontMoves()
		{
			// Arrange
			var expectedFrontSide = new Cubelet[,]
				{
				{ Cubelet.Green((2,2)), Cubelet.Green((2,1)), Cubelet.Green((2,0)) },
				{ Cubelet.Green((1,2)), Cubelet.Green((1,1)), Cubelet.Green((1,0)) },
				{ Cubelet.Yellow((2,2)), Cubelet.Yellow((1,2)), Cubelet.Red((0,0)) }
				};
			var expectedRightSide = new Cubelet[,]
				{
				{ Cubelet.Orange((2,2)), Cubelet.White((2,1)), Cubelet.White((2,0)) },
				{ Cubelet.Orange((1,2)), Cubelet.Red((1,1)), Cubelet.Red((0,1)) },
				{ Cubelet.Green((0,2)), Cubelet.Red((1,2)), Cubelet.Red((0,2)) }
				};
			var expectedBackSide = new Cubelet[,]
				{
				{ Cubelet.Orange((0,2)), Cubelet.Blue((0,1)), Cubelet.Blue((0,2)) },
				{ Cubelet.White((1,2)), Cubelet.Blue((1,1)), Cubelet.Blue((1,2)) },
				{ Cubelet.White((0,2)), Cubelet.Blue((2,1)), Cubelet.Blue((2,2)) }
				};
			var expectedLeftSide = new Cubelet[,]
				{
				{ Cubelet.Orange((0,0)), Cubelet.Orange((0,1)), Cubelet.Red((2,0)) },
				{ Cubelet.Orange((1,0)), Cubelet.Orange((1,1)), Cubelet.Red((1,0)) },
				{ Cubelet.Orange((2,0)), Cubelet.Orange((2,1)), Cubelet.Blue((2,0)) }
				};
			var expectedUpSide = new Cubelet[,]
				{
				{ Cubelet.White((0,0)), Cubelet.White((0,1)), Cubelet.Green((0,0)) },
				{ Cubelet.White((1,0)), Cubelet.White((1,1)), Cubelet.Green((0,1)) },
				{ Cubelet.Yellow((0,2)), Cubelet.Yellow((0,1)), Cubelet.Yellow((0,0)) }
				};
			var expectedDownSide = new Cubelet[,]
				{
				{ Cubelet.Red((2,2)), Cubelet.Red((2,1)), Cubelet.White((2,2)) },
				{ Cubelet.Yellow((1,0)), Cubelet.Yellow((1,1)), Cubelet.Blue((1,0)) },
				{ Cubelet.Yellow((2,0)), Cubelet.Yellow((2,1)), Cubelet.Blue((0,0)) }
				};

			var cube = new RubiksCube(_solverMock.Object, new(3));

			// Act
			cube.Execute(FaceSide.Front, Rotation.Clockwise);
			cube.Execute(FaceSide.Right, Rotation.Clockwise);
			cube.Execute(FaceSide.Front, Rotation.Clockwise);

			// Assert
			Assert.Equal(expectedFrontSide, cube.Faces[(int)FaceSide.Front].Cubelets);
			Assert.Equal(expectedRightSide, cube.Faces[(int)FaceSide.Right].Cubelets);
			Assert.Equal(expectedBackSide, cube.Faces[(int)FaceSide.Back].Cubelets);
			Assert.Equal(expectedLeftSide, cube.Faces[(int)FaceSide.Left].Cubelets);
			Assert.Equal(expectedUpSide, cube.Faces[(int)FaceSide.Up].Cubelets);
			Assert.Equal(expectedDownSide, cube.Faces[(int)FaceSide.Down].Cubelets);
		}

		[Fact]
		public void RotateFaceClockwise_ShouldWork_AfterFrontAndRightMovesWerePerformed10Times()
		{
			// Arrange
			var expectedFrontSide = new Cubelet[,]
				{
					{ Cubelet.White((2,0)), Cubelet.Green((1,2)), Cubelet.Red((0,0)) },
					{ Cubelet.Yellow((1,2)), Cubelet.Green((1,1)), Cubelet.White((1,2)) },
					{ Cubelet.Orange((2,2)), Cubelet.Blue((1,0)), Cubelet.Yellow((0,2)) }
				};
			var expectedRightSide = new Cubelet[,]
				{
					{ Cubelet.White((2,2)), Cubelet.Yellow((0,1)), Cubelet.Blue((0,0))},
					{ Cubelet.Red((0,1)), Cubelet.Red((1,1)), Cubelet.Orange((1,2)) },
					{ Cubelet.Green((2,2)), Cubelet.White((2,1)), Cubelet.Yellow((2,2))}
				};
			var expectedBackSide = new Cubelet[,]
				{
					{ Cubelet.White((0,2)), Cubelet.Blue((0,1)), Cubelet.Blue((0,2)) },
					{ Cubelet.Green((1,0)), Cubelet.Blue((1,1)), Cubelet.Blue((1,2)) },
					{ Cubelet.Red((2,2)), Cubelet.Blue((2,1)), Cubelet.Blue((2,2)) }
				};
			var expectedLeftSide = new Cubelet[,]
				{
					{ Cubelet.Orange((0,0)), Cubelet.Orange((0,1)), Cubelet.Green((0,0))},
					{ Cubelet.Orange((1,0)), Cubelet.Orange((1,1)), Cubelet.Red((2,1)) },
					{ Cubelet.Orange((2,0)), Cubelet.Orange((2,1)), Cubelet.Yellow((0,0)) }
				};
			var expectedUpSide = new Cubelet[,]
				{
					{ Cubelet.White((0,0)), Cubelet.White((0,1)), Cubelet.Red((0,2))},
					{ Cubelet.White((1,0)), Cubelet.White((1,1)), Cubelet.Green((2,1)) },
					{ Cubelet.Orange((0,2)), Cubelet.Red((1,0)), Cubelet.Green((0,2))}
				};
			var expectedDownSide = new Cubelet[,]
				{
					{ Cubelet.Green((2,0)), Cubelet.Red((1,2)), Cubelet.Red((2,0)) },
					{ Cubelet.Yellow((1,0)), Cubelet.Yellow((1,1)), Cubelet.Green((0,1)) },
					{ Cubelet.Yellow((2,0)), Cubelet.Yellow((2,1)), Cubelet.Blue((2,0)) }
				};

			var cube = new RubiksCube(_solverMock.Object, new(3));

			// Act
			var actions = Enumerable.Repeat(() =>
			{
				cube.Execute(FaceSide.Front, Rotation.Clockwise);
				cube.Execute(FaceSide.Right, Rotation.Clockwise);
			}, 10);

			foreach (var item in actions)
			{
				item.Invoke();
			}

			// Assert
			Assert.Equal(expectedFrontSide, cube.Faces[(int)FaceSide.Front].Cubelets);
			Assert.Equal(expectedRightSide, cube.Faces[(int)FaceSide.Right].Cubelets);
			Assert.Equal(expectedBackSide, cube.Faces[(int)FaceSide.Back].Cubelets);
			Assert.Equal(expectedLeftSide, cube.Faces[(int)FaceSide.Left].Cubelets);
			Assert.Equal(expectedUpSide, cube.Faces[(int)FaceSide.Up].Cubelets);
			Assert.Equal(expectedDownSide, cube.Faces[(int)FaceSide.Down].Cubelets);
		}

		[Fact]
		public void RotateFaceClockwise_ShouldWork_AfterUpAndLeftMoves()
		{
			// Arrange
			var expectedFrontSide = new Cubelet[,]
				{
					{ Cubelet.White((2,0)), Cubelet.Red((0,1)), Cubelet.Red((0,2)) },
					{ Cubelet.White((2,1)), Cubelet.Green((1,1)), Cubelet.Green((1,2)) },
					{ Cubelet.White((2,2)), Cubelet.Green((2,1)), Cubelet.Green((2,2)) }
				};
			var expectedRightSide = new Cubelet[,]
				{
					{ Cubelet.Blue((0,0)), Cubelet.Blue((0,1)), Cubelet.Blue((0,2)) },
					{ Cubelet.Red((1,0)), Cubelet.Red((1,1)), Cubelet.Red((1,2)) },
					{ Cubelet.Red((2,0)), Cubelet.Red((2,1)), Cubelet.Red((2,2)) }
				};
			var expectedBackSide = new Cubelet[,]
				{
					{ Cubelet.Orange((0,0)), Cubelet.Orange((0,1)), Cubelet.Yellow((2,0)) },
					{ Cubelet.Blue((1,0)), Cubelet.Blue((1,1)), Cubelet.Yellow((1,0)) },
					{ Cubelet.Blue((2,0)), Cubelet.Blue((2,1)), Cubelet.Yellow((0,0)) }
				};
			var expectedLeftSide = new Cubelet[,]
				{
					{ Cubelet.Orange((2,0)), Cubelet.Orange((1,0)), Cubelet.Green((0,0)) },
					{ Cubelet.Orange((2,1)), Cubelet.Orange((1,1)), Cubelet.Green((0,1)) },
					{ Cubelet.Orange((2,2)), Cubelet.Orange((1,2)), Cubelet.Green((0,2)) }
				};
			var expectedUpSide = new Cubelet[,]
				{
					{ Cubelet.Blue((2,2)), Cubelet.White((1,0)), Cubelet.White((0,0)) },
					{ Cubelet.Blue((1,2)), Cubelet.White((1,1)), Cubelet.White((0,1)) },
					{ Cubelet.Orange((0,2)), Cubelet.White((1,2)), Cubelet.White((0,2)) }
				};
			var expectedDownSide = new Cubelet[,]
				{
					{ Cubelet.Red((0,0)), Cubelet.Yellow((0,1)), Cubelet.Yellow((0,2)) },
					{ Cubelet.Green((1,0)), Cubelet.Yellow((1,1)), Cubelet.Yellow((1,2)) },
					{ Cubelet.Green((2,0)), Cubelet.Yellow((2,1)), Cubelet.Yellow((2,2)) }
				};

			var cube = new RubiksCube(_solverMock.Object, new(3));

			// Act
			cube.Execute(FaceSide.Up, Rotation.Clockwise);
			cube.Execute(FaceSide.Left, Rotation.Clockwise);

			// Assert
			Assert.Equal(expectedFrontSide, cube.Faces[(int)FaceSide.Front].Cubelets);
			Assert.Equal(expectedRightSide, cube.Faces[(int)FaceSide.Right].Cubelets);
			Assert.Equal(expectedBackSide, cube.Faces[(int)FaceSide.Back].Cubelets);
			Assert.Equal(expectedLeftSide, cube.Faces[(int)FaceSide.Left].Cubelets);
			Assert.Equal(expectedUpSide, cube.Faces[(int)FaceSide.Up].Cubelets);
			Assert.Equal(expectedDownSide, cube.Faces[(int)FaceSide.Down].Cubelets);
		}

		[Fact]
		public void RotateFaceClockwise_ShouldWork_AfterOneOfEachMove()
		{
			// Arrange
			var expectedFrontSide = new Cubelet[,]
				{
					{ Cubelet.Blue((0,2)), Cubelet.White((2,1)), Cubelet.White((2,0)) },
					{ Cubelet.Orange((1,2)), Cubelet.Green((1,1)), Cubelet.Yellow((1,2)) },
					{ Cubelet.Yellow((0,2)), Cubelet.Yellow((0,1)), Cubelet.Red((0,0)) }
				};
			var expectedRightSide = new Cubelet[,]
				{
					{ Cubelet.Orange((0,2)), Cubelet.Blue((0,1)), Cubelet.Blue((0,0)) },
					{ Cubelet.Red((2,1)), Cubelet.Red((1,1)), Cubelet.Yellow((2,1)) },
					{ Cubelet.Green((0,2)), Cubelet.Green((1,2)), Cubelet.Yellow((2,2)) }
				};
			var expectedBackSide = new Cubelet[,]
				{
					{ Cubelet.White((0,2)), Cubelet.White((1,2)), Cubelet.Green((2,0)) },
					{ Cubelet.Blue((2,1)), Cubelet.Blue((1,1)), Cubelet.Yellow((1,0)) },
					{ Cubelet.Red((2,2)), Cubelet.Red((1,2)), Cubelet.Yellow((2,0)) }
				};
			var expectedLeftSide = new Cubelet[,]
				{
					{ Cubelet.Orange((2,2)), Cubelet.White((1,0)), Cubelet.White((0,0)) },
					{ Cubelet.Orange((2,1)), Cubelet.Orange((1,1)), Cubelet.Green((1,0)) },
					{ Cubelet.Blue((2,2)), Cubelet.Blue((1,2)), Cubelet.Red((2,0)) }
				};
			var expectedUpSide = new Cubelet[,]
				{
					{ Cubelet.Yellow((0,0)), Cubelet.Red((0,1)), Cubelet.Red((0,2)) },
					{ Cubelet.Orange((0,1)), Cubelet.White((1,1)), Cubelet.White((0,1)) },
					{ Cubelet.Orange((0,0)), Cubelet.Green((0,1)), Cubelet.Green((0,0)) }
				};
			var expectedDownSide = new Cubelet[,]
				{
					{ Cubelet.Green((2,2)), Cubelet.Green((2,1)), Cubelet.White((2,2)) },
					{ Cubelet.Orange((1,0)), Cubelet.Yellow((1,1)), Cubelet.Red((1,0)) },
					{ Cubelet.Orange((2,0)), Cubelet.Blue((1,0)), Cubelet.Blue((2,0)) }
				};

			var cube = new RubiksCube(_solverMock.Object, new(3));

			// Act
			cube.Execute(FaceSide.Front, Rotation.Clockwise);
			cube.Execute(FaceSide.Right, Rotation.Clockwise);
			cube.Execute(FaceSide.Up, Rotation.Clockwise);
			cube.Execute(FaceSide.Back, Rotation.Clockwise);
			cube.Execute(FaceSide.Left, Rotation.Clockwise);
			cube.Execute(FaceSide.Down, Rotation.Clockwise);

			// Assert
			Assert.Equal(expectedFrontSide, cube.Faces[(int)FaceSide.Front].Cubelets);
			Assert.Equal(expectedRightSide, cube.Faces[(int)FaceSide.Right].Cubelets);
			Assert.Equal(expectedBackSide, cube.Faces[(int)FaceSide.Back].Cubelets);
			Assert.Equal(expectedLeftSide, cube.Faces[(int)FaceSide.Left].Cubelets);
			Assert.Equal(expectedUpSide, cube.Faces[(int)FaceSide.Up].Cubelets);
			Assert.Equal(expectedDownSide, cube.Faces[(int)FaceSide.Down].Cubelets);
		}

		[Theory]
		[InlineData(FaceSide.Up)]
		[InlineData(FaceSide.Left)]
		[InlineData(FaceSide.Front)]
		[InlineData(FaceSide.Right)]
		[InlineData(FaceSide.Back)]
		[InlineData(FaceSide.Down)]
		public void RotateAnyFaceCounterClockwiseFourTimes_ShouldSetCubeToOriginalState(FaceSide face)
		{
			var expectedCube = new RubiksCube(_solverMock.Object, new(3));
			var cube = new RubiksCube(_solverMock.Object, new(3));

			// Act
			cube.Execute(face, Rotation.CounterClockwise);
			cube.Execute(face, Rotation.CounterClockwise);
			cube.Execute(face, Rotation.CounterClockwise);
			cube.Execute(face, Rotation.CounterClockwise);

			//Assert
			Assert.Equal(expectedCube.Faces[(int)FaceSide.Up].Cubelets, cube.Faces[(int)FaceSide.Up].Cubelets);
			Assert.Equal(expectedCube.Faces[(int)FaceSide.Left].Cubelets, cube.Faces[(int)FaceSide.Left].Cubelets);
			Assert.Equal(expectedCube.Faces[(int)FaceSide.Front].Cubelets, cube.Faces[(int)FaceSide.Front].Cubelets);
			Assert.Equal(expectedCube.Faces[(int)FaceSide.Right].Cubelets, cube.Faces[(int)FaceSide.Right].Cubelets);
			Assert.Equal(expectedCube.Faces[(int)FaceSide.Back].Cubelets, cube.Faces[(int)FaceSide.Back].Cubelets);
			Assert.Equal(expectedCube.Faces[(int)FaceSide.Down].Cubelets, cube.Faces[(int)FaceSide.Down].Cubelets);
		}

		[Fact]
		public void RotateFaceCounterClockwise_ShouldWork_AfterFrontAndRightMoves()
		{
			// Arrange
			var expectedFrontSide = new Cubelet[,]
				{
				{ Cubelet.Green((0,2)), Cubelet.Green((1,2)), Cubelet.White((0,2)) },
				{ Cubelet.Green((0,1)), Cubelet.Green((1,1)), Cubelet.White((1,2)) },
				{ Cubelet.Green((0,0)), Cubelet.Green((1,0)), Cubelet.Red((2,0)) }
				};
			var expectedRightSide = new Cubelet[,]
				{
				{ Cubelet.Red((0,2)), Cubelet.Red((1,2)), Cubelet.Red((2,2)) },
				{ Cubelet.Red((0,1)), Cubelet.Red((1,1)), Cubelet.Red((2,1)) },
				{ Cubelet.Yellow((0,2)), Cubelet.Yellow((0,1)), Cubelet.Yellow((0,0)) }
				};
			var expectedBackSide = new Cubelet[,]
				{
				{ Cubelet.Yellow((2,2)), Cubelet.Blue((0,1)), Cubelet.Blue((0,2)) },
				{ Cubelet.Yellow((1,2)), Cubelet.Blue((1,1)), Cubelet.Blue((1,2)) },
				{ Cubelet.Orange((2,2)), Cubelet.Blue((2,1)), Cubelet.Blue((2,2)) }
				};
			var expectedLeftSide = new Cubelet[,]
				{
				{ Cubelet.Orange((0,0)), Cubelet.Orange((0,1)), Cubelet.White((2,2)) },
				{ Cubelet.Orange((1,0)), Cubelet.Orange((1,1)), Cubelet.White((2,1)) },
				{ Cubelet.Orange((2,0)), Cubelet.Orange((2,1)), Cubelet.White((2,0)) }
				};
			var expectedUpSide = new Cubelet[,]
				{
				{ Cubelet.White((0,0)), Cubelet.White((0,1)), Cubelet.Blue((2,0)) },
				{ Cubelet.White((1,0)), Cubelet.White((1,1)), Cubelet.Blue((1,0)) },
				{ Cubelet.Red((0,0)), Cubelet.Red((1,0)), Cubelet.Blue((0,0)) }
				};
			var expectedDownSide = new Cubelet[,]
				{
				{ Cubelet.Orange((0,2)), Cubelet.Orange((1,2)), Cubelet.Green((2,2)) },
				{ Cubelet.Yellow((1,0)), Cubelet.Yellow((1,1)), Cubelet.Green((2,1)) },
				{ Cubelet.Yellow((2,0)), Cubelet.Yellow((2,1)), Cubelet.Green((2,0)) }
				};

			var cube = new RubiksCube(_solverMock.Object, new(3));

			// Act
			cube.Execute(FaceSide.Front, Rotation.CounterClockwise);
			cube.Execute(FaceSide.Right, Rotation.CounterClockwise);

			// Assert
			Assert.Equal(expectedFrontSide, cube.Faces[(int)FaceSide.Front].Cubelets);
			Assert.Equal(expectedRightSide, cube.Faces[(int)FaceSide.Right].Cubelets);
			Assert.Equal(expectedBackSide, cube.Faces[(int)FaceSide.Back].Cubelets);
			Assert.Equal(expectedLeftSide, cube.Faces[(int)FaceSide.Left].Cubelets);
			Assert.Equal(expectedUpSide, cube.Faces[(int)FaceSide.Up].Cubelets);
			Assert.Equal(expectedDownSide, cube.Faces[(int)FaceSide.Down].Cubelets);
		}

		[Fact]
		public void RotateFaceCounterClockwise_ShouldWork_AfterFrontAndRightAndFrontMoves()
		{
			// Arrange
			var expectedFrontSide = new Cubelet[,]
				{
					{ Cubelet.White((0,2)), Cubelet.White((1,2)), Cubelet.Red((2,0)) },
					{ Cubelet.Green((1,2)), Cubelet.Green((1,1)), Cubelet.Green((1,0)) },
					{ Cubelet.Green((0,2)), Cubelet.Green((0,1)), Cubelet.Green((0,0)) }
				};
			var expectedRightSide = new Cubelet[,]
				{
					{ Cubelet.Green((2,2)), Cubelet.Red((1,2)), Cubelet.Red((2,2)) },
					{ Cubelet.Orange((1,2)), Cubelet.Red((1,1)), Cubelet.Red((2,1)) },
					{ Cubelet.Orange((0,2)), Cubelet.Yellow((0,1)), Cubelet.Yellow((0,0)) }
				};
			var expectedBackSide = new Cubelet[,]
				{
					{ Cubelet.Yellow((2,2)), Cubelet.Blue((0,1)), Cubelet.Blue((0,2)) },
					{ Cubelet.Yellow((1,2)), Cubelet.Blue((1,1)), Cubelet.Blue((1,2)) },
					{ Cubelet.Orange((2,2)), Cubelet.Blue((2,1)), Cubelet.Blue((2,2)) }
				};
			var expectedLeftSide = new Cubelet[,]
				{
					{ Cubelet.Orange((0,0)), Cubelet.Orange((0,1)), Cubelet.Blue((0,0)) },
					{ Cubelet.Orange((1,0)), Cubelet.Orange((1,1)), Cubelet.Red((1,0)) },
					{ Cubelet.Orange((2,0)), Cubelet.Orange((2,1)), Cubelet.Red((0,0)) }
				};
			var expectedUpSide = new Cubelet[,]
				{
					{ Cubelet.White((0,0)), Cubelet.White((0,1)), Cubelet.Blue((2,0)) },
					{ Cubelet.White((1,0)), Cubelet.White((1,1)), Cubelet.Blue((1,0)) },
					{ Cubelet.Red((0,2)), Cubelet.Red((0,1)), Cubelet.Yellow((0,2)) }
				};
			var expectedDownSide = new Cubelet[,]
				{
					{ Cubelet.White((2,2)), Cubelet.White((2,1)), Cubelet.White((2,0)) },
					{ Cubelet.Yellow((1,0)), Cubelet.Yellow((1,1)), Cubelet.Green((2,1)) },
					{ Cubelet.Yellow((2,0)), Cubelet.Yellow((2,1)), Cubelet.Green((2,0)) }
				};

			var cube = new RubiksCube(_solverMock.Object, new(3));

			// Act
			cube.Execute(FaceSide.Front, Rotation.CounterClockwise);
			cube.Execute(FaceSide.Right, Rotation.CounterClockwise);
			cube.Execute(FaceSide.Front, Rotation.CounterClockwise);

			// Assert
			Assert.Equal(expectedFrontSide, cube.Faces[(int)FaceSide.Front].Cubelets);
			Assert.Equal(expectedRightSide, cube.Faces[(int)FaceSide.Right].Cubelets);
			Assert.Equal(expectedBackSide, cube.Faces[(int)FaceSide.Back].Cubelets);
			Assert.Equal(expectedLeftSide, cube.Faces[(int)FaceSide.Left].Cubelets);
			Assert.Equal(expectedUpSide, cube.Faces[(int)FaceSide.Up].Cubelets);
			Assert.Equal(expectedDownSide, cube.Faces[(int)FaceSide.Down].Cubelets);
		}

		[Fact]
		public void RotateFaceCounterClockwise_ShouldWork_AfterFrontAndRightMovesWerePerformed10Times()
		{
			// Arrange
			var expectedFrontSide = new Cubelet[,]
				{
					{ Cubelet.Orange((0,2)), Cubelet.Blue((1,0)), Cubelet.White((2,2)) },
					{ Cubelet.White((1,2)), Cubelet.Green((1,1)), Cubelet.Yellow((1,2)) },
					{ Cubelet.Yellow((0,0)), Cubelet.Green((1,2)), Cubelet.Red((2,0)) }
				};
			var expectedRightSide = new Cubelet[,]
				{
					{ Cubelet.Green((0,2)), Cubelet.Yellow((0,1)), Cubelet.White((0,2))},
					{ Cubelet.Red((2,1)), Cubelet.Red((1,1)), Cubelet.Orange((1,2)) },
					{ Cubelet.Yellow((0,2)), Cubelet.White((2,1)), Cubelet.Blue((2,0))}
				};
			var expectedBackSide = new Cubelet[,]
				{
					{ Cubelet.Red((0,2)), Cubelet.Blue((0,1)), Cubelet.Blue((0,2)) },
					{ Cubelet.Green((1,0)), Cubelet.Blue((1,1)), Cubelet.Blue((1,2)) },
					{ Cubelet.Yellow((2,2)), Cubelet.Blue((2,1)), Cubelet.Blue((2,2)) }
				};
			var expectedLeftSide = new Cubelet[,]
				{
					{ Cubelet.Orange((0,0)), Cubelet.Orange((0,1)), Cubelet.White((2,0))},
					{ Cubelet.Orange((1,0)), Cubelet.Orange((1,1)), Cubelet.Red((0,1)) },
					{ Cubelet.Orange((2,0)), Cubelet.Orange((2,1)), Cubelet.Green((2,0)) }
				};
			var expectedUpSide = new Cubelet[,]
				{
					{ Cubelet.White((0,0)), Cubelet.White((0,1)), Cubelet.Blue((0,0)) },
					{ Cubelet.White((1,0)), Cubelet.White((1,1)), Cubelet.Green((2,1)) },
					{ Cubelet.Green((0,0)), Cubelet.Red((1,2)), Cubelet.Red((0,0)) }
				};
			var expectedDownSide = new Cubelet[,]
				{
					{ Cubelet.Orange((2,2)), Cubelet.Red((1,0)), Cubelet.Green((2,2)) },
					{ Cubelet.Yellow((1,0)), Cubelet.Yellow((1,1)), Cubelet.Green((0,1)) },
					{ Cubelet.Yellow((2,0)), Cubelet.Yellow((2,1)), Cubelet.Red((2,2)) }
				};

			var cube = new RubiksCube(_solverMock.Object, new(3));
			// Act
			var actions = Enumerable.Repeat(() =>
			{
				cube.Execute(FaceSide.Front, Rotation.CounterClockwise);
				cube.Execute(FaceSide.Right, Rotation.CounterClockwise);
			}, 10);

			foreach (var item in actions)
			{
				item.Invoke();
			}

			// Assert
			Assert.Equal(expectedFrontSide, cube.Faces[(int)FaceSide.Front].Cubelets);
			Assert.Equal(expectedRightSide, cube.Faces[(int)FaceSide.Right].Cubelets);
			Assert.Equal(expectedBackSide, cube.Faces[(int)FaceSide.Back].Cubelets);
			Assert.Equal(expectedLeftSide, cube.Faces[(int)FaceSide.Left].Cubelets);
			Assert.Equal(expectedUpSide, cube.Faces[(int)FaceSide.Up].Cubelets);
			Assert.Equal(expectedDownSide, cube.Faces[(int)FaceSide.Down].Cubelets);
		}

		[Fact]
		public void RotateFaceCounterClockwise_ShouldWork_AfterUpAndLeftMoves()
		{
			// Arrange
			var expectedFrontSide = new Cubelet[,]
				{
				{ Cubelet.Yellow((0,0)), Cubelet.Orange((0,1)), Cubelet.Orange((0,2)) },
				{ Cubelet.Yellow((1,0)), Cubelet.Green((1,1)), Cubelet.Green((1,2)) },
				{ Cubelet.Yellow((2,0)), Cubelet.Green((2,1)), Cubelet.Green((2,2)) }
				};
			var expectedRightSide = new Cubelet[,]
				{
				{ Cubelet.Green((0,0)), Cubelet.Green((0,1)), Cubelet.Green((0,2)) },
				{ Cubelet.Red((1,0)), Cubelet.Red((1,1)), Cubelet.Red((1,2)) },
				{ Cubelet.Red((2,0)), Cubelet.Red((2,1)), Cubelet.Red((2,2)) }
				};
			var expectedBackSide = new Cubelet[,]
				{
				{ Cubelet.Red((0,0)), Cubelet.Red((0,1)), Cubelet.White((0,0)) },
				{ Cubelet.Blue((1,0)), Cubelet.Blue((1,1)), Cubelet.White((0,1)) },
				{ Cubelet.Blue((2,0)), Cubelet.Blue((2,1)), Cubelet.White((0,2)) }
				};
			var expectedLeftSide = new Cubelet[,]
				{
				{ Cubelet.Blue((0,2)), Cubelet.Orange((1,2)), Cubelet.Orange((2,2)) },
				{ Cubelet.Blue((0,1)), Cubelet.Orange((1,1)), Cubelet.Orange((2,1)) },
				{ Cubelet.Blue((0,0)), Cubelet.Orange((1,0)), Cubelet.Orange((2,0)) }
				};
			var expectedUpSide = new Cubelet[,]
				{
				{ Cubelet.Orange((0,0)), Cubelet.White((1,2)), Cubelet.White((2,2)) },
				{ Cubelet.Green((1,0)), Cubelet.White((1,1)), Cubelet.White((2,1)) },
				{ Cubelet.Green((2,0)), Cubelet.White((1,0)), Cubelet.White((2,0)) }
				};
			var expectedDownSide = new Cubelet[,]
				{
				{ Cubelet.Blue((2,2)), Cubelet.Yellow((0,1)), Cubelet.Yellow((0,2)) },
				{ Cubelet.Blue((1,2)), Cubelet.Yellow((1,1)), Cubelet.Yellow((1,2)) },
				{ Cubelet.Red((0,2)), Cubelet.Yellow((2,1)), Cubelet.Yellow((2,2)) }
				};

			var cube = new RubiksCube(_solverMock.Object, new(3));

			// Act
			cube.Execute(FaceSide.Up, Rotation.CounterClockwise);
			cube.Execute(FaceSide.Left, Rotation.CounterClockwise);

			// Assert
			Assert.Equal(expectedFrontSide, cube.Faces[(int)FaceSide.Front].Cubelets);
			Assert.Equal(expectedRightSide, cube.Faces[(int)FaceSide.Right].Cubelets);
			Assert.Equal(expectedBackSide, cube.Faces[(int)FaceSide.Back].Cubelets);
			Assert.Equal(expectedLeftSide, cube.Faces[(int)FaceSide.Left].Cubelets);
			Assert.Equal(expectedUpSide, cube.Faces[(int)FaceSide.Up].Cubelets);
			Assert.Equal(expectedDownSide, cube.Faces[(int)FaceSide.Down].Cubelets);
		}

		[Fact]
		public void RotateFaceCounterClockwise_ShouldWork_AfterOneOfEachMove()
		{
			// Arrange
			var expectedFrontSide = new Cubelet[,]
				{
					{ Cubelet.Orange((0,2)), Cubelet.Orange((0,1)), Cubelet.White((2,2)) },
					{ Cubelet.Yellow((1,0)), Cubelet.Green((1,1)), Cubelet.White((1,2)) },
					{ Cubelet.Yellow((0,2)), Cubelet.Yellow((0,1)), Cubelet.Blue((0,0)) }
				};
			var expectedRightSide = new Cubelet[,]
				{
					{ Cubelet.Green((0,2)), Cubelet.Green((1,2)), Cubelet.Blue((2,0)) },
					{ Cubelet.Red((0,1)), Cubelet.Red((1,1)), Cubelet.Blue((1,0)) },
					{ Cubelet.Red((0,2)), Cubelet.Yellow((1,2)), Cubelet.Orange((2,0)) }
				};
			var expectedBackSide = new Cubelet[,]
				{
					{ Cubelet.Red((2,2)), Cubelet.Blue((1,2)), Cubelet.White((0,0)) },
					{ Cubelet.Red((1,2)), Cubelet.Blue((1,1)), Cubelet.White((0,1)) },
					{ Cubelet.Yellow((2,0)), Cubelet.Yellow((2,1)), Cubelet.Green((2,0)) }
				};
			var expectedLeftSide = new Cubelet[,]
				{
					{ Cubelet.Blue((0,2)), Cubelet.White((2,1)), Cubelet.White((2,0)) },
					{ Cubelet.Blue((0,1)), Cubelet.Orange((1,1)), Cubelet.Orange((2,1)) },
					{ Cubelet.Yellow((0,0)), Cubelet.Green((1,0)), Cubelet.Red((2,0)) }
				};
			var expectedUpSide = new Cubelet[,]
				{
					{ Cubelet.Orange((0,0)), Cubelet.Orange((1,0)), Cubelet.Yellow((2,2)) },
					{ Cubelet.Green((0,1)), Cubelet.White((1,1)), Cubelet.Red((1,0)) },
					{ Cubelet.Green((0,0)), Cubelet.White((1,0)), Cubelet.Red((0,0)) }
				};
			var expectedDownSide = new Cubelet[,]
				{
					{ Cubelet.Green((2,2)), Cubelet.Green((2,1)), Cubelet.White((0,2)) },
					{ Cubelet.Orange((1,2)), Cubelet.Yellow((1,1)), Cubelet.Red((2,1)) },
					{ Cubelet.Orange((2,2)), Cubelet.Blue((2,1)), Cubelet.Blue((2,2)) }
				};

			var cube = new RubiksCube(_solverMock.Object, new(3));

			// Act
			cube.Execute(FaceSide.Front, Rotation.CounterClockwise);
			cube.Execute(FaceSide.Right, Rotation.CounterClockwise);
			cube.Execute(FaceSide.Up, Rotation.CounterClockwise);
			cube.Execute(FaceSide.Back, Rotation.CounterClockwise);
			cube.Execute(FaceSide.Left, Rotation.CounterClockwise);
			cube.Execute(FaceSide.Down, Rotation.CounterClockwise);

			// Assert
			Assert.Equal(expectedFrontSide, cube.Faces[(int)FaceSide.Front].Cubelets);
			Assert.Equal(expectedRightSide, cube.Faces[(int)FaceSide.Right].Cubelets);
			Assert.Equal(expectedBackSide, cube.Faces[(int)FaceSide.Back].Cubelets);
			Assert.Equal(expectedLeftSide, cube.Faces[(int)FaceSide.Left].Cubelets);
			Assert.Equal(expectedUpSide, cube.Faces[(int)FaceSide.Up].Cubelets);
			Assert.Equal(expectedDownSide, cube.Faces[(int)FaceSide.Down].Cubelets);
		}

		[Fact]
		public void RotateFaceCounterClockwise_ShouldWork_ExerciseTest()
		{
			// Arrange
			var expectedFrontSide = new Cubelet[,]
				{
					{ Cubelet.Orange((2,0)), Cubelet.Red((1,2)), Cubelet.Red((2,2)) },
					{ Cubelet.Orange((1,2)), Cubelet.Green((1,1)), Cubelet.White((1,2)) },
					{ Cubelet.White((2,0)), Cubelet.White((2,1)), Cubelet.White((0,0)) }
				};
			var expectedRightSide = new Cubelet[,]
				{
					{ Cubelet.Yellow((2,2)), Cubelet.Blue((0,1)), Cubelet.Orange((2,2)) },
					{ Cubelet.Red((0,1)), Cubelet.Red((1,1)), Cubelet.White((1,0)) },
					{ Cubelet.Orange((0,0)), Cubelet.Yellow((1,2)), Cubelet.Red((2,0)) }
				};
			var expectedBackSide = new Cubelet[,]
				{
					{ Cubelet.Yellow((0,0)), Cubelet.Blue((1,2)), Cubelet.White((2,2)) },
					{ Cubelet.Orange((0,1)), Cubelet.Blue((1,1)), Cubelet.Yellow((1,0)) },
					{ Cubelet.Yellow((0,2)), Cubelet.Yellow((0,1)), Cubelet.White((0,2)) }
				};
			var expectedLeftSide = new Cubelet[,]
				{
					{ Cubelet.Green((0,2)), Cubelet.Yellow((2,1)), Cubelet.Yellow((2,0)) },
					{ Cubelet.Orange((2,1)), Cubelet.Orange((1,1)), Cubelet.Green((1,0)) },
					{ Cubelet.Blue((0,0)), Cubelet.Green((1,2)), Cubelet.Orange((0,2)) }
				};
			var expectedUpSide = new Cubelet[,]
				{
					{ Cubelet.Red((0,0)), Cubelet.Orange((1,0)), Cubelet.Green((2,0)) },
					{ Cubelet.Blue((2,1)), Cubelet.White((1,1)), Cubelet.White((0,1)) },
					{ Cubelet.Blue((2,2)), Cubelet.Blue((1,0)), Cubelet.Blue((2,0)) }
				};
			var expectedDownSide = new Cubelet[,]
				{
					{ Cubelet.Green((0,0)), Cubelet.Green((0,1)), Cubelet.Blue((0,2)) },
					{ Cubelet.Red((1,0)), Cubelet.Yellow((1,1)), Cubelet.Red((2,1)) },
					{ Cubelet.Red((0,2)), Cubelet.Green((2,1)), Cubelet.Green((2,2)) }
				};

			var cube = new RubiksCube(_solverMock.Object, new(3));

			// Act
			cube.Execute(FaceSide.Front, Rotation.Clockwise);
			cube.Execute(FaceSide.Right, Rotation.CounterClockwise);
			cube.Execute(FaceSide.Up, Rotation.Clockwise);
			cube.Execute(FaceSide.Back, Rotation.CounterClockwise);
			cube.Execute(FaceSide.Left, Rotation.Clockwise);
			cube.Execute(FaceSide.Down, Rotation.CounterClockwise);

			// Assert
			Assert.Equal(expectedFrontSide, cube.Faces[(int)FaceSide.Front].Cubelets);
			Assert.Equal(expectedRightSide, cube.Faces[(int)FaceSide.Right].Cubelets);
			Assert.Equal(expectedBackSide, cube.Faces[(int)FaceSide.Back].Cubelets);
			Assert.Equal(expectedLeftSide, cube.Faces[(int)FaceSide.Left].Cubelets);
			Assert.Equal(expectedUpSide, cube.Faces[(int)FaceSide.Up].Cubelets);
			Assert.Equal(expectedDownSide, cube.Faces[(int)FaceSide.Down].Cubelets);
		}

		[Fact]
		public async Task SolveMethod_CallsSolverService()
		{
			// Arrange
			var called = false;
			var solverMock = new Mock<IRubiksCubeSolver>();
			solverMock.Setup(x => x.Solve(
					It.IsAny<RubiksCube>(),
					It.IsAny<Func<Task>?>()
				))
				.Returns(() =>
				{
					called = true;
					return Task.FromResult(true);
				});

			var cube = new RubiksCube(solverMock.Object);
			cube.Execute(FaceSide.Left, Rotation.CounterClockwise);

			// Act
			await cube.Solve(null);

			// Assert
			Assert.True(called);
			Assert.True(cube.Solved);
		}

		[Fact]
		public async Task SolveMethod_PassesCallbackActionToSolverService()
		{
			// Arrange
			var callBackCalled = false;
			var solverMock = new Mock<IRubiksCubeSolver>();
			solverMock.Setup(x => x.Solve(
					It.IsAny<RubiksCube>(),
					It.IsAny<Func<Task>?>()
				))
				.Returns(async (RubiksCube _, Func<Task>? action) =>
				{
					if (action != null)
						await action.Invoke();
					return true;
				});

			var cube = new RubiksCube(solverMock.Object);
			cube.Execute(FaceSide.Left, Rotation.CounterClockwise);

			// Act
			await cube.Solve(() =>
			{
				callBackCalled = true;
				return Task.CompletedTask;
			});

			// Assert
			Assert.True(callBackCalled);
		}

		[Fact]
		public async Task SolveMethod_CallsSolverServiceForFirstCallNotTheSecondOne()
		{
			// Arrange
			var cancellationToken = new CancellationTokenSource();
			var called = false;
			var solverMock = new Mock<IRubiksCubeSolver>();
			solverMock.Setup(x => x.Solve(
					It.IsAny<RubiksCube>(),
					It.IsAny<Func<Task>?>()
				))
				.Returns(async () =>
				{
					try
					{
						await Task.Delay(int.MaxValue, cancellationToken.Token);
					}
					catch { }
					called = true;
					return true;
				});

			var cube = new RubiksCube(solverMock.Object);
			cube.Execute(FaceSide.Left, Rotation.CounterClockwise);

			// Act
			var firstSolveCall = cube.Solve(null);
			await cube.Solve(null);

			// Assert
			Assert.False(called);
			Assert.False(cube.Solved);

			// Act
			cancellationToken.Cancel(false);
			await firstSolveCall;

			// Assert
			Assert.True(called);
			Assert.True(cube.Solved);
		}

		[Fact]
		public async Task SolveMethod_WillNotCallSolverService_WhenMixUpIsInProcess()
		{
			// Arrange
			var cancellationToken = new CancellationTokenSource();
			var called = false;
			const int numberOfMoves = 2;
			var solverMock = new Mock<IRubiksCubeSolver>();
			solverMock.Setup(x => x.Solve(
					It.IsAny<RubiksCube>(),
					It.IsAny<Func<Task>?>()
				))
				.Returns(() =>
				{
					called = true;
					return Task.FromResult(true);
				});

			var cube = new RubiksCube(solverMock.Object);
			var mixUpCall = cube.MixUp(numberOfMoves, async () =>
			{
				try
				{
					await Task.Delay(int.MaxValue, cancellationToken.Token);
				}
				catch { }
			});

			// Act
			await cube.Solve(null);

			// Assert
			Assert.False(called);
			Assert.False(cube.Solved);

			// Act
			cancellationToken.Cancel(false);
			await mixUpCall;

			// Assert
			Assert.Equal(numberOfMoves, cube.MoveHistory.Count);
			Assert.False(cube.Solved);
		}

		[Theory]
		[InlineData(20)]
		[InlineData(30)]
		public async Task MixUpMethod_WillMixTheCube(int numberOfMoves)
		{
			// Arrange
			var cube = new RubiksCube(_solverMock.Object);

			// Act
			await cube.MixUp(numberOfMoves, null);

			// Assert
			Assert.Equal(numberOfMoves, cube.MoveHistory.Count);
			Assert.False(cube.Solved);
		}

		[Fact]
		public async Task MixUpMethod_SecondCallWillNotMixTheCube()
		{
			// Arrange
			var cancellationToken = new CancellationTokenSource();
			const int numberOfMoves = 2;
			var cube = new RubiksCube(_solverMock.Object);

			// Act
			var firstMixUpCall = cube.MixUp(numberOfMoves, async () =>
			{
				try
				{
					await Task.Delay(int.MaxValue, cancellationToken.Token);
				}
				catch { }
			});
			await cube.MixUp(numberOfMoves, null);

			// Assert
			Assert.Single(cube.MoveHistory);
			Assert.False(cube.Solved);

			// Act
			cancellationToken.Cancel(false);
			await firstMixUpCall;

			// Assert
			Assert.Equal(numberOfMoves, cube.MoveHistory.Count);
			Assert.False(cube.Solved);
		}

		[Fact]
		public async Task MixUpMethod_WillNotCallMixUp_WhenSolverServiceIsInProcess()
		{
			// Arrange
			var cancellationToken = new CancellationTokenSource();
			var called = false;
			const int numberOfMoves = 2;
			var solverMock = new Mock<IRubiksCubeSolver>();
			solverMock.Setup(x => x.Solve(
					It.IsAny<RubiksCube>(),
					It.IsAny<Func<Task>?>()
				))
				.Returns(async () =>
				{
					try
					{
						await Task.Delay(int.MaxValue, cancellationToken.Token);
					}
					catch { }
					called = true;
					return true;
				});

			var cube = new RubiksCube(solverMock.Object);
			cube.Execute(FaceSide.Left, Rotation.CounterClockwise);
			var solveCall = cube.Solve(null);

			// Act
			await cube.MixUp(numberOfMoves, null);

			// Assert
			Assert.False(cube.Solved);

			// Act
			cancellationToken.Cancel(false);
			await solveCall;

			// Assert
			Assert.True(called);
			Assert.True(cube.Solved);
		}
	}
}