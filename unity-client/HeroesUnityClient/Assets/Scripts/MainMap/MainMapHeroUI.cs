
using System;
using UnityEngine;

public class MainMapHeroUI : MonoBehaviour
{
    public void ProcessMove(Vector2Int newPosition)
    {
        GetComponent<MainMapObjectUI>().SetNewMinXYPosition(newPosition);
    }

    public LogicEntities.Hero GetState() => GameManager.Current.Game.State.GetHeroById(GetComponent<MainMapObjectUI>().GetId());
}
