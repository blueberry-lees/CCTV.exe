using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TerminalIntro : MonoBehaviour
{
   

    void Update()
    {
      if (Input.anyKeyDown)
        {
            this.gameObject.SetActive(false);  // Load the next scene
        }
    }


}
