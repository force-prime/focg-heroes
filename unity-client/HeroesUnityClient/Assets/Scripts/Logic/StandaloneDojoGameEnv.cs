using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static GameDataHelpers;

public class StandaloneDojoGameEnv : IGameEnvironment
{
    private string _localId;
    private ResourcesStatics _gameStatics;
    private DojoSDKHelper _dojoSdk;

    public StandaloneDojoGameEnv(DojoSDKHelper dojoSdk)
    {
        try
        {
            _localId = File.ReadAllText("id.loc");
        } catch (Exception) { }
        if (string.IsNullOrEmpty(_localId))
        {
            _localId = Guid.NewGuid().ToString();
            File.WriteAllText("id.loc", _localId);
        }

        _gameStatics = new ResourcesStatics();
        _dojoSdk = dojoSdk;
    }

    public async Task<IGameStatics> GetGameStatics()
    {
        return _gameStatics;
    }

    public async Task<GameState> GetState(GameLoader.UserData user)
    {
        var statics = await GetGameStatics();
        var state = GameDataHelpers.GameStateFromMainMap(_gameStatics.Map);
        return await DojoSDKHelperMethods.GetState(state, user, _dojoSdk, statics);
    }

    public async Task<GameLoader.UserData> GetUserData()
    {
        var result = await SendRequest(AdminConfig.LOGIN_URL);
        if (result.IsError)
            throw new Exception(result.Error.ToString());

        var account = JsonUtility.FromJson<AccountData>(result.Value);

        return new GameLoader.UserData
        {
            world = account.starknet_config.world,
            privateKey = account.wallet.privateKey,
            account = account.wallet.address,
            actionsContract = account.starknet_config.contract,
            server = account.starknet_config.katana_url,
            torii = account.starknet_config.torii_url
        };
    }

    private Task<Result<string>> SendRequest(string uri)
    {
        var request = UnityWebRequest.Get($"{uri}?user_id={_localId}&access_token={AdminConfig.ACCESS_TOKEN}");

        TaskCompletionSource<Result<string>> taskCompletionSource = new TaskCompletionSource<Result<string>>();

        var sent = request.SendWebRequest();

        sent.completed += op => { taskCompletionSource.SetResult(request.result == UnityWebRequest.Result.Success ?
            new Result<string>(request.downloadHandler.text) :
            new Result<string>(new Error(request.error))); };

        return taskCompletionSource.Task;
    }

    [Serializable]
    private class AccountData
    {
        public StarknetData starknet_config;
        public WalletData wallet;

        [Serializable]
        public class StarknetData
        {
            public string world;
            public string contract;
            public string torii_url;
            public string katana_url;
        }

        [Serializable]
        public class WalletData
        {
            public string address;
            public string privateKey;
        }
    }
}
