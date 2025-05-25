using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TerminalButtonController : MonoBehaviour
{

    public UserInterface userInterface;

    [Header("Main Panel")]
    public GameObject mainPanel;


    [Header("Error Popup")]
    public Image errorPopupText;  // Reference to the error popup

    [Header("Exit Popup")]
    public GameObject exitPopupPanel;  // Exit popup panel
  

    [Header("Launch Popup")]
    public GameObject launchPopupPanel;  // Launch popup panel

    [Header("Launch Comfirmation")]
    public GameObject launchComfirmation; //comfirm launch panel


    [Header("Audio")]
    public AudioSource popupSound;  // Typing sound effect


    
    private bool isOtherPanelActive = false;  // To track if other panels are active
    private bool isErrorPopupActive = false;  // To track if error popup is active


    void Start()
    {
        userInterface = GetComponent<UserInterface>();
        errorPopupText.gameObject.SetActive(false);  // Hide error popup initially
        exitPopupPanel.gameObject.SetActive(false);  // Hide exit popup initially
        launchPopupPanel.gameObject.SetActive(false);  // Hide launch popup initially
        launchComfirmation.gameObject.SetActive(false); //HIDE launch comfirmation initially
        mainPanel.gameObject.SetActive(false);
    }

    void Update()
    {


        if (userInterface.isTypingFinished && Input.GetKeyDown(KeyCode.Escape))
        {
            ShowExitPopup();
        }

        // Handle main panel and other panel availability
        if (isOtherPanelActive )
        {
            mainPanel.gameObject.SetActive(false);
        }
        else if (userInterface.isTypingFinished)
        {
            mainPanel.gameObject.SetActive(true);
        }

    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    // Methods
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────

    public void ShowErrorPopup()
    {
        // Show error popup and wait for key press to hide
        popupSound.Play();
        errorPopupText.gameObject.SetActive(true);
        if (!isErrorPopupActive) StartCoroutine(WaitForAnyKeyToHideError());
    }

    public void HideErrorPopup()
    {
        // Hide the error popup
        errorPopupText.gameObject.SetActive(false);
        isErrorPopupActive = false;
    }

    public void ShowReplayPopup()
    {
        popupSound.Play();
        isOtherPanelActive = true;
        launchComfirmation.SetActive(true);
    }

    public void HideReplayPopup()
    {
        popupSound.Play();
        isOtherPanelActive = false;
        launchComfirmation.SetActive(false);
    }

    IEnumerator WaitForAnyKeyToHideError()
    {
        // Wait for any key to hide the error popup
        isErrorPopupActive = true;
        yield return null;

        while (!Input.anyKeyDown) yield return null;

        HideErrorPopup();
    }

    public void ShowExitPopup()
    {
        isOtherPanelActive = true; 
        exitPopupPanel.SetActive(true);
    }

    public void HideExitPopup()
    {
        // Hide exit popup
        isOtherPanelActive = false;
        exitPopupPanel.gameObject.SetActive(false);

    }

    public void ShowLaunchPopup()
    {
        // Show launch popup
        isOtherPanelActive = true;
        launchPopupPanel.SetActive(true);
       
    }

    public void HideLaunchPopup()
    {
        // Hide launch popup
        launchPopupPanel.SetActive(false);
        isOtherPanelActive = false;
    }


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
