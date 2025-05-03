using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text dialogueText;
    public TMP_Text speakerText;
    public Image portraitImage;
    public GameObject choicesContainer;
    public GameObject choicePrefab;

    [Header("Choice Colors")]
    public Color selectedChoiceColor = Color.yellow;
    public Color unselectedChoiceColor = Color.white;

    [Header("Audio")]
    public AudioSource typingAudio;
    public AudioSource sfxAudio;
    public TextAsset inkJSON;

    [Header("Typing Settings")]
    public float defaultSpeed = 0.03f;

    private Story story;
    private bool isTyping = false;
    private bool skipTyping = false;

    private string currentSpeaker = "Narrator";
    private Dictionary<string, AudioClip> speakerTypingSounds = new();
    private List<TMP_Text> currentChoices = new();
    private int selectedChoiceIndex = 0;


    private bool useShakeEffect = false;
    private bool useWaveEffect = false;
    private bool useJitterEffect = false;


    //define text color based on the speaker
    private Dictionary<string, string> speakerColors = new Dictionary<string, string>()
{
    { "Male", "#89CFF0" },     // light blue
    { "Female", "#FFB6C1" },   // light pink
    { "Narrator", "#FFFFFF" }  // white (default)
};


    // ───────────────────────────── Init ─────────────────────────────
    void Start()
    {
        story = new Story(inkJSON.text);
        LoadTypingSounds();
        ContinueStory();
    }

    void LoadTypingSounds()
    {
        string[] speakerTypes = { "Male", "Female", "Narrator" };

        foreach (string type in speakerTypes)
        {
            var clip = Resources.Load<AudioClip>($"Audio/Typing/{type}");
            if (clip != null)
                speakerTypingSounds[type.ToLower()] = clip;
        }
    }

    // ───────────────────────────── Update ─────────────────────────────
    void Update()
    {
        if (isTyping)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                skipTyping = true;
            return;
        }

        if (currentChoices.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) SelectNextChoice();
            else if (Input.GetKeyDown(KeyCode.DownArrow)) SelectPreviousChoice();
            else if (Input.GetKeyDown(KeyCode.Return)) ChooseSelectedChoice();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            ContinueStory();
    }

    // ───────────────────────────── Story Flow ─────────────────────────────
    void ContinueStory()
    {
        ClearChoices();
        ResetTextEffects();

        if (story.canContinue)
        {
            string text = story.Continue().Trim();
            HandleTags(story.currentTags);
            StartCoroutine(TypeText(text));
        }
        else
        {
            Debug.Log("End of story.");
        }
    }

    float GetFloatFromInk(string varName, float defaultValue)
    {
        var val = story.variablesState[varName];
        if (val is int i) return i;
        if (val is float f) return f;
        if (val is double d) return (float)d;
        return defaultValue;
    }

    // ───────────────────────────── Typing ─────────────────────────────
    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";
        skipTyping = false;

        float speed = GetFloatFromInk("speed", defaultSpeed);
        float delay = GetFloatFromInk("delay", 0f);
        yield return new WaitForSeconds(delay);

        foreach (char c in text)
        {
            if (skipTyping)
            {
                dialogueText.text = text;
                break;
            }

            dialogueText.text += c;

            if (typingAudio != null && speakerTypingSounds.TryGetValue(currentSpeaker.ToLower(), out AudioClip clip))
            {
                typingAudio.clip = clip;
                typingAudio.pitch = Random.Range(0.95f, 1.05f); // Slight pitch variation
                typingAudio.Play();
            }

            yield return new WaitForSeconds(speed);
        }

        isTyping = false;

        if (useWaveEffect) StartCoroutine(WaveText());
        if (useJitterEffect) StartCoroutine(JitterText());
        if (useShakeEffect) StartCoroutine(ShakeText());  // Trigger ShakeEffect if it's active

        if (story.currentChoices.Count > 0)
            DisplayChoices();
    }




    // ───────────────────────────── SFX ─────────────────────────────

    void PlaySFX(string clipName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/SFX/{clipName}");
        if (clip != null)
        {
            sfxAudio.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SFX not found: Audio/SFX/{clipName}");
        }
    }


    // ───────────────────────────── Choices ─────────────────────────────
    void DisplayChoices()
    {
        currentChoices.Clear();

        foreach (Choice choice in story.currentChoices)
        {
            GameObject choiceGO = Instantiate(choicePrefab, choicesContainer.transform);
            TMP_Text choiceText = choiceGO.GetComponent<TMP_Text>();
            choiceText.text = choice.text;
            currentChoices.Add(choiceText);
        }

        selectedChoiceIndex = 0;
        HighlightChoice();
    }

    void HighlightChoice()
    {
        for (int i = 0; i < currentChoices.Count; i++)
        {
            currentChoices[i].color = (i == selectedChoiceIndex) ? selectedChoiceColor : unselectedChoiceColor;
        }
    }

    void SelectNextChoice()
    {
        selectedChoiceIndex = (selectedChoiceIndex + 1) % currentChoices.Count;
        HighlightChoice();
    }

    void SelectPreviousChoice()
    {
        selectedChoiceIndex = (selectedChoiceIndex - 1 + currentChoices.Count) % currentChoices.Count;
        HighlightChoice();
    }

    void ChooseSelectedChoice()
    {
        PlaySFX("ChoiceSelect"); // Replace "ChoiceSelect" with your actual SFX name
        story.ChooseChoiceIndex(selectedChoiceIndex);
        ClearChoices();
        ContinueStory();
    }

    void ClearChoices()
    {
        foreach (Transform child in choicesContainer.transform)
            Destroy(child.gameObject);
        currentChoices.Clear();
    }

    // ───────────────────────────── Tags ─────────────────────────────
    void HandleTags(List<string> tags)
    {
        foreach (string tag in tags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2) continue;

            string key = splitTag[0].Trim().ToLower();
            string val = splitTag[1].Trim();

            switch (key)
            {
                case "speaker": //speaker of the dialogue
                    speakerText.text = val;
                    currentSpeaker = val;
                    break;
                case "expression"://character expression in dialogue
                    ChangeCharacterExpression(currentSpeaker, val);
                    break;
                case "effect": //text effect in dialogue
                    ApplyTextEffect(val);
                    break;
                case "sfx": //sound effect in dialogue
                    PlaySFX(val);
                    break;
            }
        }
    }

    void ChangeCharacterExpression(string speaker, string expression)
    {
        string spritePath = $"Portraits/{speaker}_{expression}";
        Sprite newPortrait = Resources.Load<Sprite>(spritePath);

        if (newPortrait != null)
            portraitImage.sprite = newPortrait;
        else
            Debug.LogWarning($"Portrait not found: {spritePath}");
    }


    // ───────────────────────────── Text effects ─────────────────────────────
    void ApplyTextEffect(string effect)
    {
        useWaveEffect = false;
        useJitterEffect = false;
        useShakeEffect = false;  // Add a flag for shake effect

        switch (effect.ToLower())
        {
            case "wave":
                useWaveEffect = true;
                break;
            case "jitter":
                useJitterEffect = true;
                break;
            case "shake":
                useShakeEffect = true; // Set shake effect to true
                break;
            default:
                Debug.LogWarning($"Unknown effect: {effect}");
                break;
        }
    }

    void ResetTextEffects()
    {
        StopAllCoroutines();                 // Stop any active coroutines (like wave/jitter)
        dialogueText.text = "";             // Clear the text
        dialogueText.ForceMeshUpdate();     // Force TMP to rebuild the mesh
    }

    IEnumerator ShakeText()
    {
        TMP_TextInfo textInfo = dialogueText.textInfo;
        float shakeAmount = 3f;
        float shakeSpeed = 10f;

        while (true)
        {
            dialogueText.ForceMeshUpdate();
            textInfo = dialogueText.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible) continue;

                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                Vector2 shake = new Vector2(Mathf.Sin(Time.time * shakeSpeed + i * 0.3f) * shakeAmount, Mathf.Cos(Time.time * shakeSpeed + i * 0.3f) * shakeAmount);
                for (int j = 0; j < 4; j++)
                    vertices[vertexIndex + j] += (Vector3)shake;
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                dialogueText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }

    IEnumerator WaveText()
    {
        TMP_TextInfo textInfo = dialogueText.textInfo;
        float waveSpeed = 2f;
        float waveHeight = 5f;

        while (true)
        {
            dialogueText.ForceMeshUpdate();
            textInfo = dialogueText.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible) continue;

                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                float offsetY = Mathf.Sin(Time.time * waveSpeed + i * 0.3f) * waveHeight;
                for (int j = 0; j < 4; j++)
                    vertices[vertexIndex + j].y += offsetY;
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                dialogueText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }

    IEnumerator JitterText()
    {
        TMP_TextInfo textInfo = dialogueText.textInfo;
        float jitterAmount = 1f;

        while (true)
        {
            dialogueText.ForceMeshUpdate();
            textInfo = dialogueText.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible) continue;

                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                Vector2 jitter = new Vector2(Random.Range(-jitterAmount, jitterAmount), Random.Range(-jitterAmount, jitterAmount));
                for (int j = 0; j < 4; j++)
                    vertices[vertexIndex + j] += (Vector3)jitter;
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                dialogueText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }
}

