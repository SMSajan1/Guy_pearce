using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Attack Timing")]
    public float minAttackInterval = 1.5f;
    public float maxAttackInterval = 3.5f;

    private GuyPearceAbilityController abilityController;
    private CharacterHealth targetHealth;
    private string[] abilityKeys = { "Q", "E", "A", "D", "R" };
    private Coroutine attackCoroutine;

    void Start()
    {
        abilityController = GetComponent<GuyPearceAbilityController>();
        if (abilityController != null)
            abilityController.isPlayer = false;

        // Do NOT auto start — BattleManager will call StartAttacking()
    }

    public void SetTarget(CharacterHealth target)
    {
        targetHealth = target;
        if (abilityController != null)
            abilityController.currentOpponentHealth = target;
    }

    // Called by BattleManager when this enemy enters the ring
    public void StartAttacking()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        attackCoroutine = StartCoroutine(AutoAttackLoop());
    }

    // Called by BattleManager when this enemy leaves the ring
    public void StopAttacking()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    IEnumerator AutoAttackLoop()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            // Wait until it's the enemy's turn
            yield return new WaitUntil(() => TurnManager.Instance.IsEnemyTurn());

            // Small natural delay before attacking
            float waitTime = Random.Range(minAttackInterval, maxAttackInterval);
            yield return new WaitForSeconds(waitTime);

            // Double check it's still enemy turn and target is valid
            if (TurnManager.Instance.IsEnemyTurn() && abilityController != null && targetHealth != null)
            {
                string randomKey = abilityKeys[Random.Range(0, abilityKeys.Length)];
                abilityController.TriggerAbility(randomKey);
            }

            // Wait until ability finishes and turn returns to player
            yield return new WaitUntil(() => TurnManager.Instance.IsPlayerTurn());
        }
    }
}