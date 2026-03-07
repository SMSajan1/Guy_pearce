using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EffortBar : MonoBehaviour
{
    public static EffortBar Instance;

    [Header("UI")]
    public Slider effortSlider;
    public Image fillImage; // The fill Image of the slider

    [Header("Settings")]
    public float maxEffort = 100f;
    public float fillRatePerSecond = 4f;

    [Header("Feedback Settings")]
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 8f;
    public Color flashColor = new Color(1f, 0.2f, 0.2f, 1f); // red
    public float flashDuration = 0.3f;

    private float currentEffort = 0f;
    private Color originalFillColor;
    private Vector3 originalSliderPos;
    private bool isShaking = false;

    public const float Q_Cost = 5f;
    public const float E_Cost = 10f;
    public const float A_Cost = 15f;
    public const float D_Cost = 40f;
    public const float R_Cost = 60f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentEffort = 0f;
        effortSlider.maxValue = maxEffort;
        effortSlider.value = 0f;

        // Save originals for reset after feedback
        if (fillImage != null)
            originalFillColor = fillImage.color;

        originalSliderPos = effortSlider.transform.localPosition;
    }

    void Update()
    {
        if (currentEffort < maxEffort)
        {
            currentEffort += fillRatePerSecond * Time.deltaTime;
            currentEffort = Mathf.Clamp(currentEffort, 0f, maxEffort);
            effortSlider.value = currentEffort;
        }
    }

    public bool TryUseAbility(string key)
    {
        float cost = GetCost(key);

        if (currentEffort < cost)
        {
            Debug.Log($"Not enough effort for {key}! Need {cost}%, have {currentEffort:F1}%");
            TriggerFeedback();
            return false;
        }

        currentEffort -= cost;
        effortSlider.value = currentEffort;
        return true;
    }

    void TriggerFeedback()
    {
        if (!isShaking)
            StartCoroutine(ShakeSlider());

        StartCoroutine(FlashFill());
    }

    IEnumerator ShakeSlider()
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
            effortSlider.transform.localPosition = originalSliderPos + new Vector3(offsetX, 0f, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset position
        effortSlider.transform.localPosition = originalSliderPos;
        isShaking = false;
    }

    IEnumerator FlashFill()
    {
        if (fillImage == null) yield break;

        // Flash to red
        fillImage.color = flashColor;
        yield return new WaitForSeconds(flashDuration);

        // Fade back to original color
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            fillImage.color = Color.Lerp(flashColor, originalFillColor, elapsed / flashDuration);
            yield return null;
        }

        fillImage.color = originalFillColor;
    }

    float GetCost(string key)
    {
        switch (key)
        {
            case "Q": return Q_Cost;
            case "E": return E_Cost;
            case "A": return A_Cost;
            case "D": return D_Cost;
            case "R": return R_Cost;
            default: return 0f;
        }
    }

    public float GetCurrentEffort() => currentEffort;
    public float GetMaxEffort() => maxEffort;
}