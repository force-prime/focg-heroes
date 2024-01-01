using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class MainMapObjectDescription : ScriptableObject, IMainMapObjectDescription
{
    [field: SerializeField] public Vector2Int Size { get; private set; }

    [field: SerializeField] public string Id { get; private set; }
}
