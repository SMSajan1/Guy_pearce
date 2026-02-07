using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CharacterType selectedCharacter = CharacterType.None;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SelectCharacter(CharacterType type)
    {
        selectedCharacter = type;
        Debug.Log("Selected Character: " + type);
    }
}

