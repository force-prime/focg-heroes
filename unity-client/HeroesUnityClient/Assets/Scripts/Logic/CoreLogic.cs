

using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

public class CoreLogic : MessageCenter.IMessageCenter
{
    public bool IsStarted { get; internal set; }

    private LogicEntities.Tiles _tiles;

    private IGameStatics _statics;
    private IGameState _state;

    private MessageCenter _messageCenter = new MessageCenter();

    public Error? Initialize(GameState gameState, IGameStatics gameStatics)
    {
        _statics = gameStatics;
        _state = gameState;
        _tiles = gameState.Tiles;

        return null;
    }

    public Error? MoveHero(string heroId, Direction d)
    {
        var hero = _state.GetHeroById(heroId);
        if (hero == null)
            return new Error($"hero not found by id='{heroId}'");

        Vector2Int position = hero.Position.GetNext(d);

        Debug.Log($"Move requested: {hero.Position} -> {position}");

        if (position.x < 0 || position.y < 0 || position.x >= _tiles.Width || position.y >= _tiles.Height)
            return new Error("position out of map");

        if (!hero.Position.IsAdjacent(position))
            return new Error("position is not adjacent");

        var tile = _state.GetTileAtPosition(position);

        if (!tile.IsWalkable())
            return new Error("tile not walkable");

        var mapObject = _state.GetObjectAtPosition(position, _statics);
        if (mapObject != null)
        {
            // TODO
            return new Error("can't walk into this part of map object");
        }

        hero.Position = position;
        _messageCenter.Send(new LogicEvents.HeroMove { heroId = heroId, newPosition = position });

        return null;
    }

    public Error? Pickup(string heroId, string objectId)
    {
        var err = ExtractHeroAndMapObject(heroId, objectId, out var hero, out var mapObject, out var mapObjectDescription);
        if (err != null)
            return err;

        // TODO size > 1x1
        if (!hero.Position.IsAdjacent(mapObject.Position))
            return new Error("not adjacent object");

        IResourcePileDescription rp = mapObjectDescription as IResourcePileDescription;
        if (rp != null)
        {
            var res = rp.GetResources();
            hero.Resources.Add(res);
            _messageCenter.Send(new LogicEvents.PickupResources { heroId = heroId, objectId = objectId, resources = res });
            RemoveObject(objectId); 
            return null;
        }

        return new Error("not pickupable");
    }

    public Error? PurchaseUnit(string heroId, string objectId, int count)
    {
        var err = ExtractHeroAndMapObject(heroId, objectId, out var hero, out var mapObject, out var mapObjectDescription);
        if (err != null)
            return err;

        var res = hero.Resources;

        IUnitShopDescription unitShop = mapObjectDescription as IUnitShopDescription;
        if (unitShop == null)
            return new Error($"{mapObjectDescription.ToString()} is not shop description");

        var cost = unitShop.GetCost();
        cost.Multiply(count);

        if (!hero.Resources.HasEnough(cost))
            return new Error("not enough resources");

        hero.Resources.Sub(cost);

        var army = unitShop.GetArmy();
        army.Multiply(count);

        hero.Army.Add(army);

        return null;
    }


    public Error? FightUnit(string heroId, string objectId)
    {
        var err = ExtractHeroAndMapObject(heroId, objectId, out var hero, out var mapObject, out var mapObjectDescription);
        if (err != null)
            return err;

        var army = mapObjectDescription as IArmyDescription;
        if (army == null)
            return new Error($"{mapObjectDescription.ToString()} not enough money");

        var myPower = MainMapHelpers.GetHeroArmyPower(hero, _statics);
        var armyPower = MainMapHelpers.GetArmyPower(army.GetArmy(), _statics);

        Debug.Log($"Fighting {myPower} -> {armyPower}");
        if (myPower < armyPower)
            return new Error($"Your army needs more power: {myPower} vs {armyPower}");

        _messageCenter.Send(new LogicEvents.BattleEnd { heroId = heroId, objectId = objectId });
        RemoveObject(objectId);

        return null;
    }

    private void RemoveObject(string objectId)
    {
        _state.RemoveObject(objectId);
    }

    private Error? ExtractHeroAndMapObject(string heroId, string objectId, out LogicEntities.Hero? hero, out LogicEntities.MapObject? mapObject, out IMainMapObjectDescription? mapObjectDescription)
    {
        hero = null;
        mapObject = null;
        mapObjectDescription = null;

        hero = _state.GetHeroById(heroId);
        if (hero == null) 
            return new Error($"hero not found by id='{heroId}'");

        mapObject = _state.GetMapObjectById(objectId);
        if (mapObject == null)
            return new Error($"map object not found by id='{objectId}'");

        mapObjectDescription = _statics.GetDescription(mapObject.DescriptionId);
        if (mapObjectDescription == null)
            return new Error($"description not found by id='{mapObject.DescriptionId}'");

        return null;
    }

    public void Subscribe<TMessage>(MessageCenter.Handler<TMessage> handler) => _messageCenter.Subscribe(handler);
    public void Unsubscribe<TMessage>(MessageCenter.Handler<TMessage> handler) => _messageCenter.Unsubscribe(handler);

}
