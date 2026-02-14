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

    [Header("Timing (Dodge System)")]
    public float hitDelay = 0.4f;
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
        // 🔒 Force Walk off while busy
        animator.SetBool("Walk", !isBusy && Input.GetKey(KeyCode.W));

        // 🚫 Block new abilities while busy
        if (isBusy) return;

        if (Input.GetKeyDown(KeyCode.Q))
            StartCoroutine(PlayAbility("SparkDazzle", Q_CastVFX, Q_HitVFX, opponentHead));

        if (Input.GetKeyDown(KeyCode.E))
            StartCoroutine(PlayAbility("Thunderbox", E_CastVFX, E_HitVFX, opponentHead));

        if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(PlayAbility("ArcDischarge", A_CastVFX, A_HitVFX, opponentHead));

        if (Input.GetKeyDown(KeyCode.D))
            StartCoroutine(PlayAbility("InducedCurrent", D_CastVFX, D_HitVFX, opponentFace));

        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(PlayAbility("VoltageSpike", R_CastVFX, R_HitVFX, null));

        // Opponent Dodge (Shift)
        if (dodgeWindowOpen && Input.GetKeyDown(KeyCode.LeftShift))
        {
            dodged = true;
            if (opponentAnimator != null)
                opponentAnimator.SetTrigger("Dodge");
        }
    }

    IEnumerator PlayAbility(string triggerName, GameObject castVFX, GameObject hitVFX, Transform hitPoint)
    {
        isBusy = true;

        // 🚫 Reset animator state so nothing else blends in
        animator.SetBool("Walk", false);
        animator.ResetTrigger("SparkDazzle");
        animator.ResetTrigger("Thunderbox");
        animator.ResetTrigger("ArcDischarge");
        animator.ResetTrigger("InducedCurrent");
        animator.ResetTrigger("VoltageSpike");

        dodged = false;

        // ▶ Play ability animation
        animator.SetTrigger(triggerName);

        // 1️⃣ Cast VFX on you
        if (castVFX != null)
        {
            GameObject castFx = Instantiate(castVFX, transform.position + transform.forward, transform.rotation);
            Destroy(castFx, 3f);
        }

        // 2️⃣ Wait before dodge window
        yield return new WaitForSeconds(hitDelay);

        dodgeWindowOpen = true;

        // 3️⃣ Dodge timing window
        yield return new WaitForSeconds(dodgeWindow);

        dodgeWindowOpen = false;

        // 4️⃣ Hit if not dodged
        if (!dodged)
        {
            if (hitVFX != null && hitPoint != null)
            {
                GameObject hitFx = Instantiate(hitVFX, hitPoint.position, hitPoint.rotation);
                Destroy(hitFx, 3f);
            }

            if (opponentAnimator != null)
                opponentAnimator.SetTrigger("Hit");
        }

        // 5️⃣ Recovery lock
        yield return new WaitForSeconds(recoveryTime);

        animator.SetTrigger("Idle");
        transform.position = startPosition;
        transform.rotation = startRotation;

        isBusy = false; // 🔓 unlock
    }
}
