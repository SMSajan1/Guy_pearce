using UnityEngine;
using UnityEngine.UI;

public class EffortBar : MonoBehaviour
{
    public static EffortBar Instance;

    [Header("UI")]
    public Slider effortSlider;

    [Header("Settings")]
    public float maxEffort = 100f;
    public float fillRatePerSecond = 4f; // fills 100% in 25 seconds

    private float currentEffort = 0f;

    // Effort costs per ability
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

    // Returns true if enough effort and deducts it
    public bool TryUseAbility(string key)
    {
        float cost = GetCost(key);

        if (currentEffort < cost)
        {
            Debug.Log($"Not enough effort for {key}! Need {cost}%, have {currentEffort:F1}%");
            return false;
        }

        currentEffort -= cost;
        effortSlider.value = currentEffort;
        return true;
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