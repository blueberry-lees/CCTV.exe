using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class LightApplier2D : MonoBehaviour
{
    [System.Serializable]
    public class NamedLightSettings
    {
        public string returnPointName;
        public LightSettings2D settingsAsset;
    }

    [Header("Light Settings Options")]
    public List<NamedLightSettings> settingsList;

    [Header("Light References")]
    public Light2D spotlight;
    public Light2D globalLight;
    public Volume globalVolume; // Assign in inspector

    private LightSettings2D lightSettings;

    void Start()
    {
        string returnPoint = PlayerPrefs.GetString("ReturnPoint", "");

        foreach (var entry in settingsList)
        {
            if (entry.returnPointName == returnPoint)
            {
                lightSettings = entry.settingsAsset;
                break;
            }
        }

        if (lightSettings == null)
        {
            Debug.LogWarning($"No LightSettings2D found for ReturnPoint: \"{returnPoint}\"");
            return;
        }

        ApplyLightSettings();
    }

    public void ApplyLightSettings()
    {
        if (lightSettings != null)
        {
            lightSettings.globalVolume = globalVolume;
            lightSettings.Apply(spotlight, globalLight);
        }
    }
}
