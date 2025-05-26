using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum InterfaceVersion { version1, version2, version3, version4 }

public class TerminalUI : MonoBehaviour
{

    public InterfaceVersion interfaceVersion = InterfaceVersion.version1;

    


     void OnEnable()
    {
        
        
        CheckInterfaceVersion();
    }

    //make a method that checks the playerdata (if any) and load the version accordingly
    public void CheckInterfaceVersion()
    { 
        //if round 1 have not complete then interface version is 1
        if (PlayerPrefs.GetInt("UIVersion", 0) == 1)
        {
            Debug.Log("playerpref UIVerion is 1 THEREFORE VER.1");
            SceneManager.LoadScene("Interface1");
        }
        else if (PlayerPrefs.GetInt("UIVersion", 0) == 2)
        {
            Debug.Log("playerpref UIVerion is 2 THEREFORE VER.2");
            SceneManager.LoadScene("Interface2");

        }
        else if (PlayerPrefs.GetInt("UIVersion", 0) == 3)
        {
            Debug.Log("playerpref UIVerion is 3 THEREFORE VER.3");
            SceneManager.LoadScene("Interface3");

        }
        else if (PlayerPrefs.GetInt("UIVersion", 0) == 4)
        {
            Debug.Log("playerpref UIVerion is 3 THEREFORE VER.4");
            SceneManager.LoadScene("Interface4");

        }




        //switch (interfaceVersion)
        //{
        //    case InterfaceVersion.version1:
        //        SceneManager.LoadScene("Interface1");
        //        Debug.Log("Currently in Version 1");
        //        break;
        //    case InterfaceVersion.version2:
        //        SceneManager.LoadScene("Interface2");
        //        Debug.Log("Currently in Version 2");
        //        break;
        //    case InterfaceVersion.version3:
        //        SceneManager.LoadScene("Interface3");
        //        Debug.Log("Currently in Version 3");
        //        break;
        //    case InterfaceVersion.version4:
        //        SceneManager.LoadScene("Interface4");
        //        Debug.Log("Currently in Version 4");
        //        break;
        //}
    }


}
