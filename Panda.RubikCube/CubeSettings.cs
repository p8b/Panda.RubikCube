using System;

using Panda.RubikCube.Enums;

namespace Panda.RubikCube
{
    public class CubeSettings
    {
        public int Size { get; }
        public CubeletColour[] CubeletColours { get; } = new CubeletColour[6];

        public CubeSettings(
            int size = 3,
            CubeletColour front = CubeletColour.Green,
            CubeletColour right = CubeletColour.Red,
            CubeletColour back = CubeletColour.Blue,
            CubeletColour left = CubeletColour.Orange,
            CubeletColour up = CubeletColour.White,
            CubeletColour down = CubeletColour.Yellow)
        {
            if (size < 1 || size > 3)
                throw new ArgumentException("Size can only be 1, 2 or 3.");

            Size = size;
            CubeletColours[(int)FaceSide.Front] = front;
            CubeletColours[(int)FaceSide.Right] = right;
            CubeletColours[(int)FaceSide.Back] = back;
            CubeletColours[(int)FaceSide.Left] = left;
            CubeletColours[(int)FaceSide.Up] = up;
            CubeletColours[(int)FaceSide.Down] = down;
        }
    }
}