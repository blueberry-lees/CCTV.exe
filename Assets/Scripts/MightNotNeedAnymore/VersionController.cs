using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum InterfaceVersion { version1, version2, version3, version4 }

public class VersionController : MonoBehaviour
{
    public InterfaceVersion interfaceVersion = InterfaceVersion.version1;

    public GameObject v1, v2, v3, v4;


    void OnEnable()
    {
        CheckInterfaceVersion();
    }

    //make a method that checks the playerdata (if any) and load the version accordingly
    public void CheckInterfaceVersion()
    {
        int version = PlayerPrefs.GetInt("UIVersion", 0);

        switch (version)
        {
            case 1:
                Debug.Log("playerpref UIVersion is 1, THEREFORE VER.1");
                if (v1 != null)
                    Instantiate(v1, transform); // Optional: set parent
                else
                    Debug.LogWarning("v1 prefab not assigned.");
                break;

            case 2:
                Debug.Log("playerpref UIVersion is 2, THEREFORE VER.2");
                if (v2 != null)
                    Instantiate(v2, transform); // Optional: set parent
                else
                    Debug.LogWarning("v2 prefab not assigned.");
                break;

            case 3:
                Debug.Log("playerpref UIVersion is 3, THEREFORE VER.3");
                if (v3 != null)
                    Instantiate(v3, transform); // Optional: set parent
                else
                    Debug.LogWarning("v3 prefab not assigned.");
                break;

            case 4:
                Debug.Log("playerpref UIVersion is 4, THEREFORE VER.4");
                if (v4 != null)
                    Instantiate(v4, transform); // Optional: set parent
                else
                    Debug.LogWarning("v4 prefab not assigned.");
                break;

            default:
                Debug.Log("No matching UIVersion found. Defaulting to version 1.");
                if (v1 != null)
                    Instantiate(v1, transform);
                break;
        }
    }
}


