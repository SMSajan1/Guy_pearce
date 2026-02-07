using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public string gameSceneName = "GameScene";

    public void PlayGame()
    {
        if (GameManager.Instance.selectedCharacter == CharacterType.None)
        {
            Debug.Log("Select Character First!");
            return;
        }

        SceneManager.LoadScene(gameSceneName);
    }
}
