using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TypewriterText : MonoBehaviour
{
    public TMP_Text textComponent;
    private string fullText;

    [Header("Typing Settings")]
    public float typeSpeed = 0.05f;
    public AudioSource typeSound;

    [Header("Shake Settings")]
    public bool enableShake = false;
    public float shakeIntensity = 0f;
    public float shakeSpeed = 0.02f;

    [Header("Blink Settings")]
    public bool enableBlink = false;
    public float blinkInterval = 0.5f;

    private bool skipTyping = false;
    private Coroutine shakeCoroutine;
    private Coroutine blinkCoroutine;
    private Vector2 originalAnchorPos;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        fullText = textComponent.text;
        originalAnchorPos = ((RectTransform)textComponent.transform).anchoredPosition;

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        skipTyping = false;
        
        textComponent.text = "";
        //fullText = textComponent.text; // this line make sure the refresh the text if the obj is disabled & then enabled
        canvasGroup.alpha = 1f;
        StopAllCoroutines();
        StartCoroutine(DelayedType());
    }

    private IEnumerator DelayedType()
    {
        yield return null; // Wait for layout

        originalAnchorPos = ((RectTransform)textComponent.transform).anchoredPosition;
        StartCoroutine(TypeText());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ((RectTransform)textComponent.transform).anchoredPosition = originalAnchorPos;
        canvasGroup.alpha = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skipTyping = true;
        }
    }

    IEnumerator TypeText()
    {
        int i = 0;
        while (i < fullText.Length)
        {
            if (skipTyping)
            {
                textComponent.text = fullText;
                break;
            }

            // Handle rich text tags
            if (fullText[i] == '<')
            {
                int tagEnd = fullText.IndexOf('>', i);
                if (tagEnd != -1)
                {
                    while (i <= tagEnd)
                    {
                        textComponent.text += fullText[i++];
                    }
                    continue;
                }
            }

            textComponent.text += fullText[i];

            if (!char.IsWhiteSpace(fullText[i]) && typeSound != null)
                typeSound.Play();

            i++;
            yield return new WaitForSeconds(typeSpeed);
        }

        if (enableShake)
            shakeCoroutine = StartCoroutine(ShakeText());
        if (enableBlink)
            blinkCoroutine = StartCoroutine(BlinkAlpha());
    }

    IEnumerator ShakeText()
    {
        RectTransform rt = (RectTransform)textComponent.transform;
        originalAnchorPos = rt.anchoredPosition;

        while (true)
        {
            float offsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float offsetY = Random.Range(-shakeIntensity, shakeIntensity);
            rt.anchoredPosition = originalAnchorPos + new Vector2(offsetX, offsetY);

            yield return new WaitForSeconds(shakeSpeed);
        }
    }

    IEnumerator BlinkAlpha()
    {
        while (true)
        {
            // Randomize blink interval (min and max duration between flashes)
            float randomInterval = Random.Range(blinkInterval * 0.5f, blinkInterval * 1.5f); // Adjust as needed

            // Toggle alpha instantly
            canvasGroup.alpha = 0f;
            yield return new WaitForSeconds(randomInterval / 2f);

            canvasGroup.alpha = 1f;
            yield return new WaitForSeconds(randomInterval / 2f);
        }
    }

}
