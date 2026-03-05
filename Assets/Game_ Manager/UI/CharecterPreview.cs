using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterPreviewUI : MonoBehaviour
{
    [Header("Main Large Preview")]
    public CharacterPreviewAnimator mainPreview;

    [Header("3 Team Slot Images")]
    public Image slotPreview1;
    public Image slotPreview2;
    public Image slotPreview3;

    [Header("Character Sprites")]
    public Sprite phelsumSprite;
    public Sprite oroboroSprite;
    public Sprite carakaraSprite;
    public Sprite cerciSprite;
    public Sprite mbengaSprite;
    public Sprite ryuudeSprite;

    void Start()
    {
        // Clear all slots on start
        ClearSlot(slotPreview1);
        ClearSlot(slotPreview2);
        ClearSlot(slotPreview3);
    }

    public void ShowMainPreview(CharacterType type)
    {
        if (mainPreview != null)
            mainPreview.ChangeSpriteAnimated(GetSprite(type));
    }

    public void RefreshPreviews()
    {
        List<CharacterType> selected = GameManager.Instance.selectedCharacters;
        Image[] slots = { slotPreview1, slotPreview2, slotPreview3 };

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) continue;

            if (i < selected.Count)
            {
                slots[i].sprite = GetSprite(selected[i]);
                slots[i].color = Color.white; // make visible
            }
            else
            {
                ClearSlot(slots[i]);
            }
        }
    }

    void ClearSlot(Image slot)
    {
        if (slot == null) return;
        slot.sprite = null;
        slot.color = new Color(1, 1, 1, 0); // invisible
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