using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public enum Turn { Player, Enemy }
    public Turn currentTurn = Turn.Player;

    void Awake()
    {
        Instance = this;
    }

    public bool IsPlayerTurn() => currentTurn == Turn.Player;
    public bool IsEnemyTurn() => currentTurn == Turn.Enemy;

    public void SetPlayerTurn()
    {
        currentTurn = Turn.Player;
        Debug.Log("--- PLAYER TURN ---");
    }

    public void SetEnemyTurn()
    {
        currentTurn = Turn.Enemy;
        Debug.Log("--- ENEMY TURN ---");
    }

    //public void SetPlayerTurn()
    //{
    //    currentTurn = Turn.Player;
    //    Debug.Log("--- PLAYER TURN SET --- from: " + new System.Diagnostics.StackTrace().ToString());
    //}

    //public void SetEnemyTurn()
    //{
    //    currentTurn = Turn.Enemy;
    //    Debug.Log("--- ENEMY TURN SET --- from: " + new System.Diagnostics.StackTrace().ToString());
    //}

}