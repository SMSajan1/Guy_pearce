using UnityEngine;
using UnityEngine.UI;

public class CharacterPreviewUI : MonoBehaviour
{
    [Header("Preview Animator")]
    public CharacterPreviewAnimator previewAnimator;

    [Header("Character Sprites")]
    public Sprite phelsumSprite;
    public Sprite oroboroSprite;
    public Sprite carakaraSprite;
    public Sprite cerciSprite;

    public void ShowPreview(CharacterType type)
    {
        Sprite selectedSprite = null;

        switch (type)
        {
            case CharacterType.Phelsum:
                selectedSprite = phelsumSprite;
                break;

            case CharacterType.oroboro:
                selectedSprite = oroboroSprite;
                break;

            case CharacterType.carakara:
                selectedSprite = carakaraSprite;
                break;

            case CharacterType.cerci:
                selectedSprite = cerciSprite;
                break;
        }

        if (selectedSprite != null && previewAnimator != null)
        {
            previewAnimator.ChangeSpriteAnimated(selectedSprite);
        }
    }
}
