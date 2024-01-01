using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IGameEnvironment
{
    Task<IGameStatics> GetGameStatics();
    Task<GameState> GetState(GameLoader.UserData user);
    Task<GameLoader.UserData> GetUserData(); 
}
