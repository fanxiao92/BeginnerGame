using System;

namespace PushBox
{
    public class Map
    {
        public void Render()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    RenderGrid(this.grids[i, j]);
                }
                Console.Out.WriteLine();
            }
        }

        static void RenderGrid(GridType gridType)
        {
            switch (gridType)
            {
                case GridType.Wall:
                    Console.Out.Write("#");
                    break;
                case GridType.Box:
                    Console.Out.Write("*");
                    break;
                case GridType.Player:
                    Console.Out.Write("!");
                    break;
                case GridType.Empty:
                    Console.Out.Write(" ");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gridType), gridType, null);
            } 
        }

        const int Width = 4;
        const int Height = 4;

        enum GridType
        {
            Wall,
            Box,
            Player,
            Empty,
        }

        readonly GridType[,] grids = new GridType[Height, Width]
        {
            {GridType.Wall, GridType.Wall, GridType.Wall, GridType.Wall},
            {GridType.Wall, GridType.Empty, GridType.Box, GridType.Wall},
            {GridType.Wall, GridType.Empty, GridType.Player, GridType.Wall},
            {GridType.Wall, GridType.Wall, GridType.Wall, GridType.Wall},
        };
    }
}