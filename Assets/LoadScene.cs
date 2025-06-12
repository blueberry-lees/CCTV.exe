using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    
    
    public void LoadTheScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        
    }

    public void LoadTheSceneAsync(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }




    public void LoadTheSceneAdditive(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void EmptyDialogueHistory(bool clear)
    {
        if (clear)
        {
            GameState.ClearDialogueHistory();
        }
        else return;
    }

    public void LoadInterfaceScene()
    {
        switch (GameState.uiVersion)
        {
            case 1:
                Debug.Log("UIVersion 1 ? Version1");
                SceneManager.LoadScene("Version1");
                break;
            case 2:
                Debug.Log("UIVersion 2 ? Version2");
                SceneManager.LoadScene("Version2");

                break;
            case 3:
                Debug.Log("UIVersion 3 ? Version3");
                SceneManager.LoadScene("Version3");

                break;
            case 4:
                Debug.Log("UIVersion 4 ? Version4");
                GameState.returnPoint = "SPLIT_ENDING"; //rename
                SceneManager.LoadScene("Version4");
                break;
            default:
                Debug.LogWarning("Unknown UI Version");
                break;
        }
    }
}
