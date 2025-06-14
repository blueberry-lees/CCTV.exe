using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameStateDebugger : MonoBehaviour
{
    [Header("Ink State (usually hidden)")]
    [TextArea(3, 10)]
    public string inkStateJSON;

    [Header("Player Info")]
    public string playerName;
    public string returnPoint;
    public int uiVersion = 1;
    public int trust = 5;
    public int delusion = 5;

    [Header("Presentation State")]
    public string lastBackground;
    public string lastCharacter;
    public string lastExpression;
    public string lastSpeaker;
    public string lastAmbient;

    [Header("Controls")]
    public bool loadFromSave;
    public bool saveToFile;
    public bool resetSaveFile;

    void OnValidate()
    {
        // Handle editor toggles
        if (loadFromSave)
        {
            LoadGameState();
            loadFromSave = false;
        }

        if (saveToFile)
        {
            ApplyToGameState();
            GameState.SaveAll();
            saveToFile = false;
        }

        if (resetSaveFile)
        {
            GameState.ResetAll();
            resetSaveFile = false;
        }
    }

    void LoadGameState()
    {
        GameState.LoadAll();

        // Copy from GameState to inspector
        inkStateJSON = GameState.inkStateJSON;
        playerName = GameState.playerName;
        returnPoint = GameState.returnPoint;
        uiVersion = GameState.uiVersion;
        trust = GameState.trust;
        delusion = GameState.delusion;

        lastBackground = GameState.presentation.lastBackground;
        lastCharacter = GameState.presentation.lastCharacter;
        lastExpression = GameState.presentation.lastExpression;
        lastSpeaker = GameState.presentation.lastSpeaker;
        lastAmbient = GameState.presentation.lastAmbient;
    }

    void ApplyToGameState()
    {
        // Copy from inspector to GameState
        GameState.inkStateJSON = inkStateJSON;
        GameState.playerName = playerName;
        GameState.returnPoint = returnPoint;
        GameState.uiVersion = uiVersion;
        GameState.trust = trust;
        GameState.delusion = delusion;

        GameState.presentation.lastBackground = lastBackground;
        GameState.presentation.lastCharacter = lastCharacter;
        GameState.presentation.lastExpression = lastExpression;
        GameState.presentation.lastSpeaker = lastSpeaker;
        GameState.presentation.lastAmbient = lastAmbient;
    }
}
