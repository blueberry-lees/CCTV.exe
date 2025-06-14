using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFullScreen : MonoBehaviour
{
    private bool isFullscreen = false;
    private int targetWidth = 720;
    private int targetHeight = 720;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            ToggleFullscreen();
        }
    }

    void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;

        if (isFullscreen)
        {
            // Go fullscreen at current monitor resolution but maintain the 1:1 aspect
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

            // Set a resolution with the 1:1 ratio and black bars around it
            Screen.SetResolution(targetWidth, targetHeight, true);
        }
        else
        {
            // Return to windowed mode at 720x720
            Screen.SetResolution(targetWidth, targetHeight, false);
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

}
