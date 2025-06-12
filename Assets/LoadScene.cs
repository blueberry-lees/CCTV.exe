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
}
