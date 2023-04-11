using System;
using System.Linq;
using System.Threading.Tasks;

using Panda.RubikCube.Entities;
using Panda.RubikCube.Enums;

namespace Panda.RubikCube.Services
{
	public class ReverseMoveHistorySolver : IRubiksCubeSolver
	{
		public async Task<bool> Solve(RubiksCube rubiksCube, Func<Task>? moveExecuted)
		{
			var Solved = false;
			var solvedCube = new RubiksCube(new ReverseMoveHistorySolver(), rubiksCube.Settings);
			for (int i = rubiksCube.MoveHistory.Count - 1; i >= 0; i--)
			{
				Solved = rubiksCube.Faces.SequenceEqual(solvedCube.Faces);
				if (Solved) break;

				var reverseRotation = rubiksCube.MoveHistory[i].Rotation == Rotation.Clockwise ? Rotation.CounterClockwise : Rotation.Clockwise;
				rubiksCube.Execute(new CubeMoveRequest(rubiksCube.MoveHistory[i].Side, reverseRotation));
				if (moveExecuted != null)
					await moveExecuted.Invoke();

				Solved = rubiksCube.Faces.SequenceEqual(solvedCube.Faces);
			}
			return Solved;
		}
	}
}