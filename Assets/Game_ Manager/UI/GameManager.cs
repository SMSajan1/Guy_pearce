using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<CharacterType> selectedCharacters = new List<CharacterType>();
    public List<Sprite> selectedSprites = new List<Sprite>(); // NEW
    public const int MaxTeamSize = 3;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool SelectCharacter(CharacterType type, Sprite sprite = null)
    {
        if (selectedCharacters.Contains(type))
        {
            int index = selectedCharacters.IndexOf(type);
            selectedCharacters.RemoveAt(index);
            selectedSprites.RemoveAt(index);
            return false;
        }

        if (selectedCharacters.Count >= MaxTeamSize) return false;

        selectedCharacters.Add(type);
        selectedSprites.Add(sprite); // NEW
        return true;
    }

    public bool IsSelected(CharacterType type) => selectedCharacters.Contains(type);
    public bool IsTeamFull() => selectedCharacters.Count >= MaxTeamSize;
}