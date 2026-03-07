using UnityEngine;
using System.Collections.Generic;

public class PlayerActivator : MonoBehaviour
{
    [Header("Character Prefabs")]
    public GameObject phelsumPrefab;
    public GameObject oroboroPrefab;
    public GameObject carakaraPrefab;
    public GameObject cerciPrefab;
    public GameObject mbengaPrefab;
    public GameObject ryuudePrefab;

    [Header("Player Spawn Points")]
    public Transform playerSpawn1;
    public Transform playerSpawn2;
    public Transform playerSpawn3;

    [Header("Opponent Spawn Points")]
    public Transform opponentSpawn1;
    public Transform opponentSpawn2;
    public Transform opponentSpawn3;

    void Start()
    {
        SpawnTeams();
    }

    void SpawnTeams()
    {
        List<CharacterType> playerTeam = GameManager.Instance.selectedCharacters;
        List<CharacterType> opponentTeam = GetOpponentTeam(playerTeam);

        Transform[] playerSpawns = { playerSpawn1, playerSpawn2, playerSpawn3 };
        Transform[] opponentSpawns = { opponentSpawn1, opponentSpawn2, opponentSpawn3 };

        List<CharacterHealth> spawnedPlayers = new List<CharacterHealth>();
        List<CharacterHealth> spawnedEnemies = new List<CharacterHealth>();

        // Spawn player team
        for (int i = 0; i < playerTeam.Count; i++)
        {
            GameObject prefab = GetPrefab(playerTeam[i]);
            if (prefab != null)
            {
                GameObject go = Instantiate(prefab, playerSpawns[i].position, playerSpawns[i].rotation);

                GuyPearceAbilityController ctrl = go.GetComponent<GuyPearceAbilityController>();
                if (ctrl != null) ctrl.isPlayer = true;

                EnemyAI ai = go.GetComponent<EnemyAI>();
                if (ai != null) ai.enabled = false;

                CharacterHealth health = go.GetComponent<CharacterHealth>();
                if (health != null)
                {
                    health.isPlayer = true;
                    spawnedPlayers.Add(health);
                }
            }
        }

        // Spawn opponent team
        for (int i = 0; i < opponentTeam.Count; i++)
        {
            GameObject prefab = GetPrefab(opponentTeam[i]);
            if (prefab != null)
            {
                GameObject go = Instantiate(prefab, opponentSpawns[i].position, opponentSpawns[i].rotation);

                GuyPearceAbilityController ctrl = go.GetComponent<GuyPearceAbilityController>();
                if (ctrl != null) ctrl.isPlayer = false;

                EnemyAI ai = go.GetComponent<EnemyAI>();
                if (ai != null) ai.enabled = true;

                CharacterHealth health = go.GetComponent<CharacterHealth>();
                if (health != null)
                {
                    health.isPlayer = false;
                    spawnedEnemies.Add(health);
                }
            }
        }

        // Hand teams to BattleManager
        BattleManager.Instance.InitTeams(spawnedPlayers, spawnedEnemies);
    }

    List<CharacterType> GetOpponentTeam(List<CharacterType> playerTeam)
    {
        List<CharacterType> all = new List<CharacterType>
        {
            CharacterType.Phelsum, CharacterType.oroboro, CharacterType.carakara,
            CharacterType.cerci,   CharacterType.mbenga,  CharacterType.ryuude
        };

        List<CharacterType> opponents = new List<CharacterType>();
        foreach (CharacterType c in all)
            if (!playerTeam.Contains(c)) opponents.Add(c);

        return opponents;
    }

    GameObject GetPrefab(CharacterType type)
    {
        switch (type)
        {
            case CharacterType.Phelsum: return phelsumPrefab;
            case CharacterType.oroboro: return oroboroPrefab;
            case CharacterType.carakara: return carakaraPrefab;
            case CharacterType.cerci: return cerciPrefab;
            case CharacterType.mbenga: return mbengaPrefab;
            case CharacterType.ryuude: return ryuudePrefab;
            default: return null;
        }
    }
}