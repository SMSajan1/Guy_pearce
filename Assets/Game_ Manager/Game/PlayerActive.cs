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

    [Header("Spawn Points (3 positions)")]
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;

    void Start()
    {
        SpawnSelectedCharacters();
    }

    void SpawnSelectedCharacters()
    {
        List<CharacterType> team = GameManager.Instance.selectedCharacters;
        Transform[] spawnPoints = { spawnPoint1, spawnPoint2, spawnPoint3 };

        for (int i = 0; i < team.Count; i++)
        {
            GameObject prefab = GetPrefab(team[i]);
            if (prefab != null)
                Instantiate(prefab, spawnPoints[i].position, spawnPoints[i].rotation);
        }
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