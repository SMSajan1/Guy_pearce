using System.Collections;
using UnityEngine;

public class GuyPearceCerciAbilityController : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("VFX Prefabs")]
    public GameObject StrongVFX;     // Q
    public GameObject BurstVFX;     // E
    public GameObject WaterVFX;    // A
    public GameObject sliveVFX;     // D
    public GameObject breathVFX; // R

    [Header("Opponent Target")]
    public Transform opponentHead;
    public Transform opponentFace;

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(PlayAbility("Q_Attack", StrongVFX, opponentHead));
            animator.SetTrigger("Q_Attack");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PlayAbility("E_Attack", BurstVFX, opponentHead));
            animator.SetTrigger("E_Attack");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(PlayAbility("spark", WaterVFX, opponentHead));
            animator.SetTrigger("spark");
        }

        if (Input.GetKeyDown(KeyCode.D))
            {   
            StartCoroutine(PlayAbility("dirt", sliveVFX, opponentFace));
            animator.SetTrigger("D_Attack");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(PlayAbility("redLight", breathVFX, null));
            animator.SetTrigger("redLight");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //walk forward boolean true
            //loop the animation while key is held down
            if (Input.GetKey(KeyCode.W))
            {
                animator.SetBool("Walk", true);
            }
            else
            {
                animator.SetBool("Walk", false);
            }

        }
    }

    IEnumerator PlayAbility(string triggerName, GameObject vfx, Transform spawnPoint)
    {
        animator.SetTrigger(triggerName);

        // Spawn VFX
        if (vfx != null)
        {
            GameObject fx;

            if (spawnPoint != null)
                fx = Instantiate(vfx, spawnPoint.position, spawnPoint.rotation);
            else
                fx = Instantiate(vfx);

            Destroy(fx, 3f);
        }

        // wait animation time
        yield return new WaitForSeconds(1f);

        // reset position
        transform.position = startPosition;
        transform.rotation = startRotation;

        animator.SetTrigger("Idle");
    }
}
