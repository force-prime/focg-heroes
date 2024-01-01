using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;


static public class GameDataHelpers
{
    static public GameState GameStateFromMainMap(MainMap map)
    {
        map.InitFromUI();

        var objects = map.Objects.Select((x, index) => new LogicEntities.MapObject { Id = x.GetId(), DescriptionId = x.GetDescription().Id, Position = x.MinXY }).ToList();
        var tiles = map.BuildTiles();
        var heroes = map.Heroes.Select((x, index) => new LogicEntities.Hero
        {
            Id = x.GetComponent<MainMapObjectUI>().GetId(),
            Position = x.GetComponent<MainMapObjectUI>().MinXY,
            Army = new LogicEntities.Army(),
            Resources = new LogicEntities.Resources()
        }).ToList();

        var state = new GameState(heroes, objects, tiles);
        return state;
    }

#if UNITY_EDITOR
    public class EditorStatics : IGameStatics
    {
        private Dictionary<string, UnitDescription> _units;
        private Dictionary<string, MainMapObjectDescription> _objects;

        public EditorStatics()
        {
            _units = AssetDatabaseExtension.LoadAssetsByType<UnitDescription>().ToDictionary(x => x.Id);
            _objects = AssetDatabaseExtension.LoadAssetsByType<MainMapObjectDescription>().ToDictionary(x => x.Id);
        }

        public IMainMapObjectDescription? GetDescription(string descriptionId) => _objects.GetValueOrDefault(descriptionId);

        public IUnitDescription? GetUnitDescription(string descriptionId) => _units.GetValueOrDefault(descriptionId);
    }
#endif

    public class ResourcesStatics : IGameStatics
    {
        private Dictionary<string, UnitDescription> _units;
        private Dictionary<string, IMainMapObjectDescription> _objects;

        private MainMap _map;

        public MainMap Map => _map;

        public ResourcesStatics()
        {
            var description = Resources.Load<GameDescription>("GameDescription");
            _map = description.mainMap;

            var mObjs = _map.GetComponentsInChildren<MainMapObjectUI>();
            UnityEngine.Debug.Log("ResourcesStatics obj count: " + mObjs.Length);
            _objects = mObjs.Select(x => x.GetDescription()).Distinct().ToDictionary(x => x.Id);

            _units = description.units.ToDictionary(x => x.Id);
            //_objects = AssetDatabaseExtension.LoadAssetsByType<MainMapObjectDescription>().ToDictionary(x => x.Id);
        }

        public IMainMapObjectDescription? GetDescription(string descriptionId) => _objects.GetValueOrDefault(descriptionId);

        public IUnitDescription? GetUnitDescription(string descriptionId) => _units.GetValueOrDefault(descriptionId);
    }
}
