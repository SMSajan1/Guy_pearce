using UnityEngine;
using UnityEngine.UI;

public class SwapButton : MonoBehaviour
{
    public int characterIndex;

    [Header("UI")]
    public Image characterImage;        // The image on the button
    public Image dimOverlay;            // A dark overlay image (child of button)

    void Start()
    {
        // Load sprite from GameManager
        if (characterImage != null && characterIndex < GameManager.Instance.selectedSprites.Count)
        {
            Sprite sprite = GameManager.Instance.selectedSprites[characterIndex];
            if (sprite != null)
            {
                characterImage.sprite = sprite;
                characterImage.color = Color.white;
            }
        }

        // Hide dim overlay initially
        if (dimOverlay != null)
            dimOverlay.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        BattleManager.Instance.SwapToPlayer(characterIndex);
    }

    // Called by BattleManager to update visual state
    public void SetState(bool isActive, bool isDead)
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
            btn.interactable = !isActive && !isDead;

        // Show dim overlay if active or dead
        if (dimOverlay != null)
            dimOverlay.gameObject.SetActive(isActive || isDead);
    }
}