using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

public class DojoSDKLogic : MapLogicBase { 
    private DojoSDKHelper _dojoSdk;

    private string ActionsContract => GameLoader.Instance.User.actionsContract;

    protected override void Awake()
    {
        base.Awake();

        _dojoSdk = GetComponent<DojoSDKHelper>();
    }

    public override async Task<Result<InteractionType>> Interact(string heroId, Vector2Int position)
    {
        var obj = State.GetObjectAtPosition(position, Statics);

        var r = await base.Interact(heroId, position);
        if (r.IsError)
            return r;

        var callResult = r.Value switch {
            InteractionType.Pickup => _dojoSdk.Call(ActionsContract, "pickup", position.x.ToHexString(), position.y.ToHexString()),
            InteractionType.UnitPurchase => await PurchaseUnit(heroId, obj, 1),
            InteractionType.Attack => await Attack(heroId, obj),
            _ => throw new NotImplementedException()
        };

        return r;
    }

    public override async Task<Error> MoveHero(string heroId, Direction direction)
    {
        var error = await base.MoveHero(heroId, direction);

        if (error != null)
            return error;

        return _dojoSdk.Call(ActionsContract, "move", ((int) direction).ToHexString());
    }

    public async Task<Error> PurchaseUnit(string heroId, LogicEntities.MapObject obj, int count)
    {
        return _dojoSdk.Call(ActionsContract, "purchase_unit", obj.Position.x.ToHexString(), obj.Position.y.ToHexString(), count.ToHexString());
    }

    public async Task<Error> Attack(string heroId, LogicEntities.MapObject obj)
    {
        return _dojoSdk.Call(ActionsContract, "fight", obj.Position.x.ToHexString(), obj.Position.y.ToHexString());
    }
}

#if UNITY_EDITOR
public class DojoEditorEnv : EditorEnv
{
    private readonly DojoSDKHelper _dojoSdk;

    public DojoEditorEnv(DojoSDKHelper dojoSdk)
    {
        _dojoSdk = dojoSdk;
    }

    public override async Task<GameState> GetState(GameLoader.UserData user)
    {
        var state = await base.GetState(user);

        var statics = await GetGameStatics();

        return await DojoSDKHelperMethods.GetState(state, user, _dojoSdk, statics);
    }
}
#endif