using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Panda.RubikCube.Enums;
using Panda.RubikCube.Services;

namespace Panda.RubikCube.Entities
{
	public class RubiksCube
	{
		public bool Mixing { get; private set; }
		public bool Solving { get; private set; }
		public bool Solved { get; private set; } = true;
		public CubeFace[] Faces { get; } = new CubeFace[6];

		public List<CubeMoveRequest> MoveHistory { get; } = new List<CubeMoveRequest>();
		public CubeSettings Settings { get; }
		public IRubiksCubeSolver Solver { get; }

		/// <summary>
		///    Create a new Rubik cube.
		/// </summary>
		/// <param name="settings">If null default <see cref="CubeSettings"/> will be used.</param>
		public RubiksCube(IRubiksCubeSolver solver, CubeSettings? settings = null)
		{
			settings ??= new CubeSettings();

			Solver = solver;
			Settings = settings;

			foreach (FaceSide face in Enum.GetValues(typeof(FaceSide)))
			{
				Faces[(int)face] = new CubeFace(face, settings);
			}
		}

		public void Execute(FaceSide side, Rotation rotation)
			=> Execute(new CubeMoveRequest(side, rotation));

		public void Execute(CubeMoveRequest moveRequest)
		{
			Solved = false;
			var side = moveRequest.Side;
			// Copy and set the move request face as front.
			CubeFace[] rotatedCube = CloneCubeAndSetFrontAs(side);

			rotatedCube = RotateFrontFace(moveRequest, rotatedCube);

			// Put back the rotated cube.
			Parallel.ForEach(rotatedCube, item => Faces[(int)item.OriginalFace] = item);

			MoveHistory.Add(moveRequest);

			var solvedCube = new RubiksCube(Solver, Settings);
			Solved = Faces.SequenceEqual(solvedCube.Faces);
		}

		private CubeFace[] CloneCubeAndSetFrontAs(FaceSide side)
		{
			var rotatedCube = new CubeFace[6];

			rotatedCube[(int)FaceSide.Front] = Faces[(int)side];
			rotatedCube[(int)FaceSide.Up] = Faces[(int)side.GetUpFaceOf()];
			rotatedCube[(int)FaceSide.Left] = Faces[(int)side.GetLeftFaceOf()];
			rotatedCube[(int)FaceSide.Right] = Faces[(int)side.GetRightFaceOf()];
			rotatedCube[(int)FaceSide.Back] = Faces[(int)side.GetBackFaceOf()];
			rotatedCube[(int)FaceSide.Down] = Faces[(int)side.GetDownFaceOf()];
			return rotatedCube;
		}

		private CubeFace[] RotateFrontFace(CubeMoveRequest moveRequest, CubeFace[] cube)
		{
			var frontFace = cube[(int)FaceSide.Front];
			var rightFace = cube[(int)FaceSide.Right];
			var upFace = cube[(int)FaceSide.Up];
			//var backFace = cube[(int)FaceSide.Back];
			var leftFace = cube[(int)FaceSide.Left];
			var downFace = cube[(int)FaceSide.Down];

			// Correct the rotation of other faces if the front face changes from original side.
			if (moveRequest.Side == FaceSide.Left)
			{
				upFace.Rotate90(false);
				downFace.Rotate90(true);
			}
			else if (moveRequest.Side == FaceSide.Right)
			{
				upFace.Rotate90(true);
				downFace.Rotate90(false);
			}
			else if (moveRequest.Side == FaceSide.Back)
			{
				upFace.Rotate180();
				downFace.Rotate180();
			}
			else if (moveRequest.Side == FaceSide.Up)
			{
				rightFace.Rotate90(false);
				leftFace.Rotate90(true);
				upFace.Rotate180();
			}
			else if (moveRequest.Side == FaceSide.Down)
			{
				rightFace.Rotate90(true);
				leftFace.Rotate90(false);
				downFace.Rotate180();
			}

			// Set the rotation effect of current front face and other faces.
			frontFace.Rotate90(moveRequest.Rotation == Rotation.Clockwise);
			if (moveRequest.Rotation == Rotation.Clockwise)
			{
				Cubelet[] upRow = upFace.GetRow(Settings.Size - 1);

				upFace.SetRow(Settings.Size - 1, leftFace.GetColumn(Settings.Size - 1), true);

				leftFace.SetColumn(Settings.Size - 1, downFace.GetRow(0), true);

				downFace.SetRow(0, rightFace.GetColumn(0), true);

				rightFace.SetColumn(0, upRow, true);
			}
			else if (moveRequest.Rotation == Rotation.CounterClockwise)
			{
				Cubelet[] upRow = upFace.GetRow(Settings.Size - 1);

				upFace.SetRow(Settings.Size - 1, rightFace.GetColumn(0), false);

				rightFace.SetColumn(0, downFace.GetRow(0), false);

				downFace.SetRow(0, leftFace.GetColumn(Settings.Size - 1), false);

				leftFace.SetColumn(Settings.Size - 1, upRow, false);
			}

			// Reverse cube rotations;
			if (moveRequest.Side == FaceSide.Left)
			{
				upFace.Rotate90(true);
				downFace.Rotate90(false);
			}
			else if (moveRequest.Side == FaceSide.Right)
			{
				upFace.Rotate90(false);
				downFace.Rotate90(true);
			}
			else if (moveRequest.Side == FaceSide.Back)
			{
				upFace.Rotate180();
				downFace.Rotate180();
			}
			else if (moveRequest.Side == FaceSide.Up)
			{
				rightFace.Rotate90(true);
				leftFace.Rotate90(false);
				upFace.Rotate180();
			}
			else if (moveRequest.Side == FaceSide.Down)
			{
				rightFace.Rotate90(false);
				leftFace.Rotate90(true);
				downFace.Rotate180();
			}

			return cube;
		}

		public async Task Solve(Func<Task>? moveExecuted)
		{
			if (Solving || Mixing) return;
			Solving = true;
			Solved = await Solver.Solve(this, moveExecuted);
			Solving = false;
		}

		public async Task MixUp(int numberOfMoves, Func<Task>? moveExecuted)
		{
			if (Solving || Mixing)
				return;
			Mixing = true;
			var random = new Random();
			var faces = Enum.GetValues(typeof(FaceSide));
			var rotations = Enum.GetValues(typeof(Rotation));
			for (int i = 0; i < numberOfMoves; i++)
			{
				var face = (FaceSide)faces.GetValue(random.Next(faces.Length));
				var rotation = (Rotation)rotations.GetValue(random.Next(rotations.Length));
				Execute(face, rotation);
				if (moveExecuted != null)
					await moveExecuted.Invoke();
			}
			Mixing = false;
		}
	}
}