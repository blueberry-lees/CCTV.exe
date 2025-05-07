using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TerminalButtonController2 : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    // Public Fields (Inspector)
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    public UserInterface2 userInterface2;

    [Header("Main Panel")]
    public GameObject mainPanel;


    [Header("Error Popup")]
    public Image errorPopupText;  // Reference to the error popup

    [Header("Exit Popup")]
    public GameObject exitPopupPanel;  // Exit popup panel
   

    //private float exitInputDelay = 0.2f;  // Delay for exit input
    //private float exitPanelOpenedTime = 0f;  // Time when exit panel was opened
    //private int exitChoiceIndex = 0;  // Current choice for the exit (Yes/No)

    [Header("Launch Popup")]
    public GameObject launchPopupPanel;  // Launch popup panel

    [Header("Launch Comfirmation")]
    public GameObject launchComfirmation; //comfirm launch panel



    [Header("Audio")]
    public AudioSource typeSound;  // Typing sound effect

    [Header("Text Colors")]
    public Color normalColor = Color.white;  // Normal button color
    public Color highlightColor = Color.white;  // Highlighted button color

    

    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    // Booleans
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────

    
    private bool isOtherPanelActive = false;  // To track if other panels are active
    private bool isExitPanelActive = false;  // To track if exit panel is active
    private bool isErrorPopupActive = false;  // To track if error popup is active
    private bool isLaunchComfirmationActive = false;

    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    // Unity Methods
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────

    void Start()
    {
        userInterface2 = GetComponent<UserInterface2>();
        errorPopupText.gameObject.SetActive(false);  // Hide error popup initially
        exitPopupPanel.gameObject.SetActive(false);  // Hide exit popup initially
        launchPopupPanel.gameObject.SetActive(false);  // Hide launch popup initially
        launchComfirmation.gameObject.SetActive(false); //HIDE launch comfirmation initially
        mainPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        // Handle input for main panel navigation (not other panels)
        


        // Handle input for exit panel
        if (isOtherPanelActive )
        {
            mainPanel.gameObject.SetActive(false);
        }
        else if (userInterface2.isTypingFinished)
        {
            mainPanel.gameObject.SetActive(true);
        }


        if (isOtherPanelActive && isLaunchComfirmationActive)
        {
            HideLaunchPopup();
        }

    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    // Navigation and Activation Methods
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────

    //void Choice(int dir)
    //{
    //    // Navigate through Yes/No choices
    //    typeSound.Play();
    //    yesOrNo[exitChoiceIndex].color = normalColor;
    //    exitChoiceIndex = (exitChoiceIndex + dir + yesOrNo.Length) % yesOrNo.Length;
    //    yesOrNo[exitChoiceIndex].color = highlightColor;
    //}


    //void ActivateExitChoice(int index)
    //{
    //    // Handle exit choice (Yes/No)
    //    typeSound.Play();
    //    switch (exitChoiceIndex)
    //    {
    //        case 0: Application.Quit(); break;  // Exit the application
    //        case 1: HideExitPopup(); isOtherPanelActive = false; break;  // Don't exit
    //    }
    //}

    public void ShowErrorPopup()
    {
        // Show error popup and wait for key press to hide
        typeSound.Play();
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
        isLaunchComfirmationActive = true;
        typeSound.Play();
        isOtherPanelActive = true;
        launchComfirmation.SetActive(true);
    }

    public void HideReplayPopup()
    {
        isLaunchComfirmationActive = false;
        typeSound.Play();
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

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exiting game...");
    }

}
