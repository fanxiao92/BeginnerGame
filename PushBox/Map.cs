using System;

namespace PushBox
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public class Map
    {
        /// <summary>
        /// 加载地图信息
        /// </summary>
        public void LoadMapInfo(string mapInfo)
        {
            string[] lines = mapInfo.Split(new[]{Environment.NewLine}, StringSplitOptions.None);
            this.LoadMapMetaInfo(lines[0]);
            this.LoadMapRealInfo(lines[1..]);
        }
        
        /// <summary>
        /// 加载地图 meta 信息
        /// </summary>
        /// <param name="metaInfo"></param>
        void LoadMapMetaInfo(string metaInfo)
        {
            string[] infos = metaInfo.Split(' ');
            this.height = int.Parse(infos[0]);
            this.width = int.Parse(infos[1]);
            this.grids = new int [this.height * this.width];
        }

        /// <summary>
        /// 加载地图真实信息
        /// </summary>
        /// <param name="realLineInfo"></param>
        void LoadMapRealInfo(IReadOnlyList<string> realLineInfo)
        {
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    this.grids[y * this.width + x] = (int)char.GetNumericValue(realLineInfo[y][x]);
                }
            }
        }
        
        /// <summary>
        /// 渲染地图
        /// </summary>
        public void Render()
        {
            for (int y = 0; y < this.height; y++)
            {    
                for (int x = 0; x < this.width; x++)
                {
                    Console.Out.Write(MapObjectGraphics[this.grids[y * this.width + x]]);
                }
                Console.Out.WriteLine();
            }
        }
        
        /// <summary>
        /// 获取玩家的位置
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Position GetPlayerLocation()
        {
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    int mapObjectType = this.grids[y * this.width + x];
                    if (mapObjectType == (int)MapObjectType.Player || mapObjectType == (int)MapObjectType.PlayerOnGoal)
                    {
                        return new Position(x, y);
                    }
                }
            }

            throw new ArgumentOutOfRangeException("Grids","Cant Find Player");
        }

        /// <summary>
        /// 是否为墙
        /// </summary>
        /// <param name="location"></param>
        public bool IsWall(Position location)
        {
            return this.grids[location.ToSingle(this.width)] == (int)MapObjectType.Wall;
        }

        /// <summary>
        /// 是否空地
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool IsSpace(Position location)
        {
            return this.grids[location.ToSingle(this.width)] == (int)MapObjectType.Space;
        }

        /// <summary>
        /// 移动到目标位置
        /// </summary>
        /// <param name="currLocation"></param>
        /// <param name="nextLocation"></param>
        /// <param name="nextDirectLocation"></param>
        /// <returns></returns>
        public void MoveToTargetLocation(Position currLocation, Position nextLocation, Position nextDirectLocation)
        {
            var grid = (MapObjectType)this.grids[nextLocation.ToSingle(this.width)];
            switch (grid)
            {
                case MapObjectType.Space:
                    this.MoveToSpaceLocation(currLocation, nextLocation);
                    break;
                case MapObjectType.Wall:
                    this.MoveToWallLocation(currLocation, nextLocation);
                    break;
                case MapObjectType.Goal:
                    this.MoveToGoalLocation(currLocation, nextLocation);
                    break;
                case MapObjectType.Box:
                    this.MoveToBoxLocation(currLocation, nextLocation, nextDirectLocation);
                    break;
                case MapObjectType.BoxOnGoal:
                    this.MoveToBoxOnGoalLocation(currLocation, nextLocation, nextDirectLocation);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 移动到空地位置
        /// </summary>
        /// <param name="currLocation"></param>
        /// <param name="nextLocation"></param>
        void MoveToSpaceLocation(Position currLocation, Position nextLocation)
        {
            int currIdx = currLocation.ToSingle(this.width);
            int currGrid = this.grids[currIdx];
            Debug.Assert(currGrid == (int)MapObjectType.Player || currGrid == (int)MapObjectType.PlayerOnGoal);

            int nextIdx = nextLocation.ToSingle(this.width);
            int nextGrid = this.grids[nextIdx];
            Debug.Assert(nextGrid == (int)MapObjectType.Space);
            
            if (currGrid == (int)MapObjectType.PlayerOnGoal)
            {
                this.grids[currIdx] = (int)MapObjectType.Goal;
            }
            else
            {
                this.grids[currIdx] = (int)MapObjectType.Space;
            }

            this.grids[nextIdx] = (int)MapObjectType.Player;
        }
        
        /// <summary>
        /// 移动到墙位置
        /// </summary>
        /// <param name="currLocation"></param>
        /// <param name="nextLocation"></param>
        void MoveToWallLocation(Position currLocation, Position nextLocation)
        {
            int currIdx = currLocation.ToSingle(this.width);
            int currGrid = this.grids[currIdx];
            Debug.Assert(currGrid == (int)MapObjectType.Player || currGrid == (int)MapObjectType.PlayerOnGoal);

            int nextIdx = nextLocation.ToSingle(this.width);
            int nextGrid = this.grids[nextIdx];
            Debug.Assert(nextGrid == (int)MapObjectType.Wall);
        }
        
        /// <summary>
        /// 移动到墙位置
        /// </summary>
        /// <param name="currLocation"></param>
        /// <param name="nextLocation"></param>
        void MoveToGoalLocation(Position currLocation, Position nextLocation)
        {
            int currIdx = currLocation.ToSingle(this.width);
            int currGrid = this.grids[currIdx];
            Debug.Assert(currGrid == (int)MapObjectType.Player || currGrid == (int)MapObjectType.PlayerOnGoal);

            int nextIdx = nextLocation.ToSingle(this.width);
            int nextGrid = this.grids[nextIdx];
            Debug.Assert(nextGrid == (int)MapObjectType.Goal);
            
            if (currGrid == (int)MapObjectType.PlayerOnGoal)
            {
                this.grids[currIdx] = (int)MapObjectType.Goal;
            }
            else
            {
                this.grids[currIdx] = (int)MapObjectType.Space;
            }
            this.grids[nextIdx] = (int)MapObjectType.PlayerOnGoal;
        }

        /// <summary>
        /// 移动到墙位置
        /// </summary>
        /// <param name="currLocation"></param>
        /// <param name="nextLocation"></param>
        /// <param name="nextDirectLocation"></param>
        void MoveToBoxLocation(Position currLocation, Position nextLocation, Position nextDirectLocation)
        {
            int currIdx = currLocation.ToSingle(this.width);
            int currGrid = this.grids[currIdx];
            Debug.Assert(currGrid == (int)MapObjectType.Player || currGrid == (int)MapObjectType.PlayerOnGoal);

            int nextIdx = nextLocation.ToSingle(this.width);
            int nextGrid = this.grids[nextIdx];
            Debug.Assert(nextGrid == (int)MapObjectType.Box);

            int nextDirectIdx = nextDirectLocation.ToSingle(this.width);
            int nextDirectGrid = this.grids[nextDirectIdx];
            if (nextDirectGrid == (int)MapObjectType.Box ||
                nextDirectGrid == (int)MapObjectType.BoxOnGoal ||
                nextDirectGrid == (int)MapObjectType.Wall)
            {
                return;
            }
            
            if (currGrid == (int)MapObjectType.PlayerOnGoal)
            {
                this.grids[currIdx] = (int)MapObjectType.Goal;
            }
            else
            {
                this.grids[currIdx] = (int)MapObjectType.Space;
            }
            this.grids[nextIdx] = (int)MapObjectType.Player;
            
            if (nextDirectGrid == (int)MapObjectType.Goal)
            {
                this.grids[nextDirectIdx] = (int)MapObjectType.BoxOnGoal;
            }
            else
            {
                this.grids[nextDirectIdx] = (int)MapObjectType.Box;
            }
        }
        
        /// <summary>
        /// 移动到墙位置
        /// </summary>
        /// <param name="currLocation"></param>
        /// <param name="nextLocation"></param>
        /// <param name="nextDirectLocation"></param>
        void MoveToBoxOnGoalLocation(Position currLocation, Position nextLocation, Position nextDirectLocation)
        {
            int currIdx = currLocation.ToSingle(this.width);
            int currGrid = this.grids[currIdx];
            Debug.Assert(currGrid == (int)MapObjectType.Player || currGrid == (int)MapObjectType.PlayerOnGoal);

            int nextIdx = nextLocation.ToSingle(this.width);
            int nextGrid = this.grids[nextIdx];
            Debug.Assert(nextGrid == (int)MapObjectType.BoxOnGoal);
            
            int nextDirectIdx = nextDirectLocation.ToSingle(this.width);
            int nextDirectGrid = this.grids[nextDirectIdx];
            if (nextDirectGrid == (int)MapObjectType.Box ||
                nextDirectGrid == (int)MapObjectType.BoxOnGoal ||
                nextDirectGrid == (int)MapObjectType.Wall)
            {
                return;
            }
            
            if (currGrid == (int)MapObjectType.PlayerOnGoal)
            {
                this.grids[currIdx] = (int)MapObjectType.Goal;
            }
            else
            {
                this.grids[currIdx] = (int)MapObjectType.Space;
            }
            this.grids[nextIdx] = (int)MapObjectType.PlayerOnGoal;

            if (nextDirectGrid == (int)MapObjectType.Goal)
            {
                this.grids[nextDirectIdx] = (int)MapObjectType.BoxOnGoal;
            }
            else
            {
                this.grids[nextDirectIdx] = (int)MapObjectType.Box;
            }
        }
        
        int height;
        int width;
        int[] grids;

        /// <summary>
        /// 地图对象类型
        /// </summary>
        enum MapObjectType
        {
            Space = 0,
            Wall = 1,
            Goal = 2,
            Box = 3,
            BoxOnGoal = 4,
            Player = 5,
            PlayerOnGoal = 6,
        }

        static readonly char[] MapObjectGraphics = {' ', '#', '.', 'o', 'O', 'p', 'P' };
    }
}