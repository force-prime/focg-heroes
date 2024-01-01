using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUI : MonoBehaviour
{
    private LogicEntities.Hero _hero;

    [SerializeField] private TMP_Text powerLabel;

    // Start is called before the first frame update
    void Start()
    {

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

        var power = MainMapHelpers.GetHeroArmyPower(_hero, GameManager.Current.Game.Statics);
        powerLabel.text = $"Power: {power}";
    }
}
