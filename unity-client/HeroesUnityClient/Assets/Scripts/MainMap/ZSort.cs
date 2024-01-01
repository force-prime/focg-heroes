using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[ExecuteInEditMode]
public class ZSort : MonoBehaviour
{
    private void Update()
    {
        var t = transform.localPosition;
        transform.localPosition = new Vector3(t.x, t.y, t.y / 100);
    }
}
