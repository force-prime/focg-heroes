using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitDescription", menuName = "Units/UnitDescription")]
public class UnitDescription : ScriptableObject, IUnitDescription
{
    [SerializeField] private Sprite _icon;

    [field: SerializeField] public string Id { get; private set; }

    [field: SerializeField] public int Attack { get; private set; }
    [field: SerializeField] public int HP { get; private set; }

    public Sprite GetIcon() => _icon;
}
