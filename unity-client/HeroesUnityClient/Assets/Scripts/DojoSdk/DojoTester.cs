using bottlenoselabs.C2CS.Runtime;
using Dojo.Starknet;
using Dojo.Torii;
using dojo_bindings;
using System;
using UnityEngine;

public class DojoTester : MonoBehaviour
{
    private const string TORII_URL = "http://127.0.0.1:8080";
    private const string RPC_URL = "http://127.0.0.1:5050";

    private const string ACCOUNT = "0x0517ececd29116499f4a1b64b094da79ba08dfd54a3edaa316134c41f8160973";
    private const string PRIVATE_KEY = "0x1800000000300000180000000000030000000000003006001800006600";

    private const string WORLD_ADDRESS = "0x75ed4e805bb9499491fe0c4b38e76e380a5fead91fa7299effdcd9e3aabc52e";
    private const string ACTIONS_ADDRESS = "0x3d88fea916a3db5c40407529f62cb6c78abb0dab05024f1dda5cf574b87a89";
    private const string MAPACTIONS_ADDRESS = "0x1095bad8c35b628360800e3bce58c4133b5c5de98b37f31c867191ced2c3a5f";

    private DojoSDKHelper _dojoSdk;

    Rect windowRect = new Rect(20, 20, 190, 50);

    void OnGUI()
    {
        // Register the window. Notice the 3rd parameter
        windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "DojoTester");
    }

    // Make the contents of the window
    void DoMyWindow(int windowID)
    {
        GUILayout.Label("RPC_URL: " + RPC_URL);
      
        if (GUILayout.Button("Start"))
            StartGameRequest();

        if (GUILayout.Button("Move"))
            DoMove();

        if (GUILayout.Button("UploadMap"))
            UploadMap();
    }


    private void Awake()
    {
        _dojoSdk = GetComponent<DojoSDKHelper>();
        SetupAccount();
    }

    private void SetupAccount()
    {
        var err = _dojoSdk.SetupAccount(RPC_URL, PRIVATE_KEY, ACCOUNT);
    }

    private void SetupTorii()
    {
       /*
        var entities = new dojo.KeysClause[]
        {
            new()
            {
                model = "PlayerPosition",
                keys = new string[]{ ACCOUNT }
            }
        };

        _torii = new ToriiClient(TORII_URL, RPC_URL, WORLD_ADDRESS, entities);

        _torii.StartSubscription();
        
        _torii.RegisterSyncModelUpdates(new dojo.KeysClause { model = "PlayerPosition", keys = new[] { ACCOUNT } }, false);

        ToriiEvents.Instance.OnSyncModelUpdated += Instance_OnSyncModelUpdated;
       */
    }

    private void Instance_OnSyncModelUpdated()
    {
        Debug.Log("Instance_OnSyncModelUpdated");
    }

    private void StartGameRequest()
    {
        _dojoSdk.Call(ACTIONS_ADDRESS, "start_game");
    }

    private void DoMove()
    {
        _dojoSdk.Call(ACTIONS_ADDRESS, "move", "0x2");
    }

    private void UploadMap()
    {
        /*
        var statics = new EditorStatics();
        var state = GameDataHelpers.GameStateFromMainMap(MainMapHelpers.GetMainMap());
        var uploadData = ChainData.PrepareMapUploadData(state, statics, ACCOUNT);
        _dojoSdk.Call(MAPACTIONS_ADDRESS, "upload_map", uploadData.ToArray());
        */
    }
}
