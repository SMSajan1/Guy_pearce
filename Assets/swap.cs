using UnityEngine;

public class SwapButton : MonoBehaviour
{
    // Set this to 0, 1, or 2 in Inspector for each button
    public int characterIndex;

    public void OnClick()
    {
        BattleManager.Instance.SwapToPlayer(characterIndex);
    }
}