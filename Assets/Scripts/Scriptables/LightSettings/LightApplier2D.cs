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

    [Header("Auto-Fetched Light References")]
    public Light2D spotlight;
    public Light2D globalLight;
    public Volume globalVolume;

    private LightSettings2D lightSettings;

    void Awake()
    {
        // Auto-assign from children if not already set
        if (spotlight == null || globalLight == null || globalVolume == null)
        {
            Light2D[] light2Ds = GetComponentsInChildren<Light2D>(true);
            foreach (var light in light2Ds)
            {
                if (light.lightType == Light2D.LightType.Point && spotlight == null)
                    spotlight = light;
                else if (light.lightType == Light2D.LightType.Global && globalLight == null)
                    globalLight = light;
            }

            if (globalVolume == null)
                globalVolume = GetComponentInChildren<Volume>(true);
        }

        if (spotlight == null)
            Debug.LogWarning("Spotlight (Point Light2D) not found as child of LightApplier2D.");
        if (globalLight == null)
            Debug.LogWarning("Global Light2D not found as child of LightApplier2D.");
        if (globalVolume == null)
            Debug.LogWarning("Global Volume not found as child of LightApplier2D.");
    }

    void Start()
    {
        string returnPoint = GameState.returnPoint;

        lightSettings = null;
        LightSettings2D defaultSettings = null;

        foreach (var entry in settingsList)
        {
            if (entry.returnPointName == returnPoint)
            {
                lightSettings = entry.settingsAsset;
                break;
            }

            if (entry.returnPointName == "") // fallback default
            {
                defaultSettings = entry.settingsAsset;
            }
        }

        if (lightSettings == null && defaultSettings != null)
        {
            Debug.LogWarning($"No exact LightSettings2D found for ReturnPoint: \"{returnPoint}\". Using default.");
            lightSettings = defaultSettings;
        }

        if (lightSettings == null)
        {
            Debug.LogWarning($"No LightSettings2D found for ReturnPoint: \"{returnPoint}\", and no default provided.");
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
