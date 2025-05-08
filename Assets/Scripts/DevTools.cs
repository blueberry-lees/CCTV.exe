using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    [Header("Enable this only in dev/test builds")]
    public bool enableResetKey = true;

    [Tooltip("Key to press for clearing all saved data")]
    public KeyCode resetKey = KeyCode.R;

    void Update()
    {
        if (enableResetKey && Input.GetKeyDown(resetKey))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.LogWarning("PlayerPrefs have been reset.");
        }
    }
}