using System.ComponentModel.DataAnnotations;

namespace Panda.RubikCube.Enums
{
    /// <summary>
    ///    Specifies the cubelet colour.
    /// </summary>
    public enum CubeletColour : byte
    {
        [Display(Name = "G")]
        Green,
        [Display(Name = "B")]
        Blue,
        [Display(Name = "R")]
        Red,
        [Display(Name = "O")]
        Orange,
        [Display(Name = "W")]
        White,
        [Display(Name = "Y")]
        Yellow,
    }
}