using System.Collections;
using UnityEngine;

public class GuyPearceAbilityController : MonoBehaviour
{
    public enum CharacterType
    {
        PHELSUM,
        OROBORO,
        CARAKARA,
        CERCI
    }

    [Header("Character Type")]
    public CharacterType characterType;

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

    [Header("Cast VFX Offsets (Local X,Y,Z)")]
    public Vector3 Q_CastOffset;
    public Vector3 E_CastOffset;
    public Vector3 A_CastOffset;
    public Vector3 D_CastOffset;
    public Vector3 R_CastOffset;

    [Header("Hit VFX (on opponent)")]
    public GameObject Q_HitVFX;
    public GameObject E_HitVFX;
    public GameObject A_HitVFX;
    public GameObject D_HitVFX;
    public GameObject R_HitVFX;

    [Header("Hit VFX Offsets (Local X,Y,Z)")]
    public Vector3 Q_HitOffset;
    public Vector3 E_HitOffset;
    public Vector3 A_HitOffset;
    public Vector3 D_HitOffset;
    public Vector3 R_HitOffset;

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
        if (isBusy) return;

        if (dodgeWindowOpen && Input.GetKeyDown(KeyCode.LeftShift))
        {
            dodged = true;
            if (opponentAnimator != null)
                opponentAnimator.SetTrigger("Dodge");
        }

        switch (characterType)
        {
            case CharacterType.PHELSUM: HandlePhelsum(); break;
            case CharacterType.OROBORO: HandleOroboro(); break;
            case CharacterType.CARAKARA: HandleCarakara(); break;
            case CharacterType.CERCI: HandleCerci(); break;
        }
    }

    void HandlePhelsum()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            StartCoroutine(PlayAbility("SparkDazzle", Q_CastVFX, Q_CastOffset, Q_HitVFX, Q_HitOffset, opponentHead, "Hit_1", false));
        if (Input.GetKeyDown(KeyCode.E))
            StartCoroutine(PlayAbility("Thunderbox", E_CastVFX, E_CastOffset, E_HitVFX, E_HitOffset, opponentHead, "Hit_2", false));
        if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(PlayAbility("ArcDischarge", A_CastVFX, A_CastOffset, A_HitVFX, A_HitOffset, opponentHead, "Hit_3", true));
        if (Input.GetKeyDown(KeyCode.D))
            StartCoroutine(PlayAbility("InducedCurrent", D_CastVFX, D_CastOffset, D_HitVFX, D_HitOffset, opponentFace, "Hit_4", true));
        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(PlayAbility("VoltageSpike", R_CastVFX, R_CastOffset, R_HitVFX, R_HitOffset, opponentHead, "Hit_5", true));
    }

    void HandleOroboro()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            StartCoroutine(PlayAbility("AnthelionBlast", Q_CastVFX, Q_CastOffset, Q_HitVFX, Q_HitOffset, opponentHead, "Hit_1", true));
        if (Input.GetKeyDown(KeyCode.E))
            StartCoroutine(PlayAbility("CrownFire", E_CastVFX, E_CastOffset, E_HitVFX, E_HitOffset, opponentHead, "Hit_2", false));
        if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(PlayAbility("Combust", A_CastVFX, A_CastOffset, A_HitVFX, A_HitOffset, opponentHead, "Hit_3", true));
        if (Input.GetKeyDown(KeyCode.D))
            StartCoroutine(PlayAbility("Tunnel", D_CastVFX, D_CastOffset, D_HitVFX, D_HitOffset, opponentFace, "Hit_4", true));
        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(PlayAbility("RedFlag", R_CastVFX, R_CastOffset, R_HitVFX, R_HitOffset, opponentHead, "Hit_5", true));
    }

    void HandleCarakara()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            StartCoroutine(PlayAbility("Squall", Q_CastVFX, Q_CastOffset, Q_HitVFX, Q_HitOffset, opponentHead, "Hit_1", true));
        if (Input.GetKeyDown(KeyCode.E))
            StartCoroutine(PlayAbility("Aerodynamic", E_CastVFX, E_CastOffset, E_HitVFX, E_HitOffset, opponentHead, "Hit_2", false));
        if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(PlayAbility("DeftSwipe", A_CastVFX, A_CastOffset, A_HitVFX, A_HitOffset, opponentHead, "Hit_3", true));
        if (Input.GetKeyDown(KeyCode.D))
            StartCoroutine(PlayAbility("JetMax", D_CastVFX, D_CastOffset, D_HitVFX, D_HitOffset, opponentFace, "Hit_4", true));
        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(PlayAbility("Intensify", R_CastVFX, R_CastOffset, R_HitVFX, R_HitOffset, opponentHead, "Hit_5", true));
    }

    void HandleCerci()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            StartCoroutine(PlayAbility("Downdraft", Q_CastVFX, Q_CastOffset, Q_HitVFX, Q_HitOffset, opponentHead, "Hit_1", true));
        if (Input.GetKeyDown(KeyCode.E))
            StartCoroutine(PlayAbility("Bluster", E_CastVFX, E_CastOffset, E_HitVFX, E_HitOffset, opponentHead, "Hit_2", false));
        if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(PlayAbility("RainBandLash", A_CastVFX, A_CastOffset, A_HitVFX, A_HitOffset, opponentHead, "Hit_3", true));
        if (Input.GetKeyDown(KeyCode.D))
            StartCoroutine(PlayAbility("SeededCloud", D_CastVFX, D_CastOffset, D_HitVFX, D_HitOffset, opponentFace, "Hit_4", true));
        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(PlayAbility("StrongBreeze", R_CastVFX, R_CastOffset, R_HitVFX, R_HitOffset, opponentHead, "Hit_5", true));
    }

    IEnumerator PlayAbility(
        string triggerName,
        GameObject castVFX, Vector3 castOffset,
        GameObject hitVFX, Vector3 hitOffset,
        Transform hitPoint,
        string opponentHitTrigger,
        bool isProjectile)
    {
        isBusy = true;
        dodged = false;

        animator.SetTrigger(triggerName);

        if (castVFX != null)
        {
            Vector3 castPos = transform.TransformPoint(castOffset);
            GameObject castFx = Instantiate(castVFX, castPos, transform.rotation);
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
                Vector3 startPos = transform.TransformPoint(castOffset);
                GameObject projectile = Instantiate(hitVFX, startPos, Quaternion.identity);
                StartCoroutine(MoveProjectile(projectile, hitPoint.TransformPoint(hitOffset)));
            }
            else if (hitVFX != null && hitPoint != null)
            {
                Vector3 hitPos = hitPoint.TransformPoint(hitOffset);
                GameObject hitFx = Instantiate(hitVFX, hitPos, hitPoint.rotation);
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
