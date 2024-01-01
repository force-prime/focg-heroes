using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDescription", menuName = "GameDescription")]
public class GameDescription : ScriptableObject
{
    [SerializeField] public MainMap mainMap;
    [SerializeField] public UnitDescription[] units;
    //[SerializeField] private MainMapObjectDescription mapObjects;
}
