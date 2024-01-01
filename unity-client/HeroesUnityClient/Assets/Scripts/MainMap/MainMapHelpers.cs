using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

static public class MainMapHelpers
{
    static public MainMap GetMainMap()
    {
        return UnityEngine.GameObject.FindAnyObjectByType<MainMap>();
    }

    static public MainUI GetMainUI()
    {
        return UnityEngine.GameObject.FindAnyObjectByType<MainUI>(UnityEngine.FindObjectsInactive.Include);
    }

    static public LogicEntities.Hero GetMyHero()
    {
        var mainMap = GetMainMap();
        return mainMap.Heroes[0].GetState();
    }

    static public int GetArmyPower(LogicEntities.Army army, IGameStatics statics)
    {
        var power = 0;
        army.Iterate((id, count) => { var unitDescription = statics.GetUnitDescription(id);  power += count * (unitDescription.Attack + unitDescription.HP); });
        return power;
    }

    static public int GetHeroArmyPower(LogicEntities.Hero hero, IGameStatics statics) => GetArmyPower(hero.Army, statics) + 5;
}
