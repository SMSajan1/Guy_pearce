using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<CharacterType> selectedCharacters = new List<CharacterType>();
    public const int MaxTeamSize = 3;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool SelectCharacter(CharacterType type)
    {
        // Deselect if already selected
        if (selectedCharacters.Contains(type))
        {
            selectedCharacters.Remove(type);
            return false; // returns false = deselected
        }

        // Don't allow more than 3
        if (selectedCharacters.Count >= MaxTeamSize) return false;

        selectedCharacters.Add(type);
        return true; // returns true = selected
    }

    public bool IsSelected(CharacterType type) => selectedCharacters.Contains(type);
    public bool IsTeamFull() => selectedCharacters.Count >= MaxTeamSize;
}