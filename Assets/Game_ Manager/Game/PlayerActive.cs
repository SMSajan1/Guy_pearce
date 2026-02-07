using UnityEngine;

public class PlayerActivator : MonoBehaviour
{
    public GameObject phelsum;
    public GameObject oroboro;
    public GameObject carakara;
    public GameObject cerci;


    void Start()
    {
        ActivateSelectedCharacter();
    }

    void ActivateSelectedCharacter()
    {
        CharacterType selected = GameManager.Instance.selectedCharacter;

        phelsum.SetActive(false);
        oroboro.SetActive(false);
        carakara.SetActive(false);
        cerci.SetActive(false);

        switch (selected)
        {
            case CharacterType.Phelsum:
                phelsum.SetActive(true);
                break;

            case CharacterType.oroboro:
                oroboro.SetActive(true);
                break;

            case CharacterType.carakara:
                carakara.SetActive(true);
                break;

            case CharacterType.cerci:
                cerci.SetActive(true);
                break;


        }
    
    }
}
