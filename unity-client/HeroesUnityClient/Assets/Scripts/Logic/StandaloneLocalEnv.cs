using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameDataHelpers;

public class StandaloneLocalEnv : IGameEnvironment
{
    private ResourcesStatics _gameStatics;

    public StandaloneLocalEnv()
    {
        _gameStatics = new ResourcesStatics();
    }

    public async Task<IGameStatics> GetGameStatics()
    {
        return _gameStatics;
    }

    public virtual async Task<GameState> GetState(GameLoader.UserData user)
    {
        var statics = await GetGameStatics();
        var state = GameDataHelpers.GameStateFromMainMap(_gameStatics.Map);
        return state;
    }

    public virtual async Task<GameLoader.UserData> GetUserData()
    {
        return new GameLoader.UserData { };
    }
}
