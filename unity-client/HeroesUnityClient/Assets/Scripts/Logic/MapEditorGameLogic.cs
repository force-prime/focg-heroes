#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Utils;

public class MapEditorGameLogic : MapLogicBase
{
    override protected void Awake()
    {
        base.Awake();

        MainMapHelpers.GetMainUI().gameObject.SetActive(true);  
    }

    private void Start()
    {
        var state = GameDataHelpers.GameStateFromMainMap(MainMapHelpers.GetMainMap());
        Initialize(state, new GameDataHelpers.EditorStatics());
    }
}

public class EditorEnv : IGameEnvironment
{
    public virtual Task<IGameStatics> GetGameStatics()
    {
        return Task.FromResult<IGameStatics>(new GameDataHelpers.EditorStatics());
    }

    public virtual Task<GameState> GetState(GameLoader.UserData user)
    {
        var maps = AssetDatabaseExtension.LoadAssets<UnityEngine.GameObject>("l:map");
        var map = maps[0].GetComponent<MainMap>();
        var state = GameDataHelpers.GameStateFromMainMap(map);
        return Task.FromResult(state);
    }

    public virtual Task<GameLoader.UserData> GetUserData()
    {
        //var user = new GameLoader.UserData();
        var user = GetLocalUserData();
        return Task.FromResult(user);
    }

    static private GameLoader.UserData GetLocalUserData()
    {
        var userData = new GameLoader.UserData();

        userData.actionsContract = "0x3d88fea916a3db5c40407529f62cb6c78abb0dab05024f1dda5cf574b87a89";

        userData.world = "0x75ed4e805bb9499491fe0c4b38e76e380a5fead91fa7299effdcd9e3aabc52e";

        userData.server = "http://127.0.0.1:5050";
        userData.torii = "http://127.0.0.1:8080";

        userData.account = "0x0517ececd29116499f4a1b64b094da79ba08dfd54a3edaa316134c41f8160973";
        //userData.publicKey = "0x223353dca4e0a209d3d672ce0c4a153ca19e216ba335442cea3c69493bccdcd";
        userData.privateKey = "0x1800000000300000180000000000030000000000003006001800006600";
        

        return userData;
    }
}
#endif