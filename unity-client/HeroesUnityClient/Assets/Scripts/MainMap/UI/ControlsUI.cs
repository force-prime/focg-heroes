using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class ControlsUI : MonoBehaviour
{
    [SerializeField] private Button buttonUp;
    [SerializeField] private Button buttonDown;
    [SerializeField] private Button buttonLeft;
    [SerializeField] private Button buttonRight;

    private PlayerCommands Commands => PlayerCommands.Current;

    private void Awake()
    {
        buttonUp.onClick.AddListener(OnUpClick);
        buttonDown.onClick.AddListener(OnDownClick);
        buttonLeft.onClick.AddListener(OnLeftClick);
        buttonRight.onClick.AddListener(OnRightClick);
    }

    private void OnRightClick()
    {
        Commands.Move(Utils.Direction.Right);
    }

    private void OnLeftClick()
    {
        Commands.Move(Utils.Direction.Left);
    }

    private void OnDownClick()
    {
        Commands.Move(Utils.Direction.Down);
    }

    private void OnUpClick()
    {
        Commands.Move(Utils.Direction.Up);
    }
}
