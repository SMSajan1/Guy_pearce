using UnityEngine;
using UnityEngine.UI;

public class CharacterUIManager : MonoBehaviour
{
    [Header("Right Side Preview Image")]
    public Image rightPreviewImage;

    public void ShowCharacter(Sprite characterSprite)
    {
        if (rightPreviewImage != null)
        {
            rightPreviewImage.sprite = characterSprite;
        }
    }
}
