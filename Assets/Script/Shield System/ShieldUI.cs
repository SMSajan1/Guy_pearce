using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShieldUI : MonoBehaviour
{
    public static ShieldUI Instance;

    [Header("UI")]
    public Slider shieldCooldownSlider; // Shows cooldown progress
    public Image shieldIcon;            // Shield icon, greys out on cooldown

    [Header("Colors")]
    public Color readyColor = Color.cyan;
    public Color activeColor = Color.white;
    public Color cooldownColor = new Color(0.4f, 0.4f, 0.4f, 1f);

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (shieldCooldownSlider != null)
        {
            shieldCooldownSlider.maxValue = 1f;
            shieldCooldownSlider.value = 1f; // Full = ready
        }

        if (shieldIcon != null)
            shieldIcon.color = readyColor;
    }

    public void OnShieldActivated(float shieldDuration, float shieldCooldown)
    {
        StartCoroutine(ShieldUIRoutine(shieldDuration, shieldCooldown));
    }

    IEnumerator ShieldUIRoutine(float shieldDuration, float shieldCooldown)
    {
        float totalTime = shieldDuration + shieldCooldown;

        // Phase 1: Shield active — show as bright
        if (shieldIcon != null)
            shieldIcon.color = activeColor;

        float elapsed = 0f;
        while (elapsed < shieldDuration)
        {
            elapsed += Time.deltaTime;
            // Slider stays full while shield is active
            if (shieldCooldownSlider != null)
                shieldCooldownSlider.value = 1f;
            yield return null;
        }

        // Phase 2: Cooldown — drain the slider
        if (shieldIcon != null)
            shieldIcon.color = cooldownColor;

        elapsed = 0f;
        while (elapsed < shieldCooldown)
        {
            elapsed += Time.deltaTime;
            if (shieldCooldownSlider != null)
                shieldCooldownSlider.value = 1f - (elapsed / shieldCooldown);
            yield return null;
        }


        // Ready again
        if (shieldCooldownSlider != null)
            shieldCooldownSlider.value = 1f;

        if (shieldIcon != null)
            shieldIcon.color = readyColor;
    }


    public void ResetUI()
    {
        StopAllCoroutines();
        if (shieldCooldownSlider != null)
            shieldCooldownSlider.value = 1f;
        if (shieldIcon != null)
            shieldIcon.color = readyColor;
    }


}