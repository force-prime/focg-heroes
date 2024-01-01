using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameLoader;

public class GameLoader : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] public Button dojoGameButton;
    [SerializeField] public Button localGameButton;
    [SerializeField] public TMP_Text statusLabel;

    [Header("Logic")]
    [SerializeField] private GameObject dojoSetup;

    static public GameLoader Instance;
    public string ErrorMessage { get; private set; } = "";

    private float _lastConnectTime = -1;

    public UserData User { get; private set; }
    public struct UserData {
        public string server;
        public string account;
        public string publicKey;
        public string privateKey;
        public string torii;

        public string world;

        public string actionsContract;
    }

    public IGameEnvironment GameEnv { get; private set; }

    public enum LoadingState
    {
        NotConnected,
        Connecting,
        Ready
    }

    public LoadingState State { get; private set; } = LoadingState.NotConnected;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        Instance = this;

        dojoGameButton.onClick.AddListener(OnDojoGameClick);
        localGameButton.onClick.AddListener(OnLocalGameClick);

#if UNITY_WEBGL
        Debug.Log("URL: " + Application.absoluteURL);
#endif
    }

    private async Task<bool> LoadUser()
    {
        try
        {
            User = await GameEnv.GetUserData();

            return true;
        } catch (Exception e)
        {
            ErrorMessage = "Can't load user data";
            return false;
        }
    }

    private async Task<Error?> LoadGameDataAndState()
    {
        try
        {
            var gameState = await GameEnv.GetState(User);
            var gameStatics = await GameEnv.GetGameStatics();

            Debug.Log($"Loading complete, state={gameState}, statics={gameStatics}");

            GameManager.Current.Game.Initialize(gameState, gameStatics);

            return null;
        } catch (Exception e)
        {
            Log.Error("Can't load game data/state: " + e.ToString());
            return new Error("Can't load game data/state");
        }
    }
 
    private void SetupDojoEnv()
    {
        var dojoObj = UnityEngine.GameObject.Instantiate(dojoSetup, transform);
        var dojo = dojoObj.GetComponent<DojoSDKHelper>();
#if UNITY_EDITOR
        GameEnv = new DojoEditorEnv(dojo);
#else
        GameEnv = new StandaloneDojoGameEnv(dojo);
#endif
    }

    private async void Connect()
    {
        UpdateControls(false);

        _lastConnectTime = Time.time;

        ErrorMessage = null;
        State = LoadingState.Connecting;

        var err = await LoadGameDataAndState();

        if (err == null) { 
            State = LoadingState.Ready;
            SceneManager.LoadScene("GameScene");
        } else 
        {
            ErrorMessage = err.ToString();
            State = LoadingState.NotConnected;
        }
    }

    private void OnDojoGameClick()
    {
        SetupDojoEnv();
        StartGame();
    }

    private void OnLocalGameClick()
    {
        GameEnv = new StandaloneLocalEnv();
        gameObject.AddComponent<MapLogicBase>();
        StartGame();
    }

    private async Task StartGame()
    {
        UpdateControls(false);
        await LoadUser();
        Connect();
    }

    private void UpdateControls(bool active)
    {
        localGameButton?.gameObject.SetActive(active);
        dojoGameButton?.gameObject.SetActive(active);
    }

    void Update()
    {
        GameManager.Current.Update();

        UpdateStatusLabel();
    }

    private void UpdateStatusLabel()
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
            statusLabel.text = ErrorMessage;
        else if (State == LoadingState.Connecting)
            statusLabel.text = "Connecting...";
    }
}

static public class WebGameLoader
{
    static public bool ExtractFromUri(ref UserData userData)
    {
        try
        {
            Uri myUri = new Uri(Application.absoluteURL);
            var query = System.Web.HttpUtility.ParseQueryString(myUri.Query);

            userData.server = query.Get("server");
            userData.account = query.Get("account");
            userData.publicKey = query.Get("public_key");
            userData.privateKey = query.Get("private_key");
            userData.torii = query.Get("torii");
            return true;
        } catch (Exception e)
        {
            Log.Error("Can't parse url: " + Application.absoluteURL);
            return false;
        }
    }
} 