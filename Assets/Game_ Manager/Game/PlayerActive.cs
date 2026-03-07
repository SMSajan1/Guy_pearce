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

    [Header("Player Spawn Points (3 positions)")]
    public Transform playerSpawn1;
    public Transform playerSpawn2;
    public Transform playerSpawn3;

    [Header("Opponent Spawn Points (3 positions)")]
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

        // Spawn player team
        for (int i = 0; i < playerTeam.Count; i++)
        {
            GameObject prefab = GetPrefab(playerTeam[i]);
            if (prefab != null)
            {
                GameObject go = Instantiate(prefab, playerSpawns[i].position, playerSpawns[i].rotation);

                // Set as player
                GuyPearceAbilityController controller = go.GetComponent<GuyPearceAbilityController>();
                if (controller != null)
                    controller.isPlayer = true;

                // Disable EnemyAI on player characters
                EnemyAI ai = go.GetComponent<EnemyAI>();
                if (ai != null)
                    ai.enabled = false;
            }
        }

        // Spawn opponent team
        for (int i = 0; i < opponentTeam.Count; i++)
        {
            GameObject prefab = GetPrefab(opponentTeam[i]);
            if (prefab != null)
            {
                GameObject go = Instantiate(prefab, opponentSpawns[i].position, opponentSpawns[i].rotation);

                // Set as opponent
                GuyPearceAbilityController controller = go.GetComponent<GuyPearceAbilityController>();
                if (controller != null)
                    controller.isPlayer = false;

                // Make sure EnemyAI is enabled on opponents
                EnemyAI ai = go.GetComponent<EnemyAI>();
                if (ai != null)
                    ai.enabled = true;
            }
        }
    }

    List<CharacterType> GetOpponentTeam(List<CharacterType> playerTeam)
    {
        List<CharacterType> allCharacters = new List<CharacterType>
        {
            CharacterType.Phelsum,
            CharacterType.oroboro,
            CharacterType.carakara,
            CharacterType.cerci,
            CharacterType.mbenga,
            CharacterType.ryuude
        };

        List<CharacterType> opponentTeam = new List<CharacterType>();
        foreach (CharacterType c in allCharacters)
        {
            if (!playerTeam.Contains(c))
                opponentTeam.Add(c);
        }

        return opponentTeam;
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