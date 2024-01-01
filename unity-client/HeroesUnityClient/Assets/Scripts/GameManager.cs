using System.Collections.Concurrent;
using System.Threading.Tasks;

public class GameManager
{
    private static GameManager _instance;
    private static object _lockObject = new object();
    public static GameManager Current
    {
        get {
            if (_instance != null)
                return _instance;

            lock (_lockObject) {
                if (_instance == null) _instance = new GameManager();
                return _instance;
            }
        }
    }

    private ConcurrentQueue<GameEvent> _events = new ConcurrentQueue<GameEvent>();

    public IGameLogic Game { get; set; } = null;
    public IGameUI UI { get; set; } = null;

    public void AddEvent(GameEvent @event)
    {
        _events.Enqueue(@event);
    }

    private GameEvent _current;
    public void Update()
    {
        if (_current != null)
        {
            if (_current.IsHandled)
                _current = null;
            else
                return;
        }

        if (_events.TryDequeue(out _current))
            _current.Handle();
    }
}

public abstract class GameEvent
{
    public bool IsHandled { get; private set; }

    public async Task Handle()
    {
        try
        {
            var err = await ApplyEvent();
            if (err != null)
                await HandleFailure(err);
        } catch (System.Exception ex)
        {
            Log.Error("Exception handling event: " + ex);
        }
        finally
        {
            IsHandled = true;
        }
    }

    protected virtual Task<Error?> ApplyEvent() => Task.FromResult((Error)null);

    protected virtual Task HandleFailure(Error err)
    {
        Log.Warning($"{GetType().Name}: failed to apply event");
        GameManager.Current.UI?.ShowError(err);
        return Task.CompletedTask;
    }
}
