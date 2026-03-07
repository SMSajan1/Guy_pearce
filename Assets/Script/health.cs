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

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Notify BattleManager to update the UI
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