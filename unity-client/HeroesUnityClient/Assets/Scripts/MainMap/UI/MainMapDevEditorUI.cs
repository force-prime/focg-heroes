#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


[CustomEditor(typeof(MainMap))]
public class MainMapDevEditorUI : Editor
{
    void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        var map = (MainMap)target;

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Export", EditorStyles.miniButton))
            Export(map);
        if (GUILayout.Button("SnapToGrid", EditorStyles.miniButton))
            SnapToGrid(map);

        EditorGUILayout.EndHorizontal();


        base.OnInspectorGUI();
    }

    private void SnapToGrid(MainMap map)
    {
        foreach (var o in map.GetComponentsInChildren<MainMapObjectUI>())
        {
            var size = o.GetDescription().Size;
            var realMinXY = o.transform.localPosition - new Vector3(1.0f * size.x / 2, 1.0f * size.y / 2);
            var calculatedMinXY = new Vector3(Mathf.Round(realMinXY.x), Mathf.Round(realMinXY.y));
            o.transform.localPosition += (calculatedMinXY - realMinXY);
        }
    }

    private void Export(MainMap map)
    {
        map.InitFromUI();

        List<ExportData.Cell> cells = new List<ExportData.Cell>();

        for (int x = 0; x < map.Size.x; x++)
            for (int y = 0; y < map.Size.y; y++)
            {
                var c = map.GetMapCell(x, y);
                cells.Add(new ExportData.Cell { x = x, y = y, type = c.TileData.Type.ToString(), object_type = c.MapObject != null ? c.MapObject.name : "" });
            }
        
        var file = EditorUtility.SaveFilePanel("Export to", "", map.gameObject.scene.name, "json");

        var str = JsonUtility.ToJson(new ExportData { cells = cells.ToArray() }, true);
        File.WriteAllText(file, str);
    }

    [Serializable]
    private class ExportData
    {
        public Cell[] cells;
        //public ObjectData[] objects;

        [Serializable]
        public class Cell
        {
            public int x;
            public int y;
            public string type;
            public string object_type;
        }

        /*
        [Serializable]
        public class ObjectData
        {
            
        }
        */
    }
}
#endif