using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("Player UI")]
    public Slider playerHealthSlider;
    public TextMeshProUGUI playerNameText;

    [Header("Enemy UI")]
    public Slider enemyHealthSlider;
    public TextMeshProUGUI enemyNameText;

    [Header("Spawn Points")]
    public Transform playerRingSpawn;
    public Transform enemyRingSpawn;
    public Transform playerBenchSpawn1;
    public Transform playerBenchSpawn2;
    public Transform enemyBenchSpawn1;
    public Transform enemyBenchSpawn2;

    [HideInInspector] public List<CharacterHealth> playerTeam = new List<CharacterHealth>();
    [HideInInspector] public List<CharacterHealth> enemyTeam = new List<CharacterHealth>();

    private CharacterHealth activePlayer;
    private CharacterHealth activeEnemy;

    private int playerIndex = 0;
    private int enemyIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    public void InitTeams(List<CharacterHealth> players, List<CharacterHealth> enemies)
    {
        playerTeam = players;
        enemyTeam = enemies;

        PositionBench();

        SetActivePlayer(0);
        SetActiveEnemy(0);
    }

    void PositionBench()
    {
        Transform[] playerBenches = { playerBenchSpawn1, playerBenchSpawn2 };
        Transform[] enemyBenches = { enemyBenchSpawn1, enemyBenchSpawn2 };

        for (int i = 1; i < playerTeam.Count; i++)
            playerTeam[i].transform.position = playerBenches[i - 1].position;

        for (int i = 1; i < enemyTeam.Count; i++)
            enemyTeam[i].transform.position = enemyBenches[i - 1].position;
    }

    void SetActivePlayer(int index)
    {
        if (index >= playerTeam.Count) return;

        activePlayer = playerTeam[index];
        activePlayer.transform.position = playerRingSpawn.position;

        playerNameText.text = activePlayer.gameObject.name;
        playerHealthSlider.maxValue = activePlayer.maxHealth;
        playerHealthSlider.value = activePlayer.currentHealth;

        // Link this player's ability controller to the current active enemy
        GuyPearceAbilityController ctrl = activePlayer.GetComponent<GuyPearceAbilityController>();
        if (ctrl != null && activeEnemy != null)
            ctrl.currentOpponentHealth = activeEnemy;

        // Re-register shield controller for new active player
        ShieldController shield = activePlayer.GetComponent<ShieldController>();
        // Shield is already on the character, nothing extra needed —
        // ShieldUI will be triggered by the new character automatically

    }

    void SetActiveEnemy(int index)
    {
        if (index >= enemyTeam.Count) return;

        activeEnemy = enemyTeam[index];
        activeEnemy.transform.position = enemyRingSpawn.position;

        enemyNameText.text = activeEnemy.gameObject.name;
        enemyHealthSlider.maxValue = activeEnemy.maxHealth;
        enemyHealthSlider.value = activeEnemy.currentHealth;

        // Link enemy ability controller to current active player
        GuyPearceAbilityController ctrl = activeEnemy.GetComponent<GuyPearceAbilityController>();
        if (ctrl != null)
            ctrl.currentOpponentHealth = activePlayer;

        EnemyAI ai = activeEnemy.GetComponent<EnemyAI>();
        if (ai != null)
            ai.SetTarget(activePlayer);
    }

    public void DamageActiveEnemy(float amount)
    {
        if (activeEnemy != null)
            activeEnemy.TakeDamage(amount);
    }

    public void DamageActivePlayer(float amount)
    {
        if (activePlayer != null)
            activePlayer.TakeDamage(amount);
    }

    public void OnCharacterDamaged(CharacterHealth character)
    {
        if (character == activePlayer)
            playerHealthSlider.value = character.currentHealth;
        else if (character == activeEnemy)
            enemyHealthSlider.value = character.currentHealth;
    }

    public void OnCharacterDied(CharacterHealth character)
    {
        if (character == activePlayer)
        {
            Destroy(activePlayer.gameObject);
            activePlayer = null;

            playerIndex++;

            if (playerIndex >= playerTeam.Count)
            {
                Debug.Log("GAME OVER — Enemy Wins!");
                return;
            }

            // Bring next player into ring
            SetActivePlayer(playerIndex);

            // Re-link player's ability controller to current enemy
            GuyPearceAbilityController ctrl = activePlayer.GetComponent<GuyPearceAbilityController>();
            if (ctrl != null)
                ctrl.currentOpponentHealth = activeEnemy;

            // *** Tell EnemyAI to target the NEW player ***
            if (activeEnemy != null)
            {
                EnemyAI ai = activeEnemy.GetComponent<EnemyAI>();
                if (ai != null)
                    ai.SetTarget(activePlayer);

                GuyPearceAbilityController enemyCtrl = activeEnemy.GetComponent<GuyPearceAbilityController>();
                if (enemyCtrl != null)
                    enemyCtrl.currentOpponentHealth = activePlayer;
            }
        }
        else if (character == activeEnemy)
        {
            Destroy(activeEnemy.gameObject);
            activeEnemy = null;

            enemyIndex++;

            if (enemyIndex >= enemyTeam.Count)
            {
                Debug.Log("GAME OVER — Player Wins!");
                return;
            }

            // Bring next enemy into ring
            SetActiveEnemy(enemyIndex);

            // Re-link current player to new enemy
            if (activePlayer != null)
            {
                GuyPearceAbilityController ctrl = activePlayer.GetComponent<GuyPearceAbilityController>();
                if (ctrl != null)
                    ctrl.currentOpponentHealth = activeEnemy;
            }
        }
    }
}