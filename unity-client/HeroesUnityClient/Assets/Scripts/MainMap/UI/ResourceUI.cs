using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    private LogicEntities.Hero _hero;

    [SerializeField] private TMP_Text goldLabel;

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

        var gold = _hero.Resources.Get(MapResourceEnum.Gold);
        goldLabel.text = $"Gold: {gold}";
    }
}
