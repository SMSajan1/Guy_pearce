using UnityEngine;
using UnityEngine.UI;

public class CharacterUIButton : MonoBehaviour
{
    public CharacterType characterType;
    public CharacterPreviewUI previewUI;

    [Header("Selection Highlight")]
    public GameObject selectedHighlight; // An outline/glow object on the button

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        bool isNowSelected = GameManager.Instance.SelectCharacter(characterType);

        // Update highlight
        if (selectedHighlight != null)
            selectedHighlight.SetActive(isNowSelected);

        // Update preview slots
        previewUI.RefreshPreviews();
    }
}