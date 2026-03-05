using UnityEngine;
using UnityEngine.UI;

public class CharacterUIButton : MonoBehaviour
{
    public CharacterType characterType;
    public CharacterPreviewUI previewUI;

    [Header("Selection Highlight")]
    public GameObject selectedHighlight;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        // Always show this character in the main large preview
        previewUI.ShowMainPreview(characterType);

        // Select or deselect
        bool isNowSelected = GameManager.Instance.SelectCharacter(characterType);

        // Update highlight
        if (selectedHighlight != null)
            selectedHighlight.SetActive(isNowSelected);

        // Refresh the 3 slot images
        previewUI.RefreshPreviews();
    }
}