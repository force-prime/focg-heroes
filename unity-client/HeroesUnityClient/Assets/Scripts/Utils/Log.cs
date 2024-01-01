static public class Log
{
    static public void Debug(string message)
    {
        UnityEngine.Debug.Log(message);
    }

    static public void Warning(string message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

    static public void Error(string message)
    {
        UnityEngine.Debug.LogError(message);
    }
}

