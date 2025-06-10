using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapePanel : MonoBehaviour
{
    public GameObject exitPanel;

    private void Start()
    {
        exitPanel.SetActive(false);
    }
    public void CloseExitPanel()
    {
        exitPanel.gameObject.SetActive(false);
    }

    //quit to interface according to the version recorded in GameState
    public void QuitToInterface()
    {
        int uiVersion = GameState.uiVersion; 
        string nextScene = "Version" + uiVersion;

        SceneManager.LoadScene(nextScene);
    }
}
