using System.ComponentModel.DataAnnotations;

namespace Panda.RubikCube.Enums
{
    /// <summary>
    ///    Specifies the rotation direction.
    /// </summary>
    public enum Rotation
    {
        [Display(Name = "Clockwise")]
        Clockwise,
        [Display(Name = "Counter-clockwise")]
        CounterClockwise
    }
}