using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MainMapObjectUI : MonoBehaviour, IMainMapObject
{
    public MainMap Map { get; private set; }

    [SerializeField] private MainMapObjectDescription Description;

    private string _id;

    public Vector2Int MinXY => GetMinXY();

    private void Awake()
    {
    }

    public void Initialize(MainMap map, string id)
    {
        Map = map;
        _id = id;
    }

    public List<MapCell> GetMyCells()
    {
        var l = new List<MapCell>();

        var b = GetBounds();
        for (int x = b.xMin; x <= b.xMax; x++)
            for (int y = b.yMin; y <= b.yMax; y++)
                l.Add(Map.GetMapCell(x, y));
        
        return l;
    }

    public RectInt GetBounds()
    {
        return new RectInt(MinXY, new Vector2Int(Description.Size.x - 1, Description.Size.y - 1));
    }

    private Vector2Int GetMinXY()
    {
        var pos = transform.localPosition;
        var width = Description.Size.x;
        var height = Description.Size.y;

        var mapPos = Map.TransformToMap(pos);
        return new Vector2Int((int)(mapPos.x - width / 2), (int)(mapPos.y - height / 2));
    }

    public void SetNewMinXYPosition(Vector2Int newPosition)
    {
        var z = transform.localPosition.z;
        var tPos = Map.MapToTransform(newPosition.x + 1.0f * Description.Size.x / 2, newPosition.y + 1.0f * Description.Size.y / 2);
        transform.localPosition = new Vector3(tPos.x, tPos.y, z);
    }

    public LogicEntities.MapObject GetState() => GameManager.Current.Game.State.GetMapObjectById(GetId());

    public string GetId() => _id;

    public IMainMapObjectDescription GetDescription() => Description;
}

