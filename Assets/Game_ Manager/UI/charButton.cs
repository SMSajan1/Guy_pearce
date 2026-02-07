using UnityEngine;
using UnityEngine.UI;

public class CharacterUIButton : MonoBehaviour
{
    public CharacterType characterType;
    public CharacterPreviewUI previewUI;   // NEW

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        // Save for Game Scene
        GameManager.Instance.SelectCharacter(characterType);

        // Update UI Preview
        previewUI.ShowPreview(characterType);
    }
}
