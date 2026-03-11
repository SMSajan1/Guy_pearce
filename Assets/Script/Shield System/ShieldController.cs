using UnityEngine;
using System.Collections;

public class ShieldController : MonoBehaviour
{
    [Header("Shield Settings")]
    public float shieldDuration = 3f;
    public float shieldCooldown = 8f;

    [Header("Shield Prefab")]
    public GameObject shieldPrefab; // Drag your shield prefab from Assets here

    [Header("Shield Offset (position relative to character)")]
    public Vector3 shieldOffset = Vector3.zero;

    private bool isShielded = false;
    private bool onCooldown = false;

    private GameObject spawnedShield; // The actual instance in the scene
    private GuyPearceAbilityController abilityController;

    void Start()
    {
        abilityController = GetComponent<GuyPearceAbilityController>();

        // Spawn the shield instance, attach it to this character, hide it
        if (shieldPrefab != null)
        {
            spawnedShield = Instantiate(shieldPrefab, transform.position + shieldOffset, transform.rotation);
            spawnedShield.transform.SetParent(transform); // attach to character
            spawnedShield.transform.localPosition = shieldOffset;
            spawnedShield.SetActive(false); // hide until activated
        }
    }

    void Update()
    {
        if (abilityController == null || !abilityController.isPlayer) return;

        if (Input.GetKeyDown(KeyCode.Space) && !isShielded && !onCooldown)
            StartCoroutine(ActivateShield());
    }

    IEnumerator ActivateShield()
    {
        isShielded = true;
        onCooldown = true;

        // Show shield
        if (spawnedShield != null)
            spawnedShield.SetActive(true);

        // Notify ShieldUI
        if (ShieldUI.Instance != null)
            ShieldUI.Instance.OnShieldActivated(shieldDuration, shieldCooldown);

        yield return new WaitForSeconds(shieldDuration);

        // Hide shield
        isShielded = false;
        if (spawnedShield != null)
            spawnedShield.SetActive(false);

        // Wait cooldown
        yield return new WaitForSeconds(shieldCooldown);
        onCooldown = false;
    }

    public bool IsShielded() => isShielded;
}