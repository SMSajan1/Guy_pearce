using UnityEngine;
using UnityEngine.UI;

public class CharacterUIButton : MonoBehaviour
{
    public CharacterType characterType;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        GameManager.Instance.SelectCharacter(characterType);
    }
}
