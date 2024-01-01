using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArmyUI : MonoBehaviour
{
    private LogicEntities.Hero _hero;
    private readonly List<UnitUI> _units = new List<UnitUI>();
    private readonly List<string> _keys = new List<string>();

    void Awake()
    {
        GetComponentsInChildren<UnitUI>(_units);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_hero == null)
            _hero = MainMapHelpers.GetMyHero();

        if (_hero == null)
            return;


        _keys.Clear();
        _hero.Army.GetKeys(_keys);

        var index = 0;

        foreach (var k in _keys)
        {
            var description = GameManager.Current.Game.Statics.GetUnitDescription(k);
            _units[index].Attach(description, _hero.Army.Get(k));

            index++;
        }

        while (index < 5)
        {
            _units[index].Hide();
            index++;
        }
    }
}
