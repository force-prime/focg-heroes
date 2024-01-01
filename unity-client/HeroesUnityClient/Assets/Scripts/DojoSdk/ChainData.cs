using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ChainData
{
    static public List<string> PrepareMapUploadData(GameState state, IGameStatics statics)
    {
        List<string> result = new List<string>();
        var tiles = state.Tiles;
        result.Add((tiles.Width * tiles.Height).ToHexString());
        for (int x = 0; x < tiles.Width; x++)
            for (int y = 0; y < tiles.Height; y++)
            {
                var tile = tiles.Data[x, y];

                result.Add(x.ToHexString());
                result.Add(y.ToHexString());

                if (tile != null)
                {
                    result.Add(((int)tile.Type).ToHexString());

                    var obj = CoreLogicHelper.GetObjectAtPosition(state.MapObjects, new UnityEngine.Vector2Int(x, y), statics);
                    result.Add((obj != null ? int.Parse(obj.Id) : 0).ToHexString());
                } else
                {
                    result.Add(0.ToHexString());
                    result.Add(0.ToHexString());
                }
            }

        result.Add(state.MapObjects.Count.ToHexString());
        foreach (var o in state.MapObjects)
        {
            result.Add(int.Parse(o.Id).ToHexString());
            result.Add(o.Position.x.ToHexString());
            result.Add(o.Position.y.ToHexString());
            result.Add(int.Parse(o.DescriptionId).ToHexString());
        }

        var heroPosition = state.Heroes[0].Position;
        result.Add(heroPosition.x.ToHexString());
        result.Add(heroPosition.y.ToHexString());

        return result;
    }

    static public List<string> PrepareStaticsUploadData(GameState state, IGameStatics statics)
    {
        List<string> result = new List<string>();

        var descriptions = state.MapObjects.Select(x => x.DescriptionId).Distinct().Select(x => statics.GetDescription(x)).ToList();
        var resources = descriptions.Where(x => x is IResourcePileDescription).Cast<IResourcePileDescription>().ToList();
        var unitShops = descriptions.Where(x => x is IUnitShopDescription).Cast<IUnitShopDescription>().ToList();
        var armies = descriptions.Where(x => x is IArmyDescription).Cast<IArmyDescription>().ToList();

        result.Add(resources.Count.ToHexString());
        foreach (var r in resources)
        {
            result.Add(int.Parse(r.Id).ToHexString());
            result.Add(((int) (MapResourceEnum.Gold)).ToHexString());
            result.Add(r.GetResources().Get(MapResourceEnum.Gold).ToHexString());
        }

        result.Add(unitShops.Count.ToHexString());
        foreach (var s in unitShops)
        {
            result.Add(int.Parse(s.Id).ToHexString());
            result.Add(int.Parse(s.UnitId).ToHexString());
            result.Add(s.GetCost().Get(MapResourceEnum.Gold).ToHexString());
        }

        result.Add(armies.Count.ToHexString());
        foreach (var a in armies)
        {
            result.Add(int.Parse(a.Id).ToHexString());

            for (int i = 0; i < 10; i++)
                result.Add(0.ToHexString());
        }

        return result;
    }
}

static public class ChainDataExtensions
{
    static public string ToHexString(this int s) => string.Format("0x{0:X}", s);
}
