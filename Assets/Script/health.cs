using UnityEngine;
using TMPro;
using System.Collections;

public class CharacterHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Is this a player or enemy?")]
    public bool isPlayer = true;

    [Header("Death Settings")]
    public float deathAnimationDuration = 2f; // Match this to your death animation length

    private bool isDead = false;
    private ShieldController shieldController;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        shieldController = GetComponent<ShieldController>();
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        if (shieldController != null && shieldController.IsShielded())
        {
            Debug.Log(gameObject.name + " blocked damage with shield!");
            return;
        }

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        BattleManager.Instance.OnCharacterDamaged(this);

        if (currentHealth <= 0)
            StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        isDead = true;

        // Play death animation
        if (animator != null && HasParameter("Death"))
            animator.SetTrigger("Death");

        // Disable ability controller and AI immediately so no more attacks
        GuyPearceAbilityController ctrl = GetComponent<GuyPearceAbilityController>();
        if (ctrl != null) ctrl.enabled = false;

        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai != null) ai.StopAttacking();

        // Wait for death animation to finish
        yield return new WaitForSeconds(deathAnimationDuration);

        // Now notify BattleManager to switch characters
        BattleManager.Instance.OnCharacterDied(this);
    }

    bool HasParameter(string paramName)
    {
        if (animator == null) return false;
        foreach (AnimatorControllerParameter param in animator.parameters)
            if (param.name == paramName) return true;
        return false;
    }

    public bool IsDead() => isDead;
}