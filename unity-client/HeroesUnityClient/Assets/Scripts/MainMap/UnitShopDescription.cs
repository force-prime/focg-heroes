using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "UnitShop", menuName = "MainMap/UnitShop")]
public class UnitShopDescription : MainMapObjectDescription, IUnitShopDescription
{
    [SerializeField] private UnitDescription _unitDescription;
    [field: SerializeField] public int UnitCost { get; private set; }

    public string UnitId => _unitDescription.Id;

    public LogicEntities.Army GetArmy()
    {
        var a = new LogicEntities.Army();
        a.Set(UnitId, 1);
        return a;
    }

    public LogicEntities.Resources GetCost()
    {
        var r = new LogicEntities.Resources();
        r.Set(MapResourceEnum.Gold, UnitCost);
        return r;
    }
}
