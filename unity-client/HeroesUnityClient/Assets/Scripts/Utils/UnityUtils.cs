using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static public class UnityUtils
{
    static public void GetComponentsInChildrenOnly<T>(this Component obj, List<T> result)
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            var child = obj.transform.GetChild(i);
            result.AddRange(child.GetComponentsInChildren<T>());
        }
    }

    static public T? GetComponentInChildrenOnly<T>(this Component obj)
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            var child = obj.transform.GetChild(i);
            var component = child.GetComponentInChildren<T>();
            if (component != null)
                return component;
        }
        return default;
    }
}
