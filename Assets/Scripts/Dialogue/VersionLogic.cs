using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionLogic : MonoBehaviour
{
    [System.Serializable]
    public class InterfaceVersionData
    {
        public int uiVersion;
        public string returnPoint;
        public string sceneName;
    }

    public static readonly Dictionary<int, InterfaceVersionData> interfaceVersions = new()
    {
        { 1, new InterfaceVersionData { uiVersion = 1, returnPoint = "ROUND_1", sceneName = "Version1" } },
        { 2, new InterfaceVersionData { uiVersion = 2, returnPoint = "ROUND_2", sceneName = "Version2" } },
        { 3, new InterfaceVersionData { uiVersion = 3, returnPoint = "ROUND_3", sceneName = "Version3" } },
        { 4, new InterfaceVersionData { uiVersion = 4, returnPoint = "SPLIT_ENDING", sceneName = "Version4" } }
    };

    public static InterfaceVersionData GetVersionData(int version)
    {
        if (interfaceVersions.TryGetValue(version, out var data))
            return data;

        Debug.LogWarning($"Unknown UI Version: {version}");
        return null;
    }
}
