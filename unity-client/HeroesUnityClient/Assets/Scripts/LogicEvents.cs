using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static public class LogicEvents
{
    public struct HeroMove
    {
        public string heroId;
        public Vector2Int newPosition;
    }

    public struct PickupResources
    {
        public string heroId;
        public string objectId;
        public LogicEntities.Resources resources;
    }

    public struct BattleEnd
    {
        public string heroId;
        public string objectId;
    }
}
