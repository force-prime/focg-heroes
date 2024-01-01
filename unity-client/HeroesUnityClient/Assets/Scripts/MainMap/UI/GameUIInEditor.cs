using UnityEngine;

public class GameUIInEditor : MonoBehaviour
{
    Rect windowRect = new Rect(20, 20, 290, 250);

    void OnGUI()
    {
        // Register the window. Notice the 3rd parameter
        windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "GAMEUI");
    }

    // Make the contents of the window
    void DoMyWindow(int windowID)
    {
        //var game = GameManager.Current.Game;

        var map = MainMapHelpers.GetMainMap();
        var hero = map.Heroes[0];
        var state = hero.GetState();

        GUILayout.Label($"Position: {state.Position}");

        GUILayout.Label("Resources");
        state.Resources.Iterate((id, value) => GUILayout.Label($"{id}={value}"));

        GUILayout.Label("Army");
        state.Army.Iterate((id, value) => GUILayout.Label($"{id}={value}"));
    }

}
