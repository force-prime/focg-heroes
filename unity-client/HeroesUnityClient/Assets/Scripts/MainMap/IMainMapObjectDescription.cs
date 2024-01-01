using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IMainMapObjectDescription
{
    string Id { get; }
    Vector2Int Size { get; }
}

public interface IResourcePileDescription : IMainMapObjectDescription
{
    LogicEntities.Resources GetResources();
}

public interface IUnitShopDescription : IMainMapObjectDescription
{
    string UnitId { get; }
    LogicEntities.Resources GetCost();
    LogicEntities.Army GetArmy();
}

public interface IArmyDescription : IMainMapObjectDescription
{
    LogicEntities.Army GetArmy();
}