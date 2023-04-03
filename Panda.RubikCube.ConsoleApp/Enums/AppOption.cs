using System.ComponentModel.DataAnnotations;

namespace Panda.RubikCube.ConsoleApp.Enums
{
    public enum AppOption
    {
        [Display(Name = "Play")]
        PlayGame,
        [Display(Name = "Toggle Coordinates.")]
        ToggleShowCoordinates,
        [Display(Name = "Change Rubik's cube size.")]
        ChangeRubiksCubeSize,
        [Display(Name = "Exit.")]
        Exit,
    }
}