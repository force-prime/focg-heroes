using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class MainMapObjectUIEditorHelper : MonoBehaviour
{

    private void Awake()
    {
        if (Application.isPlaying)
            Destroy(this);
    }

    private void Update()
    {
        var c = GetComponent<MainMapObjectUI>();
        if (c != null)
        {
            var d = c.GetDescription();
            if (d == null)
                return;

            var size = d.Size;

            var collider2d = GetComponent<BoxCollider2D>();
            if (collider2d == null)
                collider2d = gameObject.AddComponent<BoxCollider2D>();

            collider2d.offset = Vector3.zero;
            collider2d.size = size;
        }
    }
}
