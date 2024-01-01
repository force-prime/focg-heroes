#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

public static class AssetDatabaseExtension
{
    public static T[] LoadAssetsByType<T>(string filter = "") where T : UnityEngine.Object
    {
        return LoadAssets<T>(filter + " t:" + typeof(T).Name);
    }

    public static T[] LoadAssets<T>(string filter = "") where T : UnityEngine.Object =>
        AssetDatabase
            .FindAssets(filter)
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<T>)
            .ToArray();
}
#endif