using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniSand.Utils
{
    public static class Directions
    {
        public static readonly Vector2Int Up = new Vector2Int(0, 1);
        public static readonly Vector2Int Down = new Vector2Int(0, -1);
        public static readonly Vector2Int Left = new Vector2Int(-1, 0);
        public static readonly Vector2Int Right = new Vector2Int(1, 0);
        public static readonly Vector2Int UpLeft = new Vector2Int(-1, 1);
        public static readonly Vector2Int UpRight = new Vector2Int(1, 1);
        public static readonly Vector2Int DownLeft = new Vector2Int(-1, -1);
        public static readonly Vector2Int DownRight = new Vector2Int(1, -1);
        
        public static readonly Vector2Int[] All = {Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight};
        
        public static Vector2Int RandomDirection()
        {
            return All[Random.Range(0, All.Length)];
        }
        
        public static Vector2Int RandomCardinalDirection()
        {
            return All[Random.Range(0, 4)];
        }
        
        public static Vector2Int RandomDiagonalDirection()
        {
            return All[Random.Range(4, 8)];
        }
        
        public static Vector2Int GetDirectionVector2Int(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Up,
                Direction.Down => Down,
                Direction.Left => Left,
                Direction.Right => Right,
                Direction.UpLeft => UpLeft,
                Direction.UpRight => UpRight,
                Direction.DownLeft => DownLeft,
                Direction.DownRight => DownRight,
                _ => Vector2Int.zero
            };
        }
        

    }
    
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }
}
