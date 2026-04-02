using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public enum Turn { Player, Enemy }
    public Turn currentTurn = Turn.Player;

    [Header("Safety Timeout")]
    public float turnTimeout = 10f; // if a turn lasts more than 10s, force switch

    private Coroutine timeoutCoroutine;

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
        RestartTimeout();
    }

    public void SetEnemyTurn()
    {
        currentTurn = Turn.Enemy;
        Debug.Log("--- ENEMY TURN ---");
        RestartTimeout();
    }

    void RestartTimeout()
    {
        if (timeoutCoroutine != null)
            StopCoroutine(timeoutCoroutine);
        timeoutCoroutine = StartCoroutine(TurnTimeoutRoutine());
    }

    IEnumerator TurnTimeoutRoutine()
    {
        yield return new WaitForSeconds(turnTimeout);

        // If we get here the turn was never switched — force it
        Debug.LogWarning("Turn timeout! Force switching from: " + currentTurn);
        if (currentTurn == Turn.Enemy)
            SetPlayerTurn();
        else
            SetEnemyTurn();
    }
}