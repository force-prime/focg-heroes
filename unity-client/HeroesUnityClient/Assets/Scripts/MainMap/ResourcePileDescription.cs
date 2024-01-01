using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourcePile", menuName = "MainMap/ResourcePile")]
public class ResourcePileDescription : MainMapObjectDescription, IResourcePileDescription
{
    [SerializeField] public int count;

    public LogicEntities.Resources GetResources()
    {
        var r = new LogicEntities.Resources();
        r.Set(MapResourceEnum.Gold, count);
        return r;
    }
}
