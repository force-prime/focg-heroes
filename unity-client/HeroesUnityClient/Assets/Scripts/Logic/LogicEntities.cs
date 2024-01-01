using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

static public class LogicEntities
{
    public class Hero
    {
        public string Id { get; set; }

        public Vector2Int Position { get; set; }

        public Resources Resources { get; set; } = new Resources();
        public Army Army { get; set; } = new Army();
    }

    public class MapObject
    {
        public string Id { get; set; }

        public Vector2Int Position { get; set; }

        public string DescriptionId { get; set; }
    }

    public class Tiles
    {
        public class Tile
        {
            public Tile(CellType type)
            {
                Type = type;
            }

            public enum CellType { Road, Grass, Obstacle, Undefined };

            public CellType Type { get; set; }

            public bool IsWalkable() => Type switch
            {
                CellType.Road or CellType.Grass => true,
                _ => false
            };
        }

        public Tile[,] Data;

        public int Width => Data.GetLength(0);
        public int Height => Data.GetLength(1); 
    }

    public class Resources : EntityAndCount<MapResourceEnum>
    {

    }

    public class Army : EntityAndCount<string>
    {

    }
}
