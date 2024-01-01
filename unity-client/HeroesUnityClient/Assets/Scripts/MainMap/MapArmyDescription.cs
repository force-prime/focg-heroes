using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "MapArmy", menuName = "MainMap/MapArmy")]
public class MapArmyDescription : MainMapObjectDescription, IArmyDescription
{
    [SerializeField] private UnitDescription _unitDescription;

    public LogicEntities.Army GetArmy()
    {
        var a = new LogicEntities.Army();
        a.Set(_unitDescription.Id, 1);
        return a;
    }
}
