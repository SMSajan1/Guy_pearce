using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterPreviewUI : MonoBehaviour
{
    [Header("3 Preview Slots (assign in order: slot 1, 2, 3)")]
    public CharacterPreviewAnimator[] previewSlots; // size 3

    [Header("Character Sprites")]
    public Sprite phelsumSprite;
    public Sprite oroboroSprite;
    public Sprite carakaraSprite;
    public Sprite cerciSprite;
    public Sprite mbengaSprite;
    public Sprite ryuudeSprite;

    [Header("Empty Slot Sprite (optional)")]
    public Sprite emptySlotSprite;

    public void RefreshPreviews()
    {
        List<CharacterType> selected = GameManager.Instance.selectedCharacters;

        for (int i = 0; i < previewSlots.Length; i++)
        {
            if (i < selected.Count)
                previewSlots[i].ChangeSpriteAnimated(GetSprite(selected[i]));
            else
                previewSlots[i].ChangeSpriteAnimated(emptySlotSprite);
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
            default: return emptySlotSprite;
        }
    }
}