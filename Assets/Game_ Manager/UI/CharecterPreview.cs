using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterPreviewUI : MonoBehaviour
{
    [Header("Main Large Preview (single image shown on hover/click)")]
    public CharacterPreviewAnimator mainPreview;

    [Header("3 Team Slot Previews (fills left to right as selected)")]
    public CharacterPreviewAnimator slotPreview1;
    public CharacterPreviewAnimator slotPreview2;
    public CharacterPreviewAnimator slotPreview3;

    [Header("Character Sprites")]
    public Sprite phelsumSprite;
    public Sprite oroboroSprite;
    public Sprite carakaraSprite;
    public Sprite cerciSprite;
    public Sprite mbengaSprite;
    public Sprite ryuudeSprite;

    // Called by CharacterUIButton when clicked
    public void ShowMainPreview(CharacterType type)
    {
        if (mainPreview != null)
            mainPreview.ChangeSpriteAnimated(GetSprite(type));
    }

    // Called after any selection change to refresh the 3 slots
    public void RefreshPreviews()
    {
        List<CharacterType> selected = GameManager.Instance.selectedCharacters;

        CharacterPreviewAnimator[] slots = { slotPreview1, slotPreview2, slotPreview3 };

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) continue;

            if (i < selected.Count)
                slots[i].ChangeSpriteAnimated(GetSprite(selected[i]));
            else
                slots[i].ChangeSpriteAnimated(null); // clears the slot
        }
    }

    Sprite GetSprite(CharacterType type)
    {
        switch (type)
        {
            case CharacterType.Phelsum: return phelsumSprite;
            case CharacterType.oroboro: return oroboroSprite;
            case CharacterType.carakara: return carakaraSprite;
            case CharacterType.cerci: return cerciSprite;
            case CharacterType.mbenga: return mbengaSprite;
            case CharacterType.ryuude: return ryuudeSprite;
            default: return null;
        }
    }
}