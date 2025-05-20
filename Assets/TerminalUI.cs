using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum InterfaceVersion { version1, version2, version3, version4 }

public class TerminalUI : MonoBehaviour/*, IDataPersistence*/
{
    GameData gameData;
    public InterfaceVersion interfaceVersion = InterfaceVersion.version1;

    public int storyProgress;

    //TODO: CheckInterfaceVersion OnEnable()

     void Update()
    {
        
        CheckInterfaceVersion();
    }



    //public void LoadData(GameData data)
    //{

    //    this.storyProgress = data.storyProgress;
    //    Debug.Log("dialogue data loaded: [" + this.storyProgress + "]");
    //}


    //public void SaveData(ref GameData data)
    //{

    //    data.storyProgress = this.storyProgress;
    //    Debug.Log("dialogue data saved: [" + this.storyProgress + "]");
    //}

    //make a method that checks the playerdata (if any) and load the version accordingly
    public void CheckInterfaceVersion()
    { 
        //if round 1 have not complete then interface version is 1
        if (gameData.storyProgress is >= 0 and <= 5)
        {
            Debug.Log("STORY PROGRESS 0 THEREFORE VER.1");
            interfaceVersion = InterfaceVersion.version1;
        }



        switch (interfaceVersion)
        {
            case InterfaceVersion.version1:
                Debug.Log("Currently in Version 1");
                break;
            case InterfaceVersion.version2:
                Debug.Log("Currently in Version 2");
                break;
            case InterfaceVersion.version3:
                Debug.Log("Currently in Version 3");
                break;
            case InterfaceVersion.version4:
                Debug.Log("Currently in Version 4");
                break;
        }
    }


}
