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

    [Header("Button References")]
    public TMP_Text[] buttons;  // Array of button texts

    private int currentIndex = 0;  // Current index for navigation between the launch, delete and exit buttons


    [Header("Error Popup")]
    public Image errorPopupText;  // Reference to the error popup

    [Header("Exit Popup")]
    public GameObject exitPopupPanel;  // Exit popup panel
    public TMP_Text[] yesOrNo;  // Yes/No text options for exit

    private float exitInputDelay = 0.2f;  // Delay for exit input
    private float exitPanelOpenedTime = 0f;  // Time when exit panel was opened
    private int exitChoiceIndex = 0;  // Current choice for the exit (Yes/No)

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
        // Initial setup for buttons and panels
        HighlightButton(0);
        errorPopupText.gameObject.SetActive(false);  // Hide error popup initially
        exitPopupPanel.gameObject.SetActive(false);  // Hide exit popup initially
        launchPopupPanel.gameObject.SetActive(false);  // Hide launch popup initially
        launchComfirmation.gameObject.SetActive(false); //HIDE launch comfirmation initially
    }

    void Update()
    {
        // Handle input for main panel navigation (not other panels)
        if (!isOtherPanelActive)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) Navigate(1);
            if (Input.GetKeyDown(KeyCode.LeftArrow)) Navigate(-1);
            if (Input.GetKeyDown(KeyCode.Return)) ActivateButton(currentIndex);
        }

        // Handle input for exit panel
        if (isOtherPanelActive && isExitPanelActive)
        {
            if (Time.time - exitPanelOpenedTime >= exitInputDelay)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow)) Choice(1);
                if (Input.GetKeyDown(KeyCode.DownArrow)) Choice(-1);
                if (Input.GetKeyDown(KeyCode.Return)) ActivateExitChoice(exitChoiceIndex);
            }
        }

        if (isOtherPanelActive && isLaunchComfirmationActive)
        {
            HideLaunchPopup();
        }

    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    // Navigation and Activation Methods
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────

    void Navigate(int dir)
    {
        // Navigate through buttons
        typeSound.Play();
        buttons[currentIndex].color = normalColor;
        currentIndex = (currentIndex + dir + buttons.Length) % buttons.Length;
        HighlightButton(currentIndex);
    }

    void Choice(int dir)
    {
        // Navigate through Yes/No choices
        typeSound.Play();
        yesOrNo[exitChoiceIndex].color = normalColor;
        exitChoiceIndex = (exitChoiceIndex + dir + yesOrNo.Length) % yesOrNo.Length;
        yesOrNo[exitChoiceIndex].color = highlightColor;
    }

    void HighlightButton(int index)
    {
        // Highlight selected button
        buttons[index].color = highlightColor;
    }

    void ActivateButton(int index)
    {
        // Activate the selected button and show corresponding popup
        typeSound.Play();
        switch (index)
        {
            case 0: ShowLaunchPopup(); break;
            case 1: ShowErrorPopup(); break;
            case 2: ShowExitPopup(); break;
        }
    }

    void ActivateExitChoice(int index)
    {
        // Handle exit choice (Yes/No)
        typeSound.Play();
        switch (exitChoiceIndex)
        {
            case 0: Application.Quit(); break;  // Exit the application
            case 1: HideExitPopup(); isOtherPanelActive = false; break;  // Don't exit
        }
    }

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
        // Show exit confirmation popup
        typeSound.Play();
        isOtherPanelActive = true;
        isExitPanelActive = true;
        exitPopupPanel.SetActive(true);
        exitChoiceIndex = 0;

        for (int i = 0; i < yesOrNo.Length; i++) yesOrNo[i].color = normalColor;
        yesOrNo[exitChoiceIndex].color = highlightColor;

        exitPanelOpenedTime = Time.time;  // Mark time when exit panel opened
    }

    public void HideExitPopup()
    {
        // Hide exit popup
        exitPopupPanel.gameObject.SetActive(false);
    }

    public void ShowLaunchPopup()
    {
        // Show launch popup
        typeSound.Play();
        isOtherPanelActive = true;
        isExitPanelActive = false;
        launchPopupPanel.SetActive(true);
        //logIndex = 0;

        //for (int i = 0; i < logs.Length; i++) logs[i].color = normalColor;
        //logs[logIndex].color = highlightColor;

        //launchPanelOpenedTime = Time.time;  // Mark time when launch panel opened
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
