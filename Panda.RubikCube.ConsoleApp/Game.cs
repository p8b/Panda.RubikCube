using Panda.RubikCube.Entities;
using Panda.RubikCube.Enums;
using Panda.RubikCube.Services;

using Spectre.Console;

namespace Panda.RubikCube.ConsoleApp
{
	internal class Game
	{
		private bool _playing;
		private IRubiksCubeSolver _solver = new ReverseMoveHistorySolver();
		public bool ShowCoordinates { get; private set; }
		public int CubeSize { get; private set; } = 3;

		public void ShowDemo()
		{
			var cube = new RubiksCube(_solver, new(CubeSize));
			AnsiConsole.Live(new Table())
				.Start(ctx =>
				{
					AnsiConsole.Console.Clear(true);
					ctx.UpdateTarget(Display(cube, ShowCoordinates));
				});
		}

		public async Task PlayGame()
		{
			_playing = true;
			var cube = new RubiksCube(_solver, new(CubeSize));
			var moves = new List<CubeMoveRequest>();
			while (_playing)
			{
				// Render the layout
				AnsiConsole.Clear();
				AnsiConsole.WriteLine();
				AnsiConsole.Write(Display(cube, ShowCoordinates));
				var moveHistory = "";

				foreach (var move in moves)
				{
					moveHistory += move.GetShortString();
				}

				AnsiConsole.WriteLine(moveHistory);

				List<object> moveFaceOptions = Enum.GetValues<FaceSide>().Cast<object>().ToList();
				moveFaceOptions.Add("Mix Up");
				moveFaceOptions.Add("Solve");
				moveFaceOptions.Add("Exit");
				var moveFaceResult = AnsiConsole.Prompt(new SelectionPrompt<object>()
					.Title("What face would you like to rotate?")
					.PageSize(10)
					.MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
					.AddChoices(moveFaceOptions)
					.UseConverter(OptionConverter));

				if (moveFaceResult is string result)
				{
					if (result == "Exit")
					{
						_playing = false;
						AnsiConsole.Clear();
						break;
					}
					else if (result == "Mix Up")
					{
						await cube.MixUp(42, async () =>
						{
							await Task.Delay(200);
							AnsiConsole.Clear();
							AnsiConsole.WriteLine();
							AnsiConsole.Write(Display(cube, ShowCoordinates));
						});
						continue;
					}
					else if (result == "Solve")
					{
						await cube.Solve(async () =>
						{
							await Task.Delay(200);
							AnsiConsole.Clear();
							AnsiConsole.WriteLine();
							AnsiConsole.Write(Display(cube, ShowCoordinates));
						});
						continue;
					}
				}

				List<object> moveDirectionOptions = Enum.GetValues<Rotation>().Cast<object>().ToList();
				moveDirectionOptions.Add("Back");
				var moveDirectionResult = AnsiConsole.Prompt(new SelectionPrompt<object>()
					.Title("What face would you like to rotate?")
					.PageSize(10)
					.MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
					.AddChoices(moveDirectionOptions)
					.UseConverter(OptionConverter));

				if (moveFaceResult is FaceSide moveFace && moveDirectionResult is Rotation moveDirection)
				{
					var move = new CubeMoveRequest(moveFace, moveDirection);
					if (move is not null)
					{
						moves.Add(move);
						cube.Execute(move);
					}
				}
			}

			static string OptionConverter(object x)
			{
				if (x is FaceSide face)
					return face.GetDisplayName() ?? "error";
				else if (x is Rotation rotation)
					return rotation.GetDisplayName() ?? "error";
				else if (x is string)
					return x.ToString() ?? "error";
				return "error";
			}
		}

		public void ChangeRubikCubeSize()
		{
			CubeSize = AnsiConsole.Prompt(new SelectionPrompt<int>()
				.Title("Please Enter the cube size:")
				.PageSize(10)
				.MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
				.AddChoices(new int[] { 1, 2, 3 })
				.UseConverter(x => x.ToString()));
		}

		public void ToggleCoordinates()
		{
			ShowCoordinates = !ShowCoordinates;
		}

		private static Table Display(RubiksCube cube, bool showCoordinates)
		{
			var table = new Table();
			table.Border(TableBorder.None);
			table.Centered();
			table.HideHeaders();

			foreach (var index in Enumerable.Range(1, 4))
			{
				table.AddColumn(index.ToString(), options =>
				{
					options.NoWrap();
					options.Padding(0, 0);
				});
			}
			table.AddRow(new Markup(""), GetFaceTable(FaceSide.Up, cube, showCoordinates));
			table.AddRow(
				GetFaceTable(FaceSide.Left, cube, showCoordinates),
				GetFaceTable(FaceSide.Front, cube, showCoordinates),
				GetFaceTable(FaceSide.Right, cube, showCoordinates),
				GetFaceTable(FaceSide.Back, cube, showCoordinates));
			table.AddRow(new Markup(""), GetFaceTable(FaceSide.Down, cube, showCoordinates));

			return table;
		}

		private static Table GetFaceTable(FaceSide face, RubiksCube cube, bool showCoordinates)
		{
			var upTable = new Table();
			upTable.Border(TableBorder.None);
			upTable.Centered();
			upTable.HideHeaders();

			foreach (var index in Enumerable.Range(1, cube.Settings.Size))
			{
				upTable.AddColumn(index.ToString(), options =>
				{
					options.NoWrap();
					options.Padding(0, 0, 1, 0);
				});
			}

			for (int i = 0; i < cube.Settings.Size; i++)
			{
				var columns = new Text[cube.Settings.Size];
				var lineSpace = new string[cube.Settings.Size];
				for (int j = 0; j < cube.Settings.Size; j++)
				{
					var cubelet = cube.Faces[(int)face].Cubelets[i, j];
					Color? backgroundColour = cubelet.Colour switch
					{
						CubeletColour.Green => Color.Green1,
						CubeletColour.Blue => Color.DodgerBlue2,
						CubeletColour.Red => Color.Red1,
						CubeletColour.Orange => Color.DarkOrange3_1,
						CubeletColour.White => Color.White,
						CubeletColour.Yellow => Color.Yellow3_1,
						_ => null
					};
					var value = showCoordinates ? cubelet.OriginalPositon.row + "," + cubelet.OriginalPositon.column : " " + cubelet.Colour.GetDisplayName() + " ";
					columns[j] = new Text(value, new Style(Color.Black, backgroundColour, Decoration.Bold));
					lineSpace[j] = "";
				}
				upTable.AddRow(columns);
				upTable.AddRow(lineSpace);
			}

			return upTable;
		}
	}
}