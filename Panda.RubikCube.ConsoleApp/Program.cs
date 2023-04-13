using Panda.RubikCube.ConsoleApp;
using Panda.RubikCube.ConsoleApp.Enums;

using Spectre.Console;

Console.Title = "Rubik's Cube - P8B Panda";
var game = new Game();
var selectionOptions = new SelectionPrompt<AppOption>()
        .Title("Please select an option.")
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
        .AddChoices(Enum.GetValues<AppOption>())
        .UseConverter(x => x.GetDisplayName() ?? "error");
while (true)
{
    game.ShowDemo();

    switch (AnsiConsole.Prompt(selectionOptions))
    {
        case AppOption.PlayGame:
            await game.PlayGame();
            break;

        case AppOption.ToggleShowCoordinates:
            game.ToggleCoordinates();
            break;

        case AppOption.ChangeCubeSize:
            game.ChangeRubikCubeSize();
            break;

        case AppOption.Exit:
            Environment.Exit(0);
            break;
    }
}