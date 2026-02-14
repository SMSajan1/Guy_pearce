using System.Collections;
using UnityEngine;

public class GuyPearceAbilityController : MonoBehaviour
{
    [Header("Animator (Your Character)")]
    public Animator animator;

    [Header("Opponent")]
    public Animator opponentAnimator;
    public Transform opponentHead;
    public Transform opponentFace;

    [Header("Cast VFX (on you)")]
    public GameObject Q_CastVFX;
    public GameObject E_CastVFX;
    public GameObject A_CastVFX;
    public GameObject D_CastVFX;
    public GameObject R_CastVFX;

    [Header("Hit VFX (on opponent)")]
    public GameObject Q_HitVFX;
    public GameObject E_HitVFX;
    public GameObject A_HitVFX;
    public GameObject D_HitVFX;
    public GameObject R_HitVFX;

    [Header("Projectile VFX (A, D, R)")]
    public float projectileSpeed = 12f;

    [Header("Timing")]
    public float hitDelay = 0.35f;
    public float dodgeWindow = 0.25f;
    public float recoveryTime = 0.3f;

    private bool dodgeWindowOpen;
    private bool dodged;
    private bool isBusy;

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        animator.SetBool("Walk", !isBusy && Input.GetKey(KeyCode.W));

        if (dodgeWindowOpen && Input.GetKeyDown(KeyCode.LeftShift))
        {
            dodged = true;
            if (opponentAnimator != null)
                opponentAnimator.SetTrigger("Dodge");
        }

        if (isBusy) return;

        if (Input.GetKeyDown(KeyCode.Q))
            StartCoroutine(PlayAbility("SparkDazzle", Q_CastVFX, Q_HitVFX, opponentHead, "Hit_1", false));

        if (Input.GetKeyDown(KeyCode.E))
            StartCoroutine(PlayAbility("Thunderbox", E_CastVFX, E_HitVFX, opponentHead, "Hit_2", false));

        if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(PlayAbility("ArcDischarge", A_CastVFX, A_HitVFX, opponentHead, "Hit_3", true));

        if (Input.GetKeyDown(KeyCode.D))
            StartCoroutine(PlayAbility("InducedCurrent", D_CastVFX, D_HitVFX, opponentFace, "Hit_4", true));

        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(PlayAbility("VoltageSpike", R_CastVFX, R_HitVFX, opponentHead, "Hit_5", true));
    }

    IEnumerator PlayAbility(
        string triggerName,
        GameObject castVFX,
        GameObject hitVFX,
        Transform hitPoint,
        string opponentHitTrigger,
        bool isProjectile
    )
    {
        isBusy = true;
        dodged = false;

        animator.SetTrigger(triggerName);

        // Cast VFX (on player)
        if (castVFX != null)
        {
            GameObject castFx = Instantiate(castVFX, transform.position + transform.forward, transform.rotation);
            Destroy(castFx, 3f);
        }

        yield return new WaitForSeconds(hitDelay);

        dodgeWindowOpen = true;
        yield return new WaitForSeconds(dodgeWindow);
        dodgeWindowOpen = false;

        if (!dodged)
        {
            if (isProjectile && hitVFX != null && hitPoint != null)
            {
                GameObject projectile = Instantiate(hitVFX, transform.position + Vector3.up, Quaternion.identity);
                StartCoroutine(MoveProjectile(projectile, hitPoint.position));
            }
            else if (hitVFX != null && hitPoint != null)
            {
                GameObject hitFx = Instantiate(hitVFX, hitPoint.position, hitPoint.rotation);
                Destroy(hitFx, 3f);
            }

            if (opponentAnimator != null)
                opponentAnimator.SetTrigger(opponentHitTrigger);
        }

        yield return new WaitForSeconds(recoveryTime);

        animator.SetTrigger("Idle");
        transform.position = startPosition;
        transform.rotation = startRotation;

        isBusy = false;
    }

    IEnumerator MoveProjectile(GameObject projectile, Vector3 targetPos)
    {
        while (projectile != null && Vector3.Distance(projectile.transform.position, targetPos) > 0.1f)
        {
            projectile.transform.position = Vector3.MoveTowards(
                projectile.transform.position,
                targetPos,
                projectileSpeed * Time.deltaTime
            );

            yield return null;
        }

        if (projectile != null)
            Destroy(projectile, 2f);
    }
}
