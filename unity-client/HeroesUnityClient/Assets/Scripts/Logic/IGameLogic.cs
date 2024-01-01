using System.Threading.Tasks;
using UnityEngine;
using Utils;

public interface IGameLogic
{
    IGameState State { get; }
    IGameStatics Statics { get; }
    MessageCenter.IMessageCenter MessageCenter { get; }

    void Initialize(GameState gameState, IGameStatics gameStatics);

    Task<Result<InteractionType>> Interact(string heroId, Vector2Int position);
    Task<Error> MoveHero(string heroId, Direction direction);
    Task<Error> PurchaseUnit(string heroId, string objectId, int count);
}

public enum InteractionType
{
    Pickup,
    UnitPurchase,
    Attack
}