using UnityEngine;
using UnityEngine.UI;

public class CharacterUIButton : MonoBehaviour
{
    public Sprite characterSprite;                 // Image for right preview
    public CharacterUIManager uiManager;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        uiManager.ShowCharacter(characterSprite);
    }
}
