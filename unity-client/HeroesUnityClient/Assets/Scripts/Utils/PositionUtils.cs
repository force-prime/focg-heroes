using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

static public class PositionUtils
{
    static public bool IsAdjacent(this Vector2Int p, Vector2Int q)
    {
        return Math.Abs(p.x - q.x) + Math.Abs(p.y - q.y) == 1;
    }

    static public Vector2Int GetNext(this Vector2Int p, Direction d) => d switch
    {
        Direction.Up => p += Vector2Int.down,
        Direction.Down => p += Vector2Int.up,
        Direction.Left => p += Vector2Int.left,
        Direction.Right => p += Vector2Int.right,
        _ => throw new NotSupportedException()
    };
}
