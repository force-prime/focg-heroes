using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    private void Update()
    {
        var direction = Utils.Direction.None;

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            direction = Utils.Direction.Left;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            direction = Utils.Direction.Right;
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            direction = Utils.Direction.Up;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            direction = Utils.Direction.Down;
        }

        if (direction != Utils.Direction.None)
        {
            PlayerCommands.Current.Move(direction);
        }
    }
}
