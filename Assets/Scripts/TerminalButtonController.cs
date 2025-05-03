using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TerminalButtonController : MonoBehaviour
{
    [Header("Buttons")]
    public TMP_Text[] buttons;
    private int currentIndex = 0;
    public Image errorPopupText;         // Reference to the error popup 

    [Header("Exit")]
    public TMP_Text[] yesOrNo;
    public GameObject exitPopupPanel;
    private int exitChoiceIndex = 0;
    //delay choice panel selectable time
    private float exitInputDelay = 0.2f;  // delay in seconds
    private float exitPanelOpenedTime = 0f;

    [Header("Launch")]
    public TMP_Text[] logs;
    public GameObject launchPopupPanel;
    private int logIndex = 0;
    //delay lauch panel selectable time
    private float launchInputDelay = 0.2f;  // delay in seconds
    private float launchPanelOpenedTime = 0f;



    public AudioSource typeSound;


    public Color normalColor = Color.green;
    public Color highlightColor = Color.white;

    public bool isOtherPanelActive = false; //to check if any other panel except the main 3 are open
    public bool isExitPanelActive = false;

    void Start()
    {
        HighlightButton(0);
        errorPopupText.gameObject.SetActive(false);  // Ensure it's off initially
        exitPopupPanel.gameObject.SetActive(false);
        launchPopupPanel.gameObject.SetActive(false);

    }

    void Update()
    {
        if(isOtherPanelActive == false)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Navigate(1);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Navigate(-1);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                ActivateButton(currentIndex);
            }
        }

        if (isOtherPanelActive && isExitPanelActive)
        {
            if (Time.time - exitPanelOpenedTime >= exitInputDelay) // only allow input after delay
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Choice(1);
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    Choice(-1);
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    ActivateExitChoice(exitChoiceIndex); // use correct index
                }
            }
        }

        if (isOtherPanelActive && !isExitPanelActive)
        {
            if (Time.time - launchPanelOpenedTime >= launchInputDelay)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    NavigateLog(-1);
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    NavigateLog(1);
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    ActivateLog(logIndex);
                }
            }
        }

    }

    void Navigate(int dir) //launch, delete, exit buttons
    {
        typeSound.Play();
        buttons[currentIndex].color = normalColor;
        currentIndex = (currentIndex + dir + buttons.Length) % buttons.Length;
        HighlightButton(currentIndex);
    }

    void Choice(int dir) //Yes or no button
    {
        typeSound.Play();
        yesOrNo[exitChoiceIndex].color = normalColor;
        exitChoiceIndex = (exitChoiceIndex + dir + yesOrNo.Length) % yesOrNo.Length;
        yesOrNo[exitChoiceIndex].color = highlightColor;
    }

    void HighlightButton(int index)
    {
        buttons[index].color = highlightColor;
    }

    void ActivateButton(int index)
    {
        typeSound.Play();
        switch (index)
        {
            case 0: // Launch review
                Debug.Log("Opening launch popup...");
                ShowLaunchPopup();
                break;
            case 1: // Delete (error popup)
                Debug.Log("Attempting deletion...");
                ShowErrorPopup();
                break;
            case 2: // Exit
                Debug.Log("Attempting exit...");
                ShowExitPopup();
                break;
        }
    }

    void ActivateExitChoice(int index)
    {
        typeSound.Play();
        switch (exitChoiceIndex)
        {
            case 0: // Yes to exit
                Debug.Log("Exit");
                Application.Quit();
                break;
            case 1: // No to exit
                Debug.Log("Don't exit");
                HideExitPopup();
                isOtherPanelActive = false;
                break;
        }
    }

    void ShowErrorPopup()
    {
        typeSound.Play();

        errorPopupText.gameObject.SetActive(true);
        Invoke("HideErrorPopup", 4f);  // Hide after 4 seconds
    }

    void HideErrorPopup()
    {
        errorPopupText.gameObject.SetActive(false);  // Hide the error message
    }

    void ShowExitPopup()
    {
        typeSound.Play();
        isOtherPanelActive = true;
        isExitPanelActive = true;
        exitPopupPanel.SetActive(true);

        exitChoiceIndex = 0;
        for (int i = 0; i < yesOrNo.Length; i++)
            yesOrNo[i].color = normalColor;
        yesOrNo[exitChoiceIndex].color = highlightColor;

        exitPanelOpenedTime = Time.time;  // mark the time panel opened
    }

    void HideExitPopup()
    {
        exitPopupPanel.gameObject.SetActive(false);  // Hide the error message
    }

    void ShowLaunchPopup()
    {
        typeSound.Play();
        isOtherPanelActive = true;
        isExitPanelActive = false;
        launchPopupPanel.SetActive(true);

        logIndex = 0;
        for (int i = 0; i < logs.Length; i++)
            logs[i].color = normalColor;
        logs[logIndex].color = highlightColor;

        launchPanelOpenedTime = Time.time;
    }

    void HideLaunchPopup()
    {
        launchPopupPanel.SetActive(false);
        isOtherPanelActive = false;
    }

    void NavigateLog(int dir)
    {
        typeSound.Play();
        logs[logIndex].color = normalColor;
        logIndex = (logIndex + dir + logs.Length) % logs.Length;
        logs[logIndex].color = highlightColor;
    }

    void ActivateLog(int index)
    {
        typeSound.Play();

        switch (index)
        {
            case 0: // Corrupted log, always available
                Debug.Log("Launching Corrupted Log...");
                SceneManager.LoadScene("Round_1");
                break;
            case 1: // Log 2 – unlock after first round
                if (PlayerPrefs.GetInt("Round_1_Completed", 0) == 1)
                {
                    Debug.Log("Launching Log 2...");
                    SceneManager.LoadScene("Round_2");
                }
                else
                {
                    Debug.Log("Log 2 locked.");
                    ShowErrorPopup(); // reuse the error popup
                }
                break;
            case 2: // Log 3 – unlock after second round
                if (PlayerPrefs.GetInt("Round_2_Completed", 0) == 1)
                {
                    Debug.Log("Launching Log 3...");
                    SceneManager.LoadScene("Round_3");
                }
                else
                {
                    Debug.Log("Log 3 locked.");
                    ShowErrorPopup();
                }
                break;
        }
    }

}
