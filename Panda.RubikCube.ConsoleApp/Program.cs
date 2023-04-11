using Panda.RubikCube.ConsoleApp;
using Panda.RubikCube.ConsoleApp.Enums;

using Spectre.Console;

var appRunning = true;
var game = new Game();
while (appRunning)
{
	game.ShowDemo();

	var selectedOption = AnsiConsole.Prompt(new SelectionPrompt<AppOption>()
		.Title("Please select an option.")
		.PageSize(10)
		.MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
		.AddChoices(Enum.GetValues<AppOption>())
		.UseConverter(x => x.GetDisplayName() ?? "error"));

	switch (selectedOption)
	{
		case AppOption.PlayGame:
			await game.PlayGame();
			break;

		case AppOption.ToggleShowCoordinates:
			game.ToggleCoordinates();
			break;

		case AppOption.ChangeRubiksCubeSize:
			game.ChangeRubikCubeSize();
			break;

		case AppOption.Exit:
			appRunning = false;
			break;
	}
}