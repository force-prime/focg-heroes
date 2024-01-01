using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IUnitDescription
{
    Sprite GetIcon();

    int HP { get; }
    int Attack { get; }
}
