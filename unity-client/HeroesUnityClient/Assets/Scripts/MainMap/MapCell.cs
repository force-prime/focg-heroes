using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MapCell
{
    public LogicEntities.Tiles.Tile TileData { get; private set; }

    public int X { get; private set; }
    public int Y { get; private set; }

    public MainMapObjectUI MapObject { get; private set; }


    static public LogicEntities.Tiles.Tile? FromTile(UnityEngine.Tilemaps.TileBase tile, TilesDescription description)
    {
        if (tile == null)
            return null;

        var cellType = LogicEntities.Tiles.Tile.CellType.Undefined;

        if (description.Walls.Contains(tile))
            cellType = LogicEntities.Tiles.Tile.CellType.Obstacle;
        else if (description.Roads.Contains(tile))
            cellType = LogicEntities.Tiles.Tile.CellType.Road;
        else
            cellType = LogicEntities.Tiles.Tile.CellType.Grass;

        return new LogicEntities.Tiles.Tile(cellType);
    }

    public Vector2 GetCenter()
    {
        return new Vector2(X + 0.5f, Y + 0.5f);
    }

    public bool ContainsPoint(float x, float y)
    {
        return (X <= x && x <= (X + 1) && Y <= y && y <= (Y + 1));
    }

    public void AttachMapObject(MainMapObjectUI mapObject)
    {
        MapObject = mapObject;
    }

    public void Setup(LogicEntities.Tiles.Tile tileData, int x, int y)
    {
        TileData = tileData;
        X = x;
        Y = y;
    }
}
