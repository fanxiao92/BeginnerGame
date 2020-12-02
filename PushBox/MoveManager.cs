using System;

namespace PushBox
{
    /// <summary>
    /// 移动管理器
    /// </summary>
    public class MoveManager
    {
        public MoveManager(Map map)
        {
            this.map = map;
        }
        
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="moveType"></param>
        public void ExecCmd(MoveType moveType)
        {
            Position deltaPosition = this.deltaPositions[(int)moveType];
            Position playerCurrLocation = this.map.GetPlayerLocation();
            Position playerNextLocation = playerCurrLocation + deltaPosition;
            this.map.MoveToTargetLocation(playerCurrLocation, playerNextLocation);
        }

        readonly Map map;

        readonly Position[] deltaPositions =
        {
            new Position(-1, 0),
            new Position(0, 1),
            new Position(0, -1),
            new Position(1, 0),
        };
    }
}