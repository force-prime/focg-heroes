using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Utils;

public class MapLogicBase : MonoBehaviour, IGameLogic
{
    private readonly CoreLogic _coreLogic = new CoreLogic();

    public IGameState State { get; private set; }
    public IGameStatics Statics { get; private set; }

    public MessageCenter.IMessageCenter MessageCenter => _coreLogic;

    protected virtual void Awake()
    {
        GameManager.Current.Game = this;
    }

    public void Initialize(GameState gameState, IGameStatics gameStatics)
    {
        State = gameState;
        Statics = gameStatics;
        _coreLogic.Initialize(gameState, Statics);
    }

    public virtual async Task<Result<InteractionType>> Interact(string heroId, Vector2Int position)
    {
        var obj = State.GetObjectAtPosition(position, Statics);
        var objectId = obj.Id;
        var error = _coreLogic.Pickup(heroId, objectId);
        if (error == null)
            return new Result<InteractionType>(InteractionType.Pickup);

        error = _coreLogic.PurchaseUnit(heroId, objectId, 1);
        if (error == null)
            return new Result<InteractionType>(InteractionType.UnitPurchase);

        error = _coreLogic.FightUnit(heroId, objectId);
        if (error == null)
            return new Result<InteractionType>(InteractionType.Attack);

        return new Result<InteractionType>(error);
    }

    public virtual Task<Error> MoveHero(string heroId, Direction d)
    {
        var error = _coreLogic.MoveHero(heroId, d);

        return Task.FromResult(error);
    }

    public virtual Task<Error> PurchaseUnit(string heroId, string objectId, int count)
    {
        throw new NotImplementedException();
    }
}
