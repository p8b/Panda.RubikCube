using Panda.RubikCube.Entities;
using Panda.RubikCube.Enums;
using Panda.RubikCube.Services;

namespace Panda.RubikCube.Tests
{
	public class ReverseMoveHistorySolverTests
	{
		[Fact]
		public async Task SolveMethod_Works_CorrectlyWithReverseMoveHistorySolver()
		{
			// Arrange
			var expectedCube = new RubiksCube(new ReverseMoveHistorySolver());
			var cube = new RubiksCube(new ReverseMoveHistorySolver());
			cube.Execute(FaceSide.Left, Rotation.CounterClockwise);

			// Act
			await cube.Solve(null);

			// Assert
			Assert.True(cube.Solved);
			Assert.True(cube.Faces.SequenceEqual(expectedCube.Faces));
		}
	}
}