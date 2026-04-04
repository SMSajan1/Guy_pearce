using UnityEngine;
using TMPro;

public class BillboardDisplay : MonoBehaviour
{
    public static BillboardDisplay Instance;

    [Header("Player Side")]
    public TextMeshPro playerNameText;
    public TextMeshPro playerKillsText;

    [Header("Enemy Side")]
    public TextMeshPro enemyNameText;
    public TextMeshPro enemyKillsText;

    private int playerKills = 0;
    private int enemyKills  = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateDisplay("---", "---", 0, 0);
    }

    public void SetFighterNames(string playerName, string enemyName)
    {
        if (playerNameText != null) playerNameText.text = playerName;
        if (enemyNameText  != null) enemyNameText.text  = enemyName;
    }

    public void AddPlayerKill()
    {
        playerKills++;
        if (playerKillsText != null)
            playerKillsText.text = playerKills.ToString();
    }

    public void AddEnemyKill()
    {
        enemyKills++;
        if (enemyKillsText != null)
            enemyKillsText.text = enemyKills.ToString();
    }

    void UpdateDisplay(string pName, string eName, int pKills, int eKills)
    {
        if (playerNameText  != null) playerNameText.text  = pName;
        if (enemyNameText   != null) enemyNameText.text   = eName;
        if (playerKillsText != null) playerKillsText.text = pKills.ToString();
        if (enemyKillsText  != null) enemyKillsText.text  = eKills.ToString();
    }
}