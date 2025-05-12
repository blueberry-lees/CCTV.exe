using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class QuitApplication : MonoBehaviour
{
    public void QuitGame()
    {
       Application.Quit();
        Debug.Log("Quiting game...");

    }


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
