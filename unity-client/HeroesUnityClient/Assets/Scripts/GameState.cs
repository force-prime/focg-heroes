using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using static LogicEntities;

public class GameState : IGameState
{
    public List<LogicEntities.Hero> Heroes { get; private set; }
    public List<LogicEntities.MapObject> MapObjects { get; private set; }
    public LogicEntities.Tiles Tiles { get; private set; }

    public GameState(List<LogicEntities.Hero> heroes, List<LogicEntities.MapObject> objects, LogicEntities.Tiles tiles)
    {
        Heroes = heroes;
        MapObjects = objects;
        Tiles = tiles;
    }

    public LogicEntities.MapObject? GetMapObjectById(string objectId)
    {
        return MapObjects.Find(x => x.Id == objectId);  
    }

    public LogicEntities.Hero? GetHeroById(string heroId)
    {
        return Heroes.Find(x => x.Id == heroId); 
    }

    public LogicEntities.Tiles.Tile GetTileAtPosition(Vector2Int position)
    {
        return Tiles.Data[position.x, position.y];
    }

    public LogicEntities.MapObject? GetObjectAtPosition(Vector2Int position, IGameStatics statics) => CoreLogicHelper.GetObjectAtPosition(MapObjects, position, statics);

    public void RemoveObject(string objectId)
    {
        MapObjects.RemoveAll(x => x.Id == objectId);
    }
}

public interface IGameState
{
    LogicEntities.MapObject? GetMapObjectById(string objectId);
    LogicEntities.Hero? GetHeroById(string heroId);
    LogicEntities.Tiles.Tile GetTileAtPosition(Vector2Int position);
    LogicEntities.MapObject? GetObjectAtPosition(Vector2Int position, IGameStatics statics);

    void RemoveObject(string objectId);
}