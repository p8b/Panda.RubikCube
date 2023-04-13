using Panda.RubikCube.Entities;
using Panda.RubikCube.Enums;
using Panda.RubikCube.Services;

using Spectre.Console;
using Spectre.Console.Rendering;

namespace Panda.RubikCube.ConsoleApp;

internal class Game
{
    private readonly IRubiksCubeSolver _solver = new ReverseMoveHistorySolver();

    public bool ShowCoordinates { get; private set; }
    public int CubeSize { get; private set; } = 3;

    public void ShowDemo()
    {
        RenderCube(new RubiksCube(_solver, new(CubeSize)));
    }

    public async Task PlayGame()
    {
        var cube = new RubiksCube(_solver, new(CubeSize));

        var liveDisplay = AnsiConsole.Live(new Table());
        liveDisplay.AutoClear(true);

        var moveFaceOptions = Enum.GetValues<FaceSide>().Cast<object>().ToList();
        moveFaceOptions.Add("Mix Up");
        moveFaceOptions.Add("Solve");
        moveFaceOptions.Add("Exit Game");
        var moveFaceSelection = new SelectionPrompt<object>()
                .Title("What face would you like to rotate?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(moveFaceOptions)
                .UseConverter(OptionConverter);

        var moveDirectionOptions = Enum.GetValues<Rotation>().Cast<object>().ToList();
        moveDirectionOptions.Add("Back");
        var moveDirectionSelections = new SelectionPrompt<object>()
                .Title("What face would you like to rotate?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(moveDirectionOptions)
                .UseConverter(OptionConverter);

        while (true)
        {
            RenderCube(cube);

            var moveFaceResult = AnsiConsole.Prompt(moveFaceSelection);

            if (moveFaceResult is string result)
            {
                if (result == "Exit Game")
                {
                    return;
                }
                else if (result == "Mix Up")
                {
                    await cube.MixUp(42, async () =>
                    {
                        await Task.Delay(200);
                        RenderCube(cube);
                    });
                    continue;
                }
                else if (result == "Solve")
                {
                    await cube.Solve(async () =>
                    {
                        await Task.Delay(200);
                        RenderCube(cube);
                    });
                    continue;
                }
            }

            var moveDirectionResult = AnsiConsole.Prompt(moveDirectionSelections);

            if (moveFaceResult is FaceSide moveFace && moveDirectionResult is Rotation moveDirection)
            {
                var move = new CubeMoveRequest(moveFace, moveDirection);
                if (move is not null)
                    cube.Execute(move);
            }
        }

        static string OptionConverter(object x)
        {
            if (x is FaceSide face)
                return face.GetDisplayName() ?? "error";
            else if (x is Rotation rotation)
                return rotation.GetDisplayName() ?? "error";
            else if (x is string stringValue)
                return stringValue;
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

    private void RenderCube(RubiksCube cube)
    {
        var layoutTable = new Table()
            .Expand()
            .Centered()
            .HideHeaders()
            .AddColumn("0", options =>
            {
                options.NoWrap();
                options.Padding(0, 0);
                options.Width(20);
            })
            .AddColumn("1", options =>
            {
                options.NoWrap();
                options.Padding(0, 0);
                options.Width(10);
            });

        var cubeTable = new Table()
            .Border(TableBorder.None)
            .Centered()
            .HideHeaders();

        foreach (var index in Enumerable.Range(1, 4))
        {
            cubeTable.AddColumn(index.ToString(), options =>
            {
                options.NoWrap();
                options.Padding(0, 0);
            });
        }

        cubeTable.AddRow(new Markup(""), GetFaceTable(FaceSide.Up));
        cubeTable.AddRow(
            GetFaceTable(FaceSide.Left),
            GetFaceTable(FaceSide.Front),
            GetFaceTable(FaceSide.Right),
            GetFaceTable(FaceSide.Back));
        cubeTable.AddRow(new Markup(""), GetFaceTable(FaceSide.Down));

        var moveHistory = string.Concat(cube.MoveHistory.Select(x => x.GetShortString()));

        layoutTable.AddRow(cubeTable, new Markup(moveHistory));

        AnsiConsole.Live(new Table()).Start((ctx) =>
        {
            AnsiConsole.Console.Clear(true);
            ctx.UpdateTarget(layoutTable);
        });

        // Local method to get the renderable face
        IRenderable GetFaceTable(FaceSide face)
        {
            var faceTable = new Table()
                .Border(TableBorder.None)
                .Centered()
                .HideHeaders();

            foreach (var index in Enumerable.Range(1, cube.Settings.Size))
            {
                faceTable.AddColumn(index.ToString(), options =>
                {
                    options.NoWrap();
                    options.Padding(0, 0);
                });
            }

            var columns = new IRenderable[cube.Settings.Size];
            for (int rowIndex = 0; rowIndex < cube.Settings.Size; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < cube.Settings.Size; columnIndex++)
                {
                    var cubelet = cube.Faces[(int)face].Cubelets[rowIndex, columnIndex];
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

                    var displayValue = ShowCoordinates ?
                        $"{cubelet.OriginalPositon.row},{cubelet.OriginalPositon.column}" :
                        $" {cubelet.Colour.GetDisplayName()} ";

                    var text = new Text(displayValue, new Style(Color.Black, backgroundColour, Decoration.Bold));
                    columns[columnIndex] = new Padder(text).Padding(0, 0, 1, 1);
                }
                faceTable.AddRow(columns);
            }

            return new Padder(faceTable).Padding(0, 0, 1, 0);
        }
    }
}