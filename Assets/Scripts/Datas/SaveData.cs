using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public string inkStateJSON;

    public List<string> choicesMadeList = new List<string>();

    public GamePresentationData presentation = new GamePresentationData();


    public string returnPoint;
    public int uiVersion;
    public int trust;
    public int delusion;
    public string playerName;
    public string lastLine = "";


    public bool hasStartedDialogue = false;
}