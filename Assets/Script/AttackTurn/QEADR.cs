using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityButtonUI : MonoBehaviour
{
    [Header("Which ability key this represents")]
    public string abilityKey;

    [Header("UI")]
    public Image buttonImage;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color dimmedColor = new Color(0.3f, 0.3f, 0.3f, 1f);
    public Color pressedColor = new Color(1f, 1f, 0.4f, 1f);

    [Header("Flash Settings")]
    public float flashDuration = 0.15f;

    private float abilityCost;
    private bool wasReady = true;

    void Start()
    {
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();

        abilityCost = GetCost(abilityKey);

        // Set initial state
        buttonImage.color = dimmedColor;
    }

    void Update()
    {
        float currentEffort = EffortBar.Instance.GetCurrentEffort();
        bool isReady = currentEffort >= abilityCost;

        if (isReady != wasReady)
        {
            buttonImage.color = isReady ? normalColor : dimmedColor;
            wasReady = isReady;
        }

        // Detect key press and flash
        if (isReady && Input.GetKeyDown(GetKeyCode(abilityKey)))
            StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        buttonImage.color = pressedColor;
        yield return new WaitForSeconds(flashDuration);

        float currentEffort = EffortBar.Instance.GetCurrentEffort();
        buttonImage.color = currentEffort >= abilityCost ? normalColor : dimmedColor;
    }

    float GetCost(string key)
    {
        switch (key)
        {
            case "Q": return EffortBar.Q_Cost;
            case "E": return EffortBar.E_Cost;
            case "A": return EffortBar.A_Cost;
            case "D": return EffortBar.D_Cost;
            case "R": return EffortBar.R_Cost;
            default: return 0f;
        }
    }

    KeyCode GetKeyCode(string key)
    {
        switch (key)
        {
            case "Q": return KeyCode.Q;
            case "E": return KeyCode.E;
            case "A": return KeyCode.A;
            case "D": return KeyCode.D;
            case "R": return KeyCode.R;
            default: return KeyCode.None;
        }
    }
}