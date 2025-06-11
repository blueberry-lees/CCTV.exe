using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "LightSettings2D", menuName = "Lighting/2D Light Settings")]
public class LightSettings2D : ScriptableObject
{
    [Header("Spotlight 2D Settings")]
    public float spotlightIntensity = 1f;
    public Color spotlightColor = Color.white;
    public float spotlightOuterAngle = 45f;
    public float spotlightInnerAngle = 25f;

    [Header("Global Light 2D Settings")]
    public float globalLightIntensity = 1f;
    public Color globalLightColor = Color.white;

    [Header("Global Volume Settings")]
    public Volume globalVolume;              // assign in inspector or runtime
    public VolumeProfile volumeProfile;      // assign the profile you want to set

    public void Apply(Light2D spotlight, Light2D globalLight)
    {
        if (spotlight != null && spotlight.lightType == Light2D.LightType.Point)
        {
            spotlight.intensity = spotlightIntensity;
            spotlight.color = spotlightColor;

            // Comment these if your URP version doesn't support them
            spotlight.pointLightOuterAngle = spotlightOuterAngle;
            spotlight.pointLightInnerAngle = spotlightInnerAngle;
        }

        if (globalLight != null && globalLight.lightType == Light2D.LightType.Global)
        {
            globalLight.intensity = globalLightIntensity;
            globalLight.color = globalLightColor;
        }

        // Apply the volume profile if set
        if (globalVolume != null && volumeProfile != null)
        {
            globalVolume.profile = volumeProfile;
        }
    }
}