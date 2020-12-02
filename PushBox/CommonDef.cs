namespace PushBox
{
    /// <summary>
    /// 移动类型
    /// </summary>
    public enum MoveType
    {
        Left,
        Down,
        Up,
        Right,
    };
    
    /// <summary>
    /// 坐标
    /// </summary>
    public class Position
    {
        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// 转换为一维数组表示
        /// </summary>
        /// <returns></returns>
        public int ToSingle(int width)
        {
            return Y * width + X;
        }
        
        public static Position operator +(Position lhs, Position rhs) => new Position(lhs.X + rhs.X, lhs.Y + rhs.Y);

        public int X { get; private set; }
        public int Y { get; private set; }
    }
}