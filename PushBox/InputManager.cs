using System;

namespace PushBox
{
    using System.Collections.Generic;

    /// <summary>
    /// 输入管理器
    /// </summary>
    public class InputManager
    {
        public MoveType GetInput()
        {
            Console.Out.WriteLine("a:left w:up s:down d:right");
            char inputChar = Console.ReadKey().KeyChar;
            if (!this.keyBoard2MoveType.TryGetValue(inputChar, out MoveType moveType))
            {
                throw new ArgumentException($"input not expection:{inputChar}");
            }
            Console.Out.WriteLine();
            return moveType;
        }

        readonly Dictionary<char, MoveType> keyBoard2MoveType = new Dictionary<char, MoveType>
        {
            {'a', MoveType.Left},
            {'w', MoveType.Up},
            {'s', MoveType.Down},
            {'d', MoveType.Right},
        };
    }
}