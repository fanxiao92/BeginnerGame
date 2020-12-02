using System;

namespace PushBox
{
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            const string MapInfo = @"10 8
11111111
12000001
13000001
10020001
10000001
10000001
10000001
10000001
10005001
11111111";
            var map = new Map();
            map.LoadMapInfo(MapInfo);
            var inputMgr = new InputManager();
            var moveMgr = new MoveManager(map);
            map.Render();
            
            while (true)
            {
                MoveType moveType = inputMgr.GetInput();
                moveMgr.ExecCmd(moveType);
                map.Render();
            }
        }
    }
}