using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class PlayerCommands : MonoBehaviour
{
    private MainMap _map;

    static public PlayerCommands Current { get; private set; }

    private void Awake()
    {
        Current = this;
        _map = MainMapHelpers.GetMainMap();
    }

    public async Task Move(Utils.Direction direction)
    {
        var game = GameManager.Current.Game;

        var hero = _map.Heroes[0];
        var state = hero.GetState();
        var heroId = state.Id;
        var position = state.Position;

        var nextPosition = position.GetNext(direction);
        if (!_map.IsInsideMap(nextPosition))
        {
            Debug.Log("outside map");
            return;
        }

        var tile = game.State.GetTileAtPosition(nextPosition);
        var mapObject = game.State.GetObjectAtPosition(nextPosition, game.Statics);

        Error? error = null;

        if (mapObject == null)
        {
            if (tile == null)
            {
                Debug.LogError($"Incorrect map: null tile at {nextPosition}");
                return;
            }

            if (!tile.IsWalkable())
            {
                Debug.Log("tile not walkable");
                return;
            }

            error = await game.MoveHero(state.Id, direction);
        } else
        {
            var description = game.Statics.GetDescription(mapObject.DescriptionId);
            if (description == null)
            {
                Debug.LogError($"Description not found for id = '{mapObject.DescriptionId}'");
                return;
            }
            var result = await game.Interact(heroId, nextPosition);
            error = result.Error;
        }

        if (error != null)
            GameManager.Current.UI.ShowError(error);
    }
}
