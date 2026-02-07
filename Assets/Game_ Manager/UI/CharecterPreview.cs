using UnityEngine;
using UnityEngine.UI;

public class CharacterPreviewUI : MonoBehaviour
{
    public Image rightPreviewImage;

    public Sprite phelsumSprite;
    public Sprite oroboroSprite;
    public Sprite carakaraSprite;
    public Sprite cerciSprite;

    public void ShowPreview(CharacterType type)
    {
        switch (type)
        {
            case CharacterType.Phelsum:
                rightPreviewImage.sprite = phelsumSprite;
                break;

            case CharacterType.oroboro:
                rightPreviewImage.sprite = oroboroSprite;
                break;

            case CharacterType.carakara:
                rightPreviewImage.sprite = carakaraSprite;
                break;

            case CharacterType.cerci:
                rightPreviewImage.sprite = cerciSprite;
                break;
        }
    }
}
