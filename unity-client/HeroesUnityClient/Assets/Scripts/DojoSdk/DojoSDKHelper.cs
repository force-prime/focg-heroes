using bottlenoselabs.C2CS.Runtime;
using Dojo;
using Dojo.Starknet;
using Dojo.Torii;
using dojo_bindings;
using DojoSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DojoSDKHelper : MonoBehaviour
{
    private JsonRpcClient _client;

    private ToriiClient _torii;

    private Account _account;
    private string _accountAddress;
    private string _rpcUrl;

    private WorldManager _worldManager;

    private void Awake()
    {
        _worldManager = UnityEngine.GameObject.FindAnyObjectByType<WorldManager>();

    }

    public void StartWorldManager(string toriiAddress, string worldAddress)
    {
        Log.Debug($"StartWorldManager: rpc={_rpcUrl}, torii={toriiAddress}, worldAddress={worldAddress}");
        _worldManager.rpcUrl = _rpcUrl;
        _worldManager.toriiUrl = toriiAddress;
        _worldManager.worldAddress = worldAddress;

        _worldManager.StartWorking();
    }

    private List<T> FindPlayerObjects<T>() where T : IPlayerState
    {
        List<T> result = new List<T>();
        _worldManager.GetComponentsInChildrenOnly<T>(result);
        result.RemoveAll(x => !CompareAddress(x.GetPlayerId(), _accountAddress));
        return result;
    }

    private bool CompareAddress(string a1, string a2)
    {
        return a1.TrimStart('0', 'x') == a2.TrimStart('0', 'x');
    }

    private T? FindSinglePlayerObject<T>() where T : IPlayerState
    {
        var all = FindPlayerObjects<T>();
        if (all.Count != 1)
            return default;
        return all[0];
    }

    public GameState? GetStateFromWorldData(GameState originalState)
    {
        var smapObjects = new List<MapObject>();
        var startObjects = new List<MapObjectData>();
        var cells = new List<PositionToCellData>();
        _worldManager.GetComponentsInChildrenOnly<MapObjectData>(startObjects);
        _worldManager.GetComponentsInChildrenOnly<PositionToCellData>(cells);
        _worldManager.GetComponentsInChildrenOnly<MapObject>(smapObjects);
        var pos2Cell = cells.ToDictionary(x => x.position);

        var position = FindSinglePlayerObject<PlayerPosition>();
        if (position == null)
            return null;

        var balance = FindSinglePlayerObject<PlayerBalance>();

        var units = FindPlayerObjects<PlayerUnits>();

        var mapObjects = FindPlayerObjects<MapObject>();
        var id2Object = mapObjects.ToDictionary(x => x.id);

        originalState.Heroes[0].Position = position.position;

        if (balance != null)
        {
            originalState.Heroes[0].Resources.Set(MapResourceEnum.Gold, (int)balance.gold);
        }

        foreach (var u in units)
        {
            originalState.Heroes[0].Army.Set(u.unitId.ToString(), (int) u.count);
        }

        List<LogicEntities.MapObject> toRemove = new List<LogicEntities.MapObject>();
        
        foreach (var obj in originalState.MapObjects)
        {
            if (id2Object.TryGetValue(uint.Parse(obj.Id), out var chainObj))
            {
                if (chainObj.descriptionId == 0)
                    toRemove.Add(obj);
            }
        }
        toRemove.ForEach(x => originalState.RemoveObject(x.Id));

        return originalState;
    }

    public Error SetupAccount(string rpcUrl, string privateKey, string account)
    {
        Log.Debug($"Dojo account: {account}");
        try
        {
            _client = new JsonRpcClient(rpcUrl);

            _rpcUrl = rpcUrl;
            _accountAddress = account;
            var signer = new SigningKey(privateKey);
            _account = new Account(_client, signer, account);
        }
        catch (Exception e)
        {
            Debug.LogError("Setup account failed: " + e.ToString());
            return new Error(e);
        }

        return null;
    }

    public void SetupTorii(string toriiUrl, string worldAddress)
    {
        throw new NotImplementedException();
    }

    public Error? Call(string address, string method, params string[] parameters)
    {
        var calldata = parameters.Select(x => dojo.felt_from_hex_be(CString.FromString(x)).ok).ToArray();

        dojo.Call call = new dojo.Call()
        {
            to = address,
            selector = method,
            calldata = calldata
        };

        try
        {
            _account.ExecuteRaw(new[] { call });
            return null;
        } catch (Exception e)
        {
            Debug.LogError("Call failed: " + e.ToString());
            return new Error(e);
        }
    }
}

static public class DojoSDKHelperMethods
{
    static public async Task<GameState> GetState(GameState baseState, GameLoader.UserData user, DojoSDKHelper dojoSdk, IGameStatics statics)
    {
        var err = dojoSdk.SetupAccount(user.server, user.privateKey, user.account);
        if (err != null)
            throw new Exception(err.ToString());

        dojoSdk.StartWorldManager(user.torii, user.world);

        var chainState = dojoSdk.GetStateFromWorldData(baseState);
        if (chainState == null)
        {
            dojoSdk.Call(user.actionsContract, "start_game");
        }

        return chainState ?? baseState;
    }

    static public async Task UploadMapDataToChain(string mapContract, DojoSDKHelper dojoSdk, GameState baseState, IGameStatics statics)
    {
        var staticsUploadData = ChainData.PrepareStaticsUploadData(baseState, statics);
        dojoSdk.Call(mapContract, "upload_descriptions", staticsUploadData.ToArray());
        await Task.Delay(5000);
        var uploadData = ChainData.PrepareMapUploadData(baseState, statics);
        dojoSdk.Call(mapContract, "upload_map", uploadData.ToArray());
    }

    static public string ToHex(this ReadOnlySpan<byte> bytes)
    {
        StringBuilder hex = new StringBuilder(bytes.Length * 2);
        hex.Append("0x");
        foreach (byte b in bytes)
            hex.AppendFormat("{0:x2}", b);
        return hex.ToString();
    }

    static public string ToHex(this dojo.FieldElement element)
    {
        return ToHex(element.data);
    }

}
