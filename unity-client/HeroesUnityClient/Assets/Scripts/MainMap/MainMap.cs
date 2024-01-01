using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;


public class MainMap : MonoBehaviour
{
    private static readonly Vector3 MAX_SCALE = new Vector3(1000, 1000, 1000);

    private static readonly int[] dx = { 0, 1, 0, -1 };
    private static readonly int[] dy = { 1, 0, -1, 0 };

    private MapCell[,] _cells;

    private BoundsInt _bounds;
    private int _width;
    private int _height;

    private readonly List<MainMapObjectUI> _mapObjects = new List<MainMapObjectUI>();
    private readonly List<MainMapHeroUI> _heroes = new List<MainMapHeroUI>();

    public IReadOnlyList<MainMapObjectUI> Objects => _mapObjects;
    public IReadOnlyList<MainMapHeroUI> Heroes => _heroes;

    public Vector2Int Size => new Vector2Int(_width, _height);

    private int _idCounter = 0;

    private void Awake()
    {
        InitFromUI();
    }

    void Start()
    {
        if (GameManager.Current.Game == null)
        {
            UnityEngine.Debug.LogError("No game found!");
            return;
        }

        if (GameManager.Current.Game.State != null)
            ApplyGameState(GameManager.Current.Game.State);

        GameManager.Current.Game.MessageCenter.Subscribe<LogicEvents.HeroMove>(OnHeroMove);
        GameManager.Current.Game.MessageCenter.Subscribe<LogicEvents.PickupResources>(OnPickupResources);
        GameManager.Current.Game.MessageCenter.Subscribe<LogicEvents.BattleEnd>(OnBattleEnd);
    }

    private void OnPickupResources(LogicEvents.PickupResources message)
    {
        var hero = GetHero(message.heroId);
        var mapObj = GetObject(message.objectId);

        DestroyMapObject(mapObj.gameObject);
        //mapObj.PickUp();
    }

    private void OnBattleEnd(LogicEvents.BattleEnd message)
    {
        var hero = GetHero(message.heroId);
        var mapObj = GetObject(message.objectId);

        DestroyMapObject(mapObj.gameObject);
        //mapObj.PickUp();
    }

    private void OnHeroMove(LogicEvents.HeroMove message)
    {
        var hero = GetHero(message.heroId);
        hero.ProcessMove(message.newPosition);
    }

    private MainMapObjectUI GetObject(string id)
    {
        return _mapObjects.Find(x => x.GetId() == id);
    }

    private MainMapHeroUI GetHero(string id)
    {
        return Heroes[0];
    }

    public void InitFromUI()
    {
        _idCounter = 0;

        _mapObjects.Clear();
        _heroes.Clear();

        SetupCellsFromGrid();

        foreach (var mapObject in grid.GetComponentsInChildren<MainMapObjectUI>())
            RegisterMapObject(mapObject.gameObject);

        FindMapObjectCells();
    }

    public void ApplyGameState(IGameState state)
    {
        _heroes[0].ProcessMove(state.GetHeroById(_heroes[0].GetState().Id).Position);

        List<MainMapObjectUI> toRemove = new List<MainMapObjectUI>();
        foreach (var obj in _mapObjects)
        {
            if (state.GetMapObjectById(obj.GetId()) == null)
                toRemove.Add(obj);
        }
        toRemove.ForEach(x => DestroyMapObject(x.gameObject));
    }

    public Grid grid
    {
        get
        {
            return gameObject.GetComponent<Grid>();
        }
    }

    public Vector2 TransformToMap(Vector3 transform)
    {
        return new Vector2(transform.x - _bounds.xMin, _height - transform.y + _bounds.yMin);
    }

    public Vector2 MapToTransform(double x, double y)
    {
        return new Vector2((float)(x + _bounds.xMin), (float)(_height - y + _bounds.yMin));
    }

    public MapCell? GetMapCell(double x, double y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            return _cells[(int)x, (int)y];
        }
        return null;
    }

    public LogicEntities.Tiles BuildTiles()
    {
        var t = new LogicEntities.Tiles();
        t.Data = new LogicEntities.Tiles.Tile[_width, _height];
        for (int x = 0; x < _width; x++)
            for (int y = 0; y < _height; y++)
            {
                var c = GetMapCell(x, y);
                t.Data[x, y] = c.TileData;
            }

        return t;
    }

    public bool IsInsideMap(Vector2Int position)
    {
        return (position.x >= 0 && position.y >= 0 && position.x < _width  && position.y < _height);
    }

    public bool IsEdgeCell(MapCell c)
    {
        return (c.X == 0 || c.Y == 0 || c.X == _width - 1 || c.Y == _height - 1);
    }

    public GameObject CreateMapObject(GameObject prefab)
    {
        var mapObjectInstance = Instantiate(prefab, grid.transform);
        RegisterMapObject(mapObjectInstance);

        return mapObjectInstance;
    }

    public void DestroyMapObject(GameObject instance)
    {
        UnregisterMapObject(instance);
        Destroy(instance);
    }

    private void RegisterMapObject(GameObject instance)
    {
        var mapObject = instance.GetComponent<MainMapObjectUI>();
        if (mapObject == null)
            return;

        mapObject.Initialize(this, (++_idCounter).ToString());

        var hero = instance.GetComponent<MainMapHeroUI>();
        
        if (hero == null)
            _mapObjects.Add(mapObject);
        else
            _heroes.Add(hero);
    }

    private void UnregisterMapObject(GameObject instance)
    {
        var mapObject = instance.GetComponent<MainMapObjectUI>();
        if (mapObject != null)
        {
            foreach (var c in mapObject.GetMyCells())
            {
                if (c.MapObject != mapObject)
                    Debug.LogError($"{c} incorrect map object at cell: " + c.MapObject);

                c.AttachMapObject(null);
            }
            _mapObjects.Remove(mapObject);
        }
    }

    private void SetupCellsFromGrid()
    {
        var g = grid.transform.Find("DataTiles").gameObject;
        var tileMap = g.GetComponentInChildren<Tilemap>();
        Debug.Log("Creating Map " + tileMap.cellBounds);

        _bounds = tileMap.cellBounds;

        _height = tileMap.size.y;
        _width = tileMap.size.x;

        var description = GetComponent<TilesDescription>();

        _cells = new MapCell[_width, _height];
        for (var x = 0; x < _width; x++)
        {
            for (var y = _height - 1; y >= 0; y--)
            {
                var pos = new Vector3Int(x + _bounds.xMin, y + _bounds.yMin, 0);
                var tile = tileMap.GetTile(pos);

                var cellX = x;
                var cellY = _height - y - 1;

                _cells[cellX, cellY] = new MapCell();
                _cells[cellX, cellY].Setup(MapCell.FromTile(tile, description), cellX, cellY);
                // Debug.Log("CELL " + x + " " + y + tile.name);
            }
        }
    }

    private void ClearAttachedMapObjects()
    {
        foreach (MapCell c in _cells)
        {
            c.AttachMapObject(null);
        }
    }

    private void FindMapObjectCells()
    {
        ClearAttachedMapObjects();

        foreach (MainMapObjectUI b in _mapObjects)
        {
            foreach (var c in b.GetMyCells())
            {
                if (c.MapObject != null)
                    Debug.LogError($"{c} already has map object: " + c.MapObject);

                c.AttachMapObject(b);
            }
        }
    }
}
