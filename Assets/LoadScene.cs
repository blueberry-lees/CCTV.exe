using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string sceneName;
    
    public void LoadTheScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadTheSceneAsync()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }


    public void LoadTheSceneAdditive()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}
