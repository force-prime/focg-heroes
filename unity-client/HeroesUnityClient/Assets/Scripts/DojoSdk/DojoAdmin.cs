#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static GameDataHelpers;


public class DojoAdmin : MonoBehaviour
{
    [Header("KatanaData")]
    [SerializeField] private string rpcUrl;
    [SerializeField] private string account;
    [SerializeField] private string privateKey;
    [SerializeField] private string contract;

    [Header("GameData")]
    [SerializeField] private MainMap _map;

    private DojoSDKHelper _dojoSdk;

    void Awake()
    {
        _dojoSdk = GetComponent<DojoSDKHelper>();   

        _dojoSdk.SetupAccount(rpcUrl, privateKey, account);
    }

    Rect windowRect = new Rect(20, 20, 190, 50);

    void OnGUI()
    {
        // Register the window. Notice the 3rd parameter
        windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "DojoTester");
    }

    // Make the contents of the window
    void DoMyWindow(int windowID)
    {
        if (GUILayout.Button("UploadMap"))
            UploadMap();
    }

    private void UploadMap()
    {
        var statics = new EditorStatics();
        var state = GameDataHelpers.GameStateFromMainMap(_map);

        DojoSDKHelperMethods.UploadMapDataToChain(contract, _dojoSdk, state, statics);
    }
}
#endif