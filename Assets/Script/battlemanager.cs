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

    [Header("Swap Buttons (assign in order: slot 1, 2, 3)")]
    public GameObject swapButton1;
    public GameObject swapButton2;
    public GameObject swapButton3;

    [HideInInspector] public List<CharacterHealth> playerTeam = new List<CharacterHealth>();
    [HideInInspector] public List<CharacterHealth> enemyTeam = new List<CharacterHealth>();

    private CharacterHealth activePlayer;
    private CharacterHealth activeEnemy;

    private int playerIndex = 0;
    private int enemyIndex = 0;

    // Bench positions for quick lookup
    private Transform[] benchSpawns;

    void Awake()
    {
        Instance = this;
    }

    public void InitTeams(List<CharacterHealth> players, List<CharacterHealth> enemies)
    {
        playerTeam = players;
        enemyTeam = enemies;

        benchSpawns = new Transform[] { playerBenchSpawn1, playerBenchSpawn2 };

        PositionBench();
        SetActivePlayer(0);
        SetActiveEnemy(0);
        RefreshSwapButtons();
    }

    void PositionBench()
    {
        // Position all non-active players on bench
        for (int i = 1; i < playerTeam.Count; i++)
        {
            if (playerTeam[i] != null && !playerTeam[i].IsDead())
                playerTeam[i].transform.position = benchSpawns[i - 1].position;
        }

        Transform[] enemyBenches = { enemyBenchSpawn1, enemyBenchSpawn2 };
        for (int i = 1; i < enemyTeam.Count; i++)
        {
            if (enemyTeam[i] != null)
                enemyTeam[i].transform.position = enemyBenches[i - 1].position;
        }
    }

    void SetActivePlayer(int index)
    {
        if (index >= playerTeam.Count) return;
        if (playerTeam[index] == null || playerTeam[index].IsDead()) return;

        activePlayer = playerTeam[index];
        playerIndex = index;

        activePlayer.transform.position = playerRingSpawn.position;

        // Update UI with THIS character's current health (preserved)
        playerNameText.text = activePlayer.gameObject.name;
        playerHealthSlider.maxValue = activePlayer.maxHealth;
        playerHealthSlider.value = activePlayer.currentHealth;

        // Link ability controller to current enemy
        GuyPearceAbilityController ctrl = activePlayer.GetComponent<GuyPearceAbilityController>();
        if (ctrl != null && activeEnemy != null)
            ctrl.currentOpponentHealth = activeEnemy;
    }

    void SetActiveEnemy(int index)
    {
        if (index >= enemyTeam.Count) return;

        activeEnemy = enemyTeam[index];
        enemyIndex = index;

        activeEnemy.transform.position = enemyRingSpawn.position;

        enemyNameText.text = activeEnemy.gameObject.name;
        enemyHealthSlider.maxValue = activeEnemy.maxHealth;
        enemyHealthSlider.value = activeEnemy.currentHealth;

        GuyPearceAbilityController ctrl = activeEnemy.GetComponent<GuyPearceAbilityController>();
        if (ctrl != null)
            ctrl.currentOpponentHealth = activePlayer;

        EnemyAI ai = activeEnemy.GetComponent<EnemyAI>();
        if (ai != null)
            ai.SetTarget(activePlayer);
    }

    // Called by swap buttons — pass 0, 1, or 2
    public void SwapToPlayer(int index)
    {
        // Can't swap to same character
        if (index == playerIndex) return;

        // Can't swap to a dead character
        if (playerTeam[index] == null || playerTeam[index].IsDead())
        {
            Debug.Log("That character is dead!");
            return;
        }

        // Send current active player to bench
        int oldIndex = playerIndex;
        CharacterHealth oldPlayer = activePlayer;

        // Find which bench slot the new character was on
        // and send old player there
        int benchSlot = index - 1; // index 1 = bench 0, index 2 = bench 1
        if (index < oldIndex) benchSlot = oldIndex - 1;

        // Move old player to a bench position
        Transform targetBench = GetBenchSpawnForIndex(oldIndex, index);
        if (oldPlayer != null)
            oldPlayer.transform.position = targetBench.position;

        // Bring new player into ring
        SetActivePlayer(index);

        // Update enemy targets to new player
        if (activeEnemy != null)
        {
            EnemyAI ai = activeEnemy.GetComponent<EnemyAI>();
            if (ai != null) ai.SetTarget(activePlayer);

            GuyPearceAbilityController enemyCtrl = activeEnemy.GetComponent<GuyPearceAbilityController>();
            if (enemyCtrl != null) enemyCtrl.currentOpponentHealth = activePlayer;
        }

        RefreshSwapButtons();
    }

    // Figures out which bench position to send the old player to
    Transform GetBenchSpawnForIndex(int oldIndex, int newIndex)
    {
        // Collect bench slots not occupied by the incoming character
        List<int> benchIndices = new List<int>();
        for (int i = 0; i < playerTeam.Count; i++)
        {
            if (i != newIndex) benchIndices.Add(i);
        }

        int slot = benchIndices.IndexOf(oldIndex);
        return slot == 0 ? playerBenchSpawn1 : playerBenchSpawn2;
    }

    // Grey out the active character button and dead character buttons
    void RefreshSwapButtons()
    {
        GameObject[] buttons = { swapButton1, swapButton2, swapButton3 };

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == null) continue;

            SwapButton swapBtn = buttons[i].GetComponent<SwapButton>();
            if (swapBtn == null) continue;

            bool isDead = playerTeam[i] == null || playerTeam[i].IsDead();
            bool isActive = (i == playerIndex);

            swapBtn.SetState(isActive, isDead);
        }
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
            playerTeam[playerIndex] = null;
            activePlayer = null;

            // Find next alive player
            int nextIndex = FindNextAlivePlayer();

            if (nextIndex == -1)
            {
                Debug.Log("GAME OVER — Enemy Wins!");
                return;
            }

            playerIndex = nextIndex;
            SetActivePlayer(nextIndex);

            GuyPearceAbilityController ctrl = activePlayer.GetComponent<GuyPearceAbilityController>();
            if (ctrl != null) ctrl.currentOpponentHealth = activeEnemy;

            if (activeEnemy != null)
            {
                EnemyAI ai = activeEnemy.GetComponent<EnemyAI>();
                if (ai != null) ai.SetTarget(activePlayer);

                GuyPearceAbilityController enemyCtrl = activeEnemy.GetComponent<GuyPearceAbilityController>();
                if (enemyCtrl != null) enemyCtrl.currentOpponentHealth = activePlayer;
            }

            RefreshSwapButtons();
        }
        else if (character == activeEnemy)
        {
            Destroy(activeEnemy.gameObject);
            enemyTeam[enemyIndex] = null;
            activeEnemy = null;

            enemyIndex++;

            if (enemyIndex >= enemyTeam.Count)
            {
                Debug.Log("GAME OVER — Player Wins!");
                return;
            }

            SetActiveEnemy(enemyIndex);

            if (activePlayer != null)
            {
                GuyPearceAbilityController ctrl = activePlayer.GetComponent<GuyPearceAbilityController>();
                if (ctrl != null) ctrl.currentOpponentHealth = activeEnemy;
            }
        }
    }

    int FindNextAlivePlayer()
    {
        for (int i = 0; i < playerTeam.Count; i++)
        {
            if (playerTeam[i] != null && !playerTeam[i].IsDead())
                return i;
        }
        return -1;
    }
}