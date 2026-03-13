using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        // Bench player characters — disable their controllers
        for (int i = 1; i < playerTeam.Count; i++)
        {
            if (playerTeam[i] != null && !playerTeam[i].IsDead())
            {
                playerTeam[i].transform.position = benchSpawns[i - 1].position;
                DisableCharacter(playerTeam[i].gameObject, isEnemy: false);
            }
        }

        Transform[] enemyBenches = { enemyBenchSpawn1, enemyBenchSpawn2 };
        for (int i = 1; i < enemyTeam.Count; i++)
        {
            if (enemyTeam[i] != null)
            {
                enemyTeam[i].transform.position = enemyBenches[i - 1].position;
                DisableCharacter(enemyTeam[i].gameObject, isEnemy: true);
            }
        }
    }

    // Disables ability controller and AI on a character
    // Add to DisableCharacter method
    void DisableCharacter(GameObject go, bool isEnemy)
    {
        GuyPearceAbilityController ctrl = go.GetComponent<GuyPearceAbilityController>();
        if (ctrl != null) ctrl.enabled = false;

        if (isEnemy)
        {
            EnemyAI ai = go.GetComponent<EnemyAI>();
            if (ai != null)
            {
                ai.StopAttacking(); // stop the loop when disabled
                ai.enabled = false;
            }
        }
    }

    // Enables ability controller and AI on a character
    void EnableCharacter(GameObject go, bool isEnemy)
    {
        GuyPearceAbilityController ctrl = go.GetComponent<GuyPearceAbilityController>();
        if (ctrl != null) ctrl.enabled = true;

        if (isEnemy)
        {
            EnemyAI ai = go.GetComponent<EnemyAI>();
            if (ai != null) ai.enabled = true;
        }
    }

    void SetActivePlayer(int index)
    {
        if (index >= playerTeam.Count) return;
        if (playerTeam[index] == null || playerTeam[index].IsDead()) return;

        activePlayer = playerTeam[index];
        playerIndex = index;

        activePlayer.transform.position = playerRingSpawn.position;
        EnableCharacter(activePlayer.gameObject, isEnemy: false);

        playerNameText.text = activePlayer.gameObject.name;
        playerHealthSlider.maxValue = activePlayer.maxHealth;
        playerHealthSlider.value = activePlayer.currentHealth;

        // Link player -> enemy hit points
        if (activeEnemy != null)
            LinkHitPoints(activePlayer, activeEnemy);
    }

    void SetActiveEnemy(int index)
    {
        if (index >= enemyTeam.Count) return;

        activeEnemy = enemyTeam[index];
        enemyIndex = index;

        activeEnemy.transform.position = enemyRingSpawn.position;
        EnableCharacter(activeEnemy.gameObject, isEnemy: true);

        enemyNameText.text = activeEnemy.gameObject.name;
        enemyHealthSlider.maxValue = activeEnemy.maxHealth;
        enemyHealthSlider.value = activeEnemy.currentHealth;

        if (activePlayer != null)
            LinkHitPoints(activeEnemy, activePlayer);

        if (activePlayer != null)
            LinkHitPoints(activePlayer, activeEnemy);

        // Only start attacking on the active enemy
        EnemyAI ai = activeEnemy.GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.SetTarget(activePlayer);
            ai.StartAttacking(); // explicitly start only this one
        }
    }

    public void SwapToPlayer(int index)
    {
        if (index == playerIndex) return;
        if (playerTeam[index] == null || playerTeam[index].IsDead())
        {
            Debug.Log("That character is dead!");
            return;
        }

        CharacterHealth oldPlayer = activePlayer;
        int oldIndex = playerIndex;

        if (oldPlayer != null)
        {
            DisableCharacter(oldPlayer.gameObject, isEnemy: false);
            Transform bench = GetBenchSpawnForIndex(oldIndex, index);
            oldPlayer.transform.position = bench.position;
        }

        SetActivePlayer(index);

        // Re-link hit points both ways after swap
        if (activeEnemy != null)
        {
            LinkHitPoints(activePlayer, activeEnemy);
            LinkHitPoints(activeEnemy, activePlayer);

            EnemyAI ai = activeEnemy.GetComponent<EnemyAI>();
            if (ai != null) ai.SetTarget(activePlayer);

            GuyPearceAbilityController enemyCtrl = activeEnemy.GetComponent<GuyPearceAbilityController>();
            if (enemyCtrl != null)
                enemyCtrl.currentOpponentHealth = activePlayer;
        }

        RefreshSwapButtons();
    }

    Transform GetBenchSpawnForIndex(int oldIndex, int newIndex)
    {
        List<int> benchIndices = new List<int>();
        for (int i = 0; i < playerTeam.Count; i++)
            if (i != newIndex) benchIndices.Add(i);

        int slot = benchIndices.IndexOf(oldIndex);
        return slot == 0 ? playerBenchSpawn1 : playerBenchSpawn2;
    }

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
        {
            activePlayer.TakeDamage(amount);

            if (CameraShake.Instance != null)
                StartCoroutine(DelayedShake(amount));
        }
    }

    IEnumerator DelayedShake(float amount)
    {
        // Delay to sync with animation/projectile reaching player
        yield return new WaitForSeconds(0.35f);

        if (amount >= 25f)
            CameraShake.Instance.Shake(0.6f, 5f, 3f);
        else if (amount >= 15f)
            CameraShake.Instance.Shake(0.45f, 3.5f, 2.5f);
        else if (amount >= 10f)
            CameraShake.Instance.Shake(0.3f, 2.5f, 2f);
        else if (amount >= 5f)
            CameraShake.Instance.Shake(0.2f, 1.5f, 1.5f);
        else
            CameraShake.Instance.Shake(0.15f, 1f, 1f);
    }

    public void OnCharacterDamaged(CharacterHealth character)
    {
        if (character == activePlayer)
            playerHealthSlider.value = character.currentHealth;
        else if (character == activeEnemy)
            enemyHealthSlider.value = character.currentHealth;
    }


    // Add this helper method to BattleManager
    void LinkHitPoints(CharacterHealth attacker, CharacterHealth defender)
    {
        if (attacker == null || defender == null) return;

        GuyPearceAbilityController ctrl = attacker.GetComponent<GuyPearceAbilityController>();
        if (ctrl == null) return;

        // Try multiple possible names in case prefabs are named differently
        Transform head = defender.transform.Find("HitPoint_Head")
                      ?? defender.transform.Find("HitPoint_head")
                      ?? defender.transform.Find("Head_HitPoint")
                      ?? defender.transform.Find("HitPointHead");

        Transform body = defender.transform.Find("HitPoint_Body")
                      ?? defender.transform.Find("HitPoint_body")
                      ?? defender.transform.Find("Body_HitPoint")
                      ?? defender.transform.Find("HitPointBody");

        if (head != null) ctrl.opponentHead = head;
        else Debug.LogWarning("HitPoint_Head not found on " + defender.gameObject.name + " — using root");

        if (body != null) ctrl.opponentBody = body;
        else Debug.LogWarning("HitPoint_Body not found on " + defender.gameObject.name + " — using root");

        Animator defenderAnimator = defender.GetComponent<Animator>();
        if (defenderAnimator != null)
            ctrl.opponentAnimator = defenderAnimator;
    }

    public void OnCharacterDied(CharacterHealth character)
    {
        if (character == activePlayer)
        {
            Destroy(activePlayer.gameObject);
            playerTeam[playerIndex] = null;
            activePlayer = null;

            int nextIndex = FindNextAlivePlayer();
            if (nextIndex == -1)
            {
                Debug.Log("GAME OVER — Enemy Wins!");
                return;
            }

            playerIndex = nextIndex;
            SetActivePlayer(nextIndex);

            // Re-link both ways
            if (activeEnemy != null)
            {
                LinkHitPoints(activePlayer, activeEnemy);
                LinkHitPoints(activeEnemy, activePlayer);

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

            // Re-link both ways
            if (activePlayer != null)
            {
                LinkHitPoints(activePlayer, activeEnemy);
                LinkHitPoints(activeEnemy, activePlayer);
            }
        }
    }
    int FindNextAlivePlayer()
    {
        for (int i = 0; i < playerTeam.Count; i++)
            if (playerTeam[i] != null && !playerTeam[i].IsDead()) return i;
        return -1;
    }




}