using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Is this a player or enemy?")]
    public bool isPlayer = true;

    private bool isDead = false;
    private ShieldController shieldController;

    void Start()
    {
        currentHealth = maxHealth;
        shieldController = GetComponent<ShieldController>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        // Block damage if shield is active
        if (shieldController != null && shieldController.IsShielded())
        {
            Debug.Log(gameObject.name + " blocked damage with shield!");
            return;
        }

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        BattleManager.Instance.OnCharacterDamaged(this);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        BattleManager.Instance.OnCharacterDied(this);
    }

    public bool IsDead() => isDead;
}