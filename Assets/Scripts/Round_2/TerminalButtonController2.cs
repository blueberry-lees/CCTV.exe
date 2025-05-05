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

    [Header("Error Popup")]
    public Image errorPopupText;  // Reference to the error popup

    [Header("Exit Popup")]
    public GameObject exitPopupPanel;  // Exit popup panel
    public TMP_Text[] yesOrNo;  // Yes/No text options for exit

    [Header("Launch Popup")]
    public GameObject launchPopupPanel;  // Launch popup panel
    public TMP_Text[] logs;  // Array of logs displayed in launch popup
    

    [Header("Audio")]
    public AudioSource typeSound;  // Typing sound effect

    [Header("Text Colors")]
    public Color normalColor = Color.green;  // Normal button color
    public Color highlightColor = Color.white;  // Highlighted button color

    

    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    // Private Fields
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────

    private int currentIndex = 0;  // Current index for navigation between the launch, delete and exit buttons
    
    
    private bool isOtherPanelActive = false;  // To track if other panels are active
    private bool isExitPanelActive = false;  // To track if exit panel is active
    private bool isErrorPopupActive = false;  // To track if error popup is active

    private float exitInputDelay = 0.2f;  // Delay for exit input
    private float exitPanelOpenedTime = 0f;  // Time when exit panel was opened
    private int exitChoiceIndex = 0;  // Current choice for the exit (Yes/No)

    private float launchInputDelay = 0.2f;  // Delay for launch panel input
    private float launchPanelOpenedTime = 0f;  // Time when launch panel was opened
    private int logIndex = 0;  // Current log index for navigation


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

        // Handle input for launch panel
        //if (isOtherPanelActive && !isExitPanelActive)
        //{
        //    if (Time.time - launchPanelOpenedTime >= launchInputDelay)
        //    {
        //        if (Input.GetKeyDown(KeyCode.UpArrow)) NavigateLog(-1);
        //        if (Input.GetKeyDown(KeyCode.DownArrow)) NavigateLog(1);
        //        if (Input.GetKeyDown(KeyCode.Return)) ActivateLog(logIndex);
        //    }
        //}
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
        logIndex = 0;

        for (int i = 0; i < logs.Length; i++) logs[i].color = normalColor;
        logs[logIndex].color = highlightColor;

        launchPanelOpenedTime = Time.time;  // Mark time when launch panel opened
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

    //void NavigateLog(int dir)
    //{
    //    // Navigate through logs in launch popup
    //    typeSound.Play();
    //    logs[logIndex].color = normalColor;
    //    logIndex = (logIndex + dir + logs.Length) % logs.Length;
    //    logs[logIndex].color = highlightColor;
    //}

    //void ActivateLog(int index)
    //{
    //    //Activate the selected log and load the corresponding scene
    //    typeSound.Play();
    //    switch (index)
    //    {
    //        case 0: ShowErrorPopup(); break;
    //        case 1: ShowErrorPopup(); break;
    //        case 2: ShowErrorPopup(); break;
    //        case 3: ShowErrorPopup(); break;
    //        case 4: SceneManager.LoadScene("Round_1"); break;
    //        case 5: HideLaunchPopup(); break;
    //    }
    //}


}
