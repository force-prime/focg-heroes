using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using static LogicEntities;

namespace Assets.Scripts
{
    static public class CoreLogicHelper
    {
        static public RectInt GetObjectBounds(LogicEntities.MapObject mapObj, IGameStatics statics)
        {
            IMainMapObjectDescription description = statics.GetDescription(mapObj.DescriptionId);
            return new RectInt(mapObj.Position, description.Size - Vector2Int.one);
        }

        static public LogicEntities.MapObject? GetObjectAtPosition(IEnumerable<LogicEntities.MapObject> objects, Vector2Int position, IGameStatics statics)
        {
            foreach (var mapObj in objects)
            {
                var b = GetObjectBounds(mapObj, statics);
                if (position.x >= b.min.x && position.y >= b.min.y && position.x <= b.max.x && position.y <= b.max.y)
                    return mapObj;
            }
            return null;
        }
    }
}
