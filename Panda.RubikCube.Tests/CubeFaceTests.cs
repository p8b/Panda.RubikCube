using Panda.RubikCube.Entities;
using Panda.RubikCube.Enums;

namespace Panda.RubikCube.Tests
{
    public class CubeFaceTests
    {
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
        public void SetRow_SetsRowCorrectly_Clockwise()
        {
            // Arrange
            const int faceSize = 3;
            var face = new CubeFace(FaceSide.Front, CubeletColour.Blue, faceSize);
            var rowToSet = new[] {
                Cubelet.Green((0, 0)),
                Cubelet.Red((0, 1)),
                Cubelet.White((0, 2))
            };
            var expectedCubelets = new[,]
            {
                { Cubelet.White((0, 0)), Cubelet.Red((0, 1)), Cubelet.Green((0, 2)) },
                { Cubelet.Blue((1, 0)), Cubelet.Blue((1, 1)), Cubelet.Blue((1, 2)) },
                { Cubelet.Blue((2, 0)), Cubelet.Blue((2, 1)), Cubelet.Blue((2, 2)) }
            };

            // Act
            face.SetRow(0, rowToSet, true);

            // Assert
            Assert.Equal(expectedCubelets, face.Cubelets);
        }

        [Fact]
        public void SetRow_SetsRowCorrectly_CounterClockwise()
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
                });
            var expectedColumn = new[] { Cubelet.Red((0, 1)), Cubelet.White((1, 1)), Cubelet.Blue((2, 1)) };

            // Act
            var actualColumn = face.GetColumn(1);

            // Assert
            Assert.Equal(expectedColumn, actualColumn);
        }

        [Fact]
        public void SetColumn_SetsColumnCorrectly_Clockwise()
        {
            // Arrange
            var face = new CubeFace(FaceSide.Front, new Cubelet[,]
                {
                    { Cubelet.Blue((0, 0)), Cubelet.Blue((0, 1)) },
                    { Cubelet.Blue((1, 0)), Cubelet.Blue((1, 1)) },
                });
            var columnToSet = new Cubelet[] { Cubelet.Red((0, 0)), Cubelet.White((0, 1)) };
            var expectedCubelets = new[,]
            {
                { Cubelet.Red((0, 0)), Cubelet.Blue((0, 1)) },
                { Cubelet.White((1, 0)), Cubelet.Blue((1, 1)) }
            };

            // Act
            face.SetColumn(0, columnToSet, true);

            // Assert
            Assert.Equal(expectedCubelets, face.Cubelets);
        }

        [Fact]
        public void SetColumn_SetsColumnCorrectly_CounterClockwise()
        {
            // Arrange
            var face = new CubeFace(FaceSide.Front, new Cubelet[,]
                {
                    { Cubelet.Blue((0, 0)), Cubelet.Blue((0, 1)) },
                    { Cubelet.Blue((1, 0)), Cubelet.Blue((1, 1)) },
                });
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
            var face = new CubeFace(FaceSide.Front, new[,]
            {
                { Cubelet.Green((0, 0)), Cubelet.White((0, 1)), Cubelet.White((0, 2)) },
                { Cubelet.White((1, 0)), Cubelet.White((1, 1)), Cubelet.White((1, 2)) },
                { Cubelet.White((2, 0)), Cubelet.White((2, 1)), Cubelet.White((2, 2)) },
            });

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
            var face = new CubeFace(FaceSide.Front, new[,]
            {
                {Cubelet.Green((0, 0)), Cubelet.Red((0, 1)), Cubelet.Red((0, 2))},
                {Cubelet.Red((1, 0)), Cubelet.Red((1, 1)), Cubelet.Red((1, 2))},
                {Cubelet.Red((2, 0)), Cubelet.Red((2, 1)), Cubelet.Red((2, 2))},
            });

            // Act
            face.Rotate90(false);

            // Assert
            var expectedFace = new CubeFace(FaceSide.Front, new[,]
            {
                {Cubelet.Red((0, 2)), Cubelet.Red((1, 2)), Cubelet.Red((2, 2))},
                {Cubelet.Red((0, 1)), Cubelet.Red((1, 1)), Cubelet.Red((2, 1))},
                {Cubelet.Green((0, 0)), Cubelet.Red((1, 0)), Cubelet.Red((2, 0))},
            });

            Assert.Equal(expectedFace.Cubelets, face.Cubelets);
        }

        [Fact]
        public void Rotate180_Clockwise_RotatesCorrectly()
        {
            // Arrange
            var face = new CubeFace(FaceSide.Front, new[,]
            {
                { Cubelet.Green((0, 0)), Cubelet.White((0, 1)), Cubelet.White((0, 2)) },
                { Cubelet.White((1, 0)), Cubelet.White((1, 1)), Cubelet.White((1, 2)) },
                { Cubelet.White((2, 0)), Cubelet.White((2, 1)), Cubelet.White((2, 2)) },
            });

            var expected = new Cubelet[,]
            {
                { Cubelet.White((2, 2)), Cubelet.White((1, 2)), Cubelet.White((0, 2)) },
                { Cubelet.White((2, 1)), Cubelet.White((1, 1)), Cubelet.White((0, 1)) },
                { Cubelet.White((2, 0)), Cubelet.White((1, 0)), Cubelet.Green((0, 0)) },
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
            var face = new CubeFace(FaceSide.Front, new[,]
            {
                {Cubelet.Green((0, 0)), Cubelet.Red((0, 1)), Cubelet.Red((0, 2))},
                {Cubelet.Red((1, 0)), Cubelet.Red((1, 1)), Cubelet.Red((1, 2))},
                {Cubelet.Red((2, 0)), Cubelet.Red((2, 1)), Cubelet.Red((2, 2))},
            });
            var expectedFace = new CubeFace(FaceSide.Front, new[,]
            {
                {Cubelet.Red((2, 2)), Cubelet.Red((2, 1)), Cubelet.Red((2, 0))},
                {Cubelet.Red((1, 2)), Cubelet.Red((1, 1)), Cubelet.Red((1, 0))},
                {Cubelet.Red((0, 2)), Cubelet.Red((0, 1)), Cubelet.Green((0, 0))},
            });

            // Act
            face.Rotate180();

            // Assert

            Assert.Equal(expectedFace.Cubelets, face.Cubelets);
        }

        [Fact]
        public void CubeFace_ConstructedWithFace_OriginalFaceIsSetCorrectly()
        {
            // Arrange
            var face = FaceSide.Front;
            var cubelets = new Cubelet[3, 3];

            // Act
            var cubeFace = new CubeFace(face, cubelets);

            // Assert
            Assert.Equal(face, cubeFace.OriginalFace);
        }
    }
}