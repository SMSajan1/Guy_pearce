using UnityEngine;
using UnityEngine.UI;

public class CharacterUIButton : MonoBehaviour
{
    public CharacterType characterType;
    public CharacterPreviewUI previewUI;

    [Header("Selection Highlight")]
    public GameObject selectedHighlight;

    [Header("Character Sprite for this button")]
    public Sprite characterSprite; // Assign in Inspector

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        previewUI.ShowMainPreview(characterType);

        // Pass sprite along with character type
        bool isNowSelected = GameManager.Instance.SelectCharacter(characterType, characterSprite);

        if (selectedHighlight != null)
            selectedHighlight.SetActive(isNowSelected);

        previewUI.RefreshPreviews();
    }
}