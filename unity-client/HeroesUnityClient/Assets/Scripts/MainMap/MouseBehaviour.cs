using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MouseBehaviour : MonoBehaviour, MouseController.IMouseEvents
{
    private Vector3 _delta;

    private void Start()
    {
        MouseController.Current.SetDefaultHandler(this);
    }

    public bool HandleClick()
    {
        return true;
    }

    public void HandleMouseDown()
    {
        _delta = Vector3.zero;    
    }

    public void HandleMouseHold(Vector3 delta, bool inDrag)
    {
        if (inDrag)
        {
            var wp = Camera.main.ScreenToWorldPoint(_delta) - Camera.main.ScreenToWorldPoint(delta);
            wp.z = 0;
            Camera.main.transform.position += wp;
            _delta = delta;
        }
    }

    public void HandleMouseUp(Vector3 delta, bool inDrag)
    {
        
    }
}
