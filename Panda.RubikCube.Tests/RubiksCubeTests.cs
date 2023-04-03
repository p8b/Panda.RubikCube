using Panda.RubikCube.Entities;
using Panda.RubikCube.Enums;

namespace Panda.RubikCube.Tests
{
    public class RubiksCubeTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CubeHasCorrectSize(int size)
        {
            // Arrange
            var cube = new RubiksCube(new(size));

            // Assert
            Assert.Equal(size, cube.Size);
        }

        [Fact]
        public void CubeHas3AsDefaultSize()
        {
            // Arrange
            var cube = new RubiksCube();

            // Assert
            Assert.Equal(3, cube.Size);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)] // and above
        public void CubeHasIncorrectSize(int size)
        {
            // Arrange
            RubiksCube createCube() => new(new(size));

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
        public void RotateFaceClockwiseFourTimes_ShouldSetCubeToOriginalState(FaceSide face)
        {
            var expectedCube = new RubiksCube(new(3));
            var cube = new RubiksCube(new(3));
            var move = new CubeMove(face, Rotation.Clockwise);

            // Act
            foreach (var _ in Enumerable.Range(1, 4))
            {
                cube.Execute(move);
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
                { Cubelet.Green(), Cubelet.Green(), Cubelet.Red() },
                { Cubelet.Green(), Cubelet.Green(), Cubelet.Yellow() },
                { Cubelet.Green(), Cubelet.Green(), Cubelet.Yellow() }
                };
            var expectedRightSide = new Cubelet[,]
                {
                { Cubelet.White(), Cubelet.White(), Cubelet.White() },
                { Cubelet.Red(), Cubelet.Red(), Cubelet.Red() },
                { Cubelet.Red(), Cubelet.Red(), Cubelet.Red() }
                };
            var expectedBackSide = new Cubelet[,]
                {
                { Cubelet.Orange(), Cubelet.Blue(), Cubelet.Blue() },
                { Cubelet.White(), Cubelet.Blue(), Cubelet.Blue() },
                { Cubelet.White(), Cubelet.Blue(), Cubelet.Blue() }
                };
            var expectedLeftSide = new Cubelet[,]
                {
                { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Yellow() },
                { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Yellow() },
                { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Yellow() }
                };
            var expectedUpSide = new Cubelet[,]
                {
                { Cubelet.White(), Cubelet.White(), Cubelet.Green() },
                { Cubelet.White(), Cubelet.White(), Cubelet.Green() },
                { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Green() }
                };
            var expectedDownSide = new Cubelet[,]
                {
                { Cubelet.Red(), Cubelet.Red(), Cubelet.Blue() },
                { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Blue() },
                { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Blue() }
                };

            var cube = new RubiksCube(new(3));

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
                { Cubelet.Green(), Cubelet.Green(), Cubelet.Green() },
                { Cubelet.Green(), Cubelet.Green(), Cubelet.Green() },
                { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Red() }
                };
            var expectedRightSide = new Cubelet[,]
                {
                { Cubelet.Orange(), Cubelet.White(), Cubelet.White() },
                { Cubelet.Orange(), Cubelet.Red(), Cubelet.Red() },
                { Cubelet.Green(), Cubelet.Red(), Cubelet.Red() }
                };
            var expectedBackSide = new Cubelet[,]
                {
                { Cubelet.Orange(), Cubelet.Blue(), Cubelet.Blue() },
                { Cubelet.White(), Cubelet.Blue(), Cubelet.Blue() },
                { Cubelet.White(), Cubelet.Blue(), Cubelet.Blue() }
                };
            var expectedLeftSide = new Cubelet[,]
                {
                { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Red() },
                { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Red() },
                { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Blue() }
                };
            var expectedUpSide = new Cubelet[,]
                {
                { Cubelet.White(), Cubelet.White(), Cubelet.Green() },
                { Cubelet.White(), Cubelet.White(), Cubelet.Green() },
                { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Yellow() }
                };
            var expectedDownSide = new Cubelet[,]
                {
                { Cubelet.Red(), Cubelet.Red(), Cubelet.White() },
                { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Blue() },
                { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Blue() }
                };

            var cube = new RubiksCube(new(3));

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
                    { Cubelet.White(), Cubelet.Green(), Cubelet.Red() },
                    { Cubelet.Yellow(), Cubelet.Green(), Cubelet.White() },
                    { Cubelet.Orange(), Cubelet.Blue(), Cubelet.Yellow() }
                };
            var expectedRightSide = new Cubelet[,]
                {
                    { Cubelet.White(), Cubelet.Yellow(), Cubelet.Blue()},
                    { Cubelet.Red(), Cubelet.Red(), Cubelet.Orange() },
                    { Cubelet.Green(), Cubelet.White(), Cubelet.Yellow()}
                };
            var expectedBackSide = new Cubelet[,]
                {
                    { Cubelet.White(), Cubelet.Blue(), Cubelet.Blue() },
                    { Cubelet.Green(), Cubelet.Blue(), Cubelet.Blue() },
                    { Cubelet.Red(), Cubelet.Blue(), Cubelet.Blue() }
                };
            var expectedLeftSide = new Cubelet[,]
                {
                    { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Green()},
                    { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Red() },
                    { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Yellow() }
                };
            var expectedUpSide = new Cubelet[,]
                {
                    { Cubelet.White(), Cubelet.White(), Cubelet.Red()},
                    { Cubelet.White(), Cubelet.White(), Cubelet.Green() },
                    { Cubelet.Orange(), Cubelet.Red(), Cubelet.Green()}
                };
            var expectedDownSide = new Cubelet[,]
                {
                    { Cubelet.Green(), Cubelet.Red(), Cubelet.Red() },
                    { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Green() },
                    { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Blue() }
                };

            var cube = new RubiksCube(new(3));

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
                { Cubelet.White(), Cubelet.Red(), Cubelet.Red() },
                { Cubelet.White(), Cubelet.Green(), Cubelet.Green() },
                { Cubelet.White(), Cubelet.Green(), Cubelet.Green() }
                };
            var expectedRightSide = new Cubelet[,]
                {
                { Cubelet.Blue(), Cubelet.Blue(), Cubelet.Blue() },
                { Cubelet.Red(), Cubelet.Red(), Cubelet.Red() },
                { Cubelet.Red(), Cubelet.Red(), Cubelet.Red() }
                };
            var expectedBackSide = new Cubelet[,]
                {
                { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Yellow() },
                { Cubelet.Blue(), Cubelet.Blue(), Cubelet.Yellow() },
                { Cubelet.Blue(), Cubelet.Blue(), Cubelet.Yellow() }
                };
            var expectedLeftSide = new Cubelet[,]
                {
                { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Green() },
                { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Green() },
                { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Green() }
                };
            var expectedUpSide = new Cubelet[,]
                {
                { Cubelet.Blue(), Cubelet.White(), Cubelet.White() },
                { Cubelet.Blue(), Cubelet.White(), Cubelet.White() },
                { Cubelet.Orange(), Cubelet.White(), Cubelet.White() }
                };
            var expectedDownSide = new Cubelet[,]
                {
                { Cubelet.Red(), Cubelet.Yellow(), Cubelet.Yellow() },
                { Cubelet.Green(), Cubelet.Yellow(), Cubelet.Yellow() },
                { Cubelet.Green(), Cubelet.Yellow(), Cubelet.Yellow() }
                };

            var cube = new RubiksCube(new(3));

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
                    { Cubelet.Blue(), Cubelet.White(), Cubelet.White() },
                    { Cubelet.Orange(), Cubelet.Green(), Cubelet.Yellow() },
                    { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Red() }
                };
            var expectedRightSide = new Cubelet[,]
                {
                    { Cubelet.Orange(), Cubelet.Blue(), Cubelet.Blue() },
                    { Cubelet.Red(), Cubelet.Red(), Cubelet.Yellow() },
                    { Cubelet.Green(), Cubelet.Green(), Cubelet.Yellow() }
                };
            var expectedBackSide = new Cubelet[,]
                {
                    { Cubelet.White(), Cubelet.White(), Cubelet.Green() },
                    { Cubelet.Blue(), Cubelet.Blue(), Cubelet.Yellow() },
                    { Cubelet.Red(), Cubelet.Red(), Cubelet.Yellow() }
                };
            var expectedLeftSide = new Cubelet[,]
                {
                    { Cubelet.Orange(), Cubelet.White(), Cubelet.White() },
                    { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Green() },
                    { Cubelet.Blue(), Cubelet.Blue(), Cubelet.Red() }
                };
            var expectedUpSide = new Cubelet[,]
                {
                    { Cubelet.Yellow(), Cubelet.Red(), Cubelet.Red() },
                    { Cubelet.Orange(), Cubelet.White(), Cubelet.White() },
                    { Cubelet.Orange(), Cubelet.Green(), Cubelet.Green() }
                };
            var expectedDownSide = new Cubelet[,]
                {
                    { Cubelet.Green(), Cubelet.Green(), Cubelet.White() },
                    { Cubelet.Orange(), Cubelet.Yellow(), Cubelet.Red() },
                    { Cubelet.Orange(), Cubelet.Blue(), Cubelet.Blue() }
                };

            var cube = new RubiksCube(new(3));

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
        public void RotateFaceCounterClockwiseFourTimes_ShouldSetCubeToOriginalState(FaceSide face)
        {
            var expectedCube = new RubiksCube(new(3));
            var cube = new RubiksCube(new(3));

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
                { Cubelet.Green(), Cubelet.Green(), Cubelet.White() },
                { Cubelet.Green(), Cubelet.Green(), Cubelet.White() },
                { Cubelet.Green(), Cubelet.Green(), Cubelet.Red() }
                };
            var expectedRightSide = new Cubelet[,]
                {
                { Cubelet.Red(), Cubelet.Red(), Cubelet.Red() },
                { Cubelet.Red(), Cubelet.Red(), Cubelet.Red() },
                { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Yellow() }
                };
            var expectedBackSide = new Cubelet[,]
                {
                { Cubelet.Yellow(), Cubelet.Blue(), Cubelet.Blue() },
                { Cubelet.Yellow(), Cubelet.Blue(), Cubelet.Blue() },
                { Cubelet.Orange(), Cubelet.Blue(), Cubelet.Blue() }
                };
            var expectedLeftSide = new Cubelet[,]
                {
                { Cubelet.Orange(), Cubelet.Orange(), Cubelet.White() },
                { Cubelet.Orange(), Cubelet.Orange(), Cubelet.White() },
                { Cubelet.Orange(), Cubelet.Orange(), Cubelet.White() }
                };
            var expectedUpSide = new Cubelet[,]
                {
                { Cubelet.White(), Cubelet.White(), Cubelet.Blue() },
                { Cubelet.White(), Cubelet.White(), Cubelet.Blue() },
                { Cubelet.Red(), Cubelet.Red(), Cubelet.Blue() }
                };
            var expectedDownSide = new Cubelet[,]
                {
                { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Green() },
                { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Green() },
                { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Green() }
                };

            var cube = new RubiksCube(new(3));

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
                    { Cubelet.White(), Cubelet.White(), Cubelet.Red() },
                    { Cubelet.Green(), Cubelet.Green(), Cubelet.Green() },
                    { Cubelet.Green(), Cubelet.Green(), Cubelet.Green() }
                };
            var expectedRightSide = new Cubelet[,]
                {
                    { Cubelet.Green(), Cubelet.Red(), Cubelet.Red() },
                    { Cubelet.Orange(), Cubelet.Red(), Cubelet.Red() },
                    { Cubelet.Orange(), Cubelet.Yellow(), Cubelet.Yellow() }
                };
            var expectedBackSide = new Cubelet[,]
                {
                    { Cubelet.Yellow(), Cubelet.Blue(), Cubelet.Blue() },
                    { Cubelet.Yellow(), Cubelet.Blue(), Cubelet.Blue() },
                    { Cubelet.Orange(), Cubelet.Blue(), Cubelet.Blue() }
                };
            var expectedLeftSide = new Cubelet[,]
                {
                    { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Blue() },
                    { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Red() },
                    { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Red() }
                };
            var expectedUpSide = new Cubelet[,]
                {
                    { Cubelet.White(), Cubelet.White(), Cubelet.Blue() },
                    { Cubelet.White(), Cubelet.White(), Cubelet.Blue() },
                    { Cubelet.Red(), Cubelet.Red(), Cubelet.Yellow() }
                };
            var expectedDownSide = new Cubelet[,]
                {
                    { Cubelet.White(), Cubelet.White(), Cubelet.White() },
                    { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Green() },
                    { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Green() }
                };

            var cube = new RubiksCube(new(3));

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
                    { Cubelet.Orange(), Cubelet.Blue(), Cubelet.White() },
                    { Cubelet.White(), Cubelet.Green(), Cubelet.Yellow() },
                    { Cubelet.Yellow(), Cubelet.Green(), Cubelet.Red() }
                };
            var expectedRightSide = new Cubelet[,]
                {
                    { Cubelet.Green(), Cubelet.Yellow(), Cubelet.White()},
                    { Cubelet.Red(), Cubelet.Red(), Cubelet.Orange() },
                    { Cubelet.Yellow(), Cubelet.White(), Cubelet.Blue()}
                };
            var expectedBackSide = new Cubelet[,]
                {
                    { Cubelet.Red(), Cubelet.Blue(), Cubelet.Blue() },
                    { Cubelet.Green(), Cubelet.Blue(), Cubelet.Blue() },
                    { Cubelet.Yellow(), Cubelet.Blue(), Cubelet.Blue() }
                };
            var expectedLeftSide = new Cubelet[,]
                {
                    { Cubelet.Orange(), Cubelet.Orange(), Cubelet.White()},
                    { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Red() },
                    { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Green() }
                };
            var expectedUpSide = new Cubelet[,]
                {
                    { Cubelet.White(), Cubelet.White(), Cubelet.Blue() },
                    { Cubelet.White(), Cubelet.White(), Cubelet.Green() },
                    { Cubelet.Green(), Cubelet.Red(), Cubelet.Red() }
                };
            var expectedDownSide = new Cubelet[,]
                {
                    { Cubelet.Orange(), Cubelet.Red(), Cubelet.Green() },
                    { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Green() },
                    { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Red() }
                };

            var cube = new RubiksCube(new(3));
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
                { Cubelet.Yellow(), Cubelet.Orange(), Cubelet.Orange() },
                { Cubelet.Yellow(), Cubelet.Green(), Cubelet.Green() },
                { Cubelet.Yellow(), Cubelet.Green(), Cubelet.Green() }
                };
            var expectedRightSide = new Cubelet[,]
                {
                { Cubelet.Green(), Cubelet.Green(), Cubelet.Green() },
                { Cubelet.Red(), Cubelet.Red(), Cubelet.Red() },
                { Cubelet.Red(), Cubelet.Red(), Cubelet.Red() }
                };
            var expectedBackSide = new Cubelet[,]
                {
                { Cubelet.Red(), Cubelet.Red(), Cubelet.White() },
                { Cubelet.Blue(), Cubelet.Blue(), Cubelet.White() },
                { Cubelet.Blue(), Cubelet.Blue(), Cubelet.White() }
                };
            var expectedLeftSide = new Cubelet[,]
                {
                { Cubelet.Blue(), Cubelet.Orange(), Cubelet.Orange() },
                { Cubelet.Blue(), Cubelet.Orange(), Cubelet.Orange() },
                { Cubelet.Blue(), Cubelet.Orange(), Cubelet.Orange() }
                };
            var expectedUpSide = new Cubelet[,]
                {
                { Cubelet.Orange(), Cubelet.White(), Cubelet.White() },
                { Cubelet.Green(), Cubelet.White(), Cubelet.White() },
                { Cubelet.Green(), Cubelet.White(), Cubelet.White() }
                };
            var expectedDownSide = new Cubelet[,]
                {
                { Cubelet.Blue(), Cubelet.Yellow(), Cubelet.Yellow() },
                { Cubelet.Blue(), Cubelet.Yellow(), Cubelet.Yellow() },
                { Cubelet.Red(), Cubelet.Yellow(), Cubelet.Yellow() }
                };

            var cube = new RubiksCube(new(3));

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
                    { Cubelet.Orange(), Cubelet.Orange(), Cubelet.White() },
                    { Cubelet.Yellow(), Cubelet.Green(), Cubelet.White() },
                    { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Blue() }
                };
            var expectedRightSide = new Cubelet[,]
                {
                    { Cubelet.Green(), Cubelet.Green(), Cubelet.Blue() },
                    { Cubelet.Red(), Cubelet.Red(), Cubelet.Blue() },
                    { Cubelet.Red(), Cubelet.Yellow(), Cubelet.Orange() }
                };
            var expectedBackSide = new Cubelet[,]
                {
                    { Cubelet.Red(), Cubelet.Blue(), Cubelet.White() },
                    { Cubelet.Red(), Cubelet.Blue(), Cubelet.White() },
                    { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.Green() }
                };
            var expectedLeftSide = new Cubelet[,]
                {
                    { Cubelet.Blue(), Cubelet.White(), Cubelet.White() },
                    { Cubelet.Blue(), Cubelet.Orange(), Cubelet.Orange() },
                    { Cubelet.Yellow(), Cubelet.Green(), Cubelet.Red() }
                };
            var expectedUpSide = new Cubelet[,]
                {
                    { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Yellow() },
                    { Cubelet.Green(), Cubelet.White(), Cubelet.Red() },
                    { Cubelet.Green(), Cubelet.White(), Cubelet.Red() }
                };
            var expectedDownSide = new Cubelet[,]
                {
                    { Cubelet.Green(), Cubelet.Green(), Cubelet.White() },
                    { Cubelet.Orange(), Cubelet.Yellow(), Cubelet.Red() },
                    { Cubelet.Orange(), Cubelet.Blue(), Cubelet.Blue() }
                };

            var cube = new RubiksCube(new(3));

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
                    { Cubelet.Orange(), Cubelet.Red(), Cubelet.Red() },
                    { Cubelet.Orange(), Cubelet.Green(), Cubelet.White() },
                    { Cubelet.White(), Cubelet.White(), Cubelet.White() }
                };
            var expectedRightSide = new Cubelet[,]
                {
                    { Cubelet.Yellow(), Cubelet.Blue(), Cubelet.Orange() },
                    { Cubelet.Red(), Cubelet.Red(), Cubelet.White() },
                    { Cubelet.Orange(), Cubelet.Yellow(), Cubelet.Red() }
                };
            var expectedBackSide = new Cubelet[,]
                {
                    { Cubelet.Yellow(), Cubelet.Blue(), Cubelet.White() },
                    { Cubelet.Orange(), Cubelet.Blue(), Cubelet.Yellow() },
                    { Cubelet.Yellow(), Cubelet.Yellow(), Cubelet.White() }
                };
            var expectedLeftSide = new Cubelet[,]
                {
                    { Cubelet.Green(), Cubelet.Yellow(), Cubelet.Yellow() },
                    { Cubelet.Orange(), Cubelet.Orange(), Cubelet.Green() },
                    { Cubelet.Blue(), Cubelet.Green(), Cubelet.Orange() }
                };
            var expectedUpSide = new Cubelet[,]
                {
                    { Cubelet.Red(), Cubelet.Orange(), Cubelet.Green() },
                    { Cubelet.Blue(), Cubelet.White(), Cubelet.White() },
                    { Cubelet.Blue(), Cubelet.Blue(), Cubelet.Blue() }
                };
            var expectedDownSide = new Cubelet[,]
                {
                    { Cubelet.Green(), Cubelet.Green(), Cubelet.Blue() },
                    { Cubelet.Red(), Cubelet.Yellow(), Cubelet.Red() },
                    { Cubelet.Red(), Cubelet.Green(), Cubelet.Green() }
                };

            var cube = new RubiksCube(new(3));

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
    }
}