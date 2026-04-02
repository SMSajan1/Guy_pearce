using UnityEngine;

public class CharacterHitReactions : MonoBehaviour
{
    [Header("Hit Trigger Names in this character's Animator")]
    public string Hit_1 = "Hit_1";
    public string Hit_2 = "Hit_2";
    public string Hit_3 = "Hit_3";
    public string Hit_4 = "Hit_4";
    public string Hit_5 = "Hit_5";

    public string GetTrigger(string defaultTrigger)
    {
        switch (defaultTrigger)
        {
            case "Hit_1": return Hit_1;
            case "Hit_2": return Hit_2;
            case "Hit_3": return Hit_3;
            case "Hit_4": return Hit_4;
            case "Hit_5": return Hit_5;
            default: return defaultTrigger;
        }
    }
}