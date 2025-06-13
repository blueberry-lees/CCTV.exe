using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFullScreen : MonoBehaviour
{
    public void FullScreen()
    {
        
            if (Screen.fullScreen)
            {
                Screen.SetResolution(512, 512, false);
            }
            else
            {
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            }
        }
    
}
