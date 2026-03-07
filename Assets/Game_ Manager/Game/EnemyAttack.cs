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

    void Start()
    {
        abilityController = GetComponent<GuyPearceAbilityController>();
        if (abilityController != null)
            abilityController.isPlayer = false;

        StartCoroutine(AutoAttackLoop());
    }

    public void SetTarget(CharacterHealth target)
    {
        targetHealth = target;
        if (abilityController != null)
            abilityController.currentOpponentHealth = target;
    }

    IEnumerator AutoAttackLoop()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            float waitTime = Random.Range(minAttackInterval, maxAttackInterval);
            yield return new WaitForSeconds(waitTime);

            if (abilityController != null && targetHealth != null)
            {
                string randomKey = abilityKeys[Random.Range(0, abilityKeys.Length)];
                abilityController.TriggerAbility(randomKey);
            }
        }
    }
}