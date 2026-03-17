using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButtonUI : MonoBehaviour
{
    [Header("Which ability key this represents")]
    public string abilityKey;

    [Header("UI")]
    public Image buttonImage;
    public TextMeshProUGUI abilityNameText; // The TMP text for this button

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color dimmedColor = new Color(0.3f, 0.3f, 0.3f, 1f);
    public Color pressedColor = new Color(1f, 1f, 0.4f, 1f);

    [Header("Flash Settings")]
    public float flashDuration = 0.15f;

    [Header("Scale Animation")]
    public float scaleUpSize = 1.3f;  // How big it gets
    public float scaleDuration = 0.1f;  // How fast it scales up
    public float holdDuration = 0.2f;  // How long it stays big
    public float scaleDownDuration = 0.15f; // How fast it returns

    [Header("Ability Names per Character")]
    public string PHELSUM_Name;
    public string OROBORO_Name;
    public string CARAKARA_Name;
    public string CERCI_Name;
    public string MBENGA_Name;
    public string RYUUDE_Name;

    private float abilityCost;
    private bool wasReady = true;
    private Vector3 originalScale;

    void Start()
    {
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();

        abilityCost = GetCost(abilityKey);
        originalScale = transform.localScale;

        buttonImage.color = dimmedColor;

        // Hide ability name text on start
        if (abilityNameText != null)
            abilityNameText.text = "";
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

        if (isReady && Input.GetKeyDown(GetKeyCode(abilityKey)))
        {
            StartCoroutine(FlashRoutine());
            StartCoroutine(ScaleRoutine());
            ShowAbilityName();
        }
    }

    void ShowAbilityName()
    {
        if (abilityNameText == null) return;

        // Get the active player's character type from BattleManager
        // We find the active player's GuyPearceAbilityController
        GuyPearceAbilityController ctrl = FindActivePlayerController();
        if (ctrl == null) return;

        string name = GetAbilityName(ctrl.characterType);
        StartCoroutine(DisplayNameRoutine(name));
    }

    GuyPearceAbilityController FindActivePlayerController()
    {
        // Find all ability controllers in scene and return the active player one
        GuyPearceAbilityController[] controllers = FindObjectsOfType<GuyPearceAbilityController>();
        foreach (GuyPearceAbilityController ctrl in controllers)
        {
            if (ctrl.isPlayer && ctrl.enabled)
                return ctrl;
        }
        return null;
    }

    string GetAbilityName(GuyPearceAbilityController.CharacterType type)
    {
        switch (type)
        {
            case GuyPearceAbilityController.CharacterType.PHELSUM: return PHELSUM_Name;
            case GuyPearceAbilityController.CharacterType.OROBORO: return OROBORO_Name;
            case GuyPearceAbilityController.CharacterType.CARAKARA: return CARAKARA_Name;
            case GuyPearceAbilityController.CharacterType.CERCI: return CERCI_Name;
            case GuyPearceAbilityController.CharacterType.MBENGA: return MBENGA_Name;
            case GuyPearceAbilityController.CharacterType.RYUUDE: return RYUUDE_Name;
            default: return "";
        }
    }

    IEnumerator DisplayNameRoutine(string name)
    {
        abilityNameText.text = name;

        // Fade in
        abilityNameText.alpha = 0f;
        float elapsed = 0f;
        while (elapsed < 0.1f)
        {
            elapsed += Time.deltaTime;
            abilityNameText.alpha = Mathf.Lerp(0f, 1f, elapsed / 0.1f);
            yield return null;
        }
        abilityNameText.alpha = 1f;

        // Hold for 2 seconds
        yield return new WaitForSeconds(2f);

        // Fade out
        elapsed = 0f;
        while (elapsed < 0.3f)
        {
            elapsed += Time.deltaTime;
            abilityNameText.alpha = Mathf.Lerp(1f, 0f, elapsed / 0.3f);
            yield return null;
        }

        abilityNameText.alpha = 0f;
        abilityNameText.text = "";
    }

    IEnumerator ScaleRoutine()
    {
        // Scale UP
        float elapsed = 0f;
        while (elapsed < scaleDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / scaleDuration;
            transform.localScale = Vector3.Lerp(originalScale, originalScale * scaleUpSize, t);
            yield return null;
        }
        transform.localScale = originalScale * scaleUpSize;

        // Hold big
        yield return new WaitForSeconds(holdDuration);

        // Scale DOWN
        elapsed = 0f;
        while (elapsed < scaleDownDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / scaleDownDuration;
            transform.localScale = Vector3.Lerp(originalScale * scaleUpSize, originalScale, t);
            yield return null;
        }
        transform.localScale = originalScale;
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
