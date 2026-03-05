using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public string gameSceneName = "GameScene";

    public void PlayGame()
    {
        if (GameManager.Instance.selectedCharacters.Count < GameManager.MaxTeamSize)
        {
            Debug.Log("Select 3 characters first!");
            return;
        }
        SceneManager.LoadScene(gameSceneName);
    }
}