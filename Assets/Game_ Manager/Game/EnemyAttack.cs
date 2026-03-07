using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Attack Timing")]
    public float minAttackInterval = 1.5f;
    public float maxAttackInterval = 3.5f;

    private GuyPearceAbilityController abilityController;
    private string[] abilityKeys = { "Q", "E", "A", "D", "R" };

    void Start()
    {
        abilityController = GetComponent<GuyPearceAbilityController>();

        if (abilityController == null)
        {
            Debug.LogWarning("EnemyAI: No GuyPearceAbilityController found on " + gameObject.name);
            return;
        }

        // Make sure this is never treated as a player
        abilityController.isPlayer = false;

        StartCoroutine(AutoAttackLoop());
    }

    IEnumerator AutoAttackLoop()
    {
        // Small delay before first attack so game has time to load
        yield return new WaitForSeconds(2f);

        while (true)
        {
            // Wait random interval between attacks
            float waitTime = Random.Range(minAttackInterval, maxAttackInterval);
            yield return new WaitForSeconds(waitTime);

            // Pick a random ability key
            string randomKey = abilityKeys[Random.Range(0, abilityKeys.Length)];
            abilityController.TriggerAbility(randomKey);
        }
    }
}