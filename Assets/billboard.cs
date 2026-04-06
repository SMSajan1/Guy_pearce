using UnityEngine;
using TMPro;

public class BillboardDisplay : MonoBehaviour
{
    public static BillboardDisplay Instance;

    [Header("Player Side")]
    public TextMeshPro playerNameText;
    public TextMeshPro playerKillsText;
    public SpriteRenderer playerImage;

    [Header("Enemy Side")]
    public TextMeshPro enemyNameText;
    public TextMeshPro enemyKillsText;
    public SpriteRenderer enemyImage;

    [Header("Character Portraits")]
    public Sprite phelsumSprite;
    public Sprite oroboroSprite;
    public Sprite carakaraSprite;
    public Sprite cerciSprite;
    public Sprite mbengaSprite;
    public Sprite ryuudeSprite;

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
    playerName = CleanName(playerName);
    enemyName  = CleanName(enemyName);

    if (playerNameText != null) playerNameText.text = playerName;
    if (enemyNameText  != null) enemyNameText.text  = enemyName;

    SetCharacterImages(playerName, enemyName);
}

    void SetCharacterImages(string playerName, string enemyName)
    {
        if (playerImage != null)
            playerImage.sprite = GetSpriteByName(playerName);

        if (enemyImage != null)
            enemyImage.sprite = GetSpriteByName(enemyName);
    }

Sprite GetSpriteByName(string name)
{
    if (string.IsNullOrEmpty(name)) return null;

    string n = name.ToLower().Trim();

    // 🔥 Remove common Unity junk
    n = n.Replace("(clone)", "").Trim();

    if (n.Contains("phelsum")) return phelsumSprite;
    if (n.Contains("oroboro")) return oroboroSprite;
    if (n.Contains("carakara")) return carakaraSprite;
    if (n.Contains("cerci")) return cerciSprite;
    if (n.Contains("mbenga")) return mbengaSprite;
    if (n.Contains("ryuude")) return ryuudeSprite;

    Debug.LogError("❌ NO SPRITE MATCH FOR: " + name);
    return null;
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

    string CleanName(string name)
    {
        if (string.IsNullOrEmpty(name)) return "";

        return name.Replace("(Clone)", "").Trim();
    }
}