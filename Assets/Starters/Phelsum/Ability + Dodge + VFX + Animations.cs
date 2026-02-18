using System.Collections;
using UnityEngine;

public class GuyPearceAbilityController : MonoBehaviour
{
    public enum CharacterType { PHELSUM, OROBORO, CARAKARA, CERCI }
    public CharacterType characterType;

    [Header("Animators")]
    public Animator animator;
    public Animator opponentAnimator;

    [Header("Opponent Points")]
    public Transform opponentHead;
    public Transform opponentBody;

    [Header("Cast VFX")]
    public GameObject Q_CastVFX, E_CastVFX, A_CastVFX, D_CastVFX, R_CastVFX;

    [Header("Cast VFX Offsets (Local)")]
    public Vector3 Q_CastOffset, E_CastOffset, A_CastOffset, D_CastOffset, R_CastOffset;

    [Header("Hit VFX")]
    public GameObject Q_HitVFX, E_HitVFX, A_HitVFX, D_HitVFX, R_HitVFX;

    [Header("Hit VFX Offsets (Local)")]
    public Vector3 Q_HitOffset, E_HitOffset, A_HitOffset, D_HitOffset, R_HitOffset;

    [Header("Timing")]
    public float hitDelay = 0.35f;
    public float animationLockTime = 0.9f;

    [Header("Projectile")]
    public float projectileSpeed = 15f;

    private bool isBusy;

    void Update()
    {
        if (isBusy) return;

        if (Input.GetKeyDown(KeyCode.Q)) TriggerAbility("Q");
        if (Input.GetKeyDown(KeyCode.E)) TriggerAbility("E");
        if (Input.GetKeyDown(KeyCode.A)) TriggerAbility("A");
        if (Input.GetKeyDown(KeyCode.D)) TriggerAbility("D");
        if (Input.GetKeyDown(KeyCode.R)) TriggerAbility("R");
    }

    void TriggerAbility(string key)
    {
        switch (characterType)
        {
            case CharacterType.PHELSUM:
                if (key == "Q") StartCoroutine(Ability("SparkDazzle", Q_CastVFX, Q_CastOffset, Q_HitVFX, Q_HitOffset, opponentHead, "Hit_1", false));
                if (key == "E") StartCoroutine(Ability("Thunderbox", E_CastVFX, E_CastOffset, E_HitVFX, E_HitOffset, opponentHead, "Hit_2", false));
                if (key == "A") StartCoroutine(Ability("ArcDischarge", A_CastVFX, A_CastOffset, A_HitVFX, A_HitOffset, opponentHead, "Hit_3", true));
                if (key == "D") StartCoroutine(Ability("InducedCurrent", D_CastVFX, D_CastOffset, D_HitVFX, D_HitOffset, opponentBody, "Hit_4", true));
                if (key == "R") StartCoroutine(Ability("VoltageSpike", R_CastVFX, R_CastOffset, R_HitVFX, R_HitOffset, opponentHead, "Hit_5", true));
                break;

            case CharacterType.OROBORO:
                if (key == "Q") StartCoroutine(Ability("AnthelionBlast", Q_CastVFX, Q_CastOffset, Q_HitVFX, Q_HitOffset, opponentHead, "Hit_1", false));
                if (key == "E") StartCoroutine(Ability("CrownFire", E_CastVFX, E_CastOffset, E_HitVFX, E_HitOffset, opponentHead, "Hit_2", false));
                if (key == "A") StartCoroutine(Ability("Combust", A_CastVFX, A_CastOffset, A_HitVFX, A_HitOffset, opponentHead, "Hit_3", true));
                if (key == "D") StartCoroutine(Ability("Tunnel", D_CastVFX, D_CastOffset, D_HitVFX, D_HitOffset, opponentBody, "Hit_4", false));
                if (key == "R") StartCoroutine(Ability("RedFlag", R_CastVFX, R_CastOffset, R_HitVFX, R_HitOffset, opponentBody, "Hit_5", false));
                break;

            case CharacterType.CARAKARA:
                if (key == "Q") StartCoroutine(Ability("Squall", Q_CastVFX, Q_CastOffset, Q_HitVFX, Q_HitOffset, opponentBody, "Hit_1", true));
                if (key == "E") StartCoroutine(Ability("Aerodynamic", E_CastVFX, E_CastOffset, E_HitVFX, E_HitOffset, opponentBody, "Hit_2", false));
                if (key == "A") StartCoroutine(Ability("DeftSwipe", A_CastVFX, A_CastOffset, A_HitVFX, A_HitOffset, opponentBody, "Hit_3", false));
                if (key == "D") StartCoroutine(Ability("JetMax", D_CastVFX, D_CastOffset, D_HitVFX, D_HitOffset, opponentBody, "Hit_4", false));
                if (key == "R") StartCoroutine(Ability("Intensify", R_CastVFX, R_CastOffset, R_HitVFX, R_HitOffset, opponentBody, "Hit_5", false));
                break;

            case CharacterType.CERCI:
                if (key == "Q") StartCoroutine(Ability("DownDraft", Q_CastVFX, Q_CastOffset, Q_HitVFX, Q_HitOffset, opponentBody, "Hit_1", false));
                if (key == "E") StartCoroutine(Ability("Bluster", E_CastVFX, E_CastOffset, E_HitVFX, E_HitOffset, opponentBody, "Hit_2", true));
                if (key == "A") StartCoroutine(Ability("RainBandLash", A_CastVFX, A_CastOffset, A_HitVFX, A_HitOffset, opponentBody, "Hit_3", true));
                if (key == "D") StartCoroutine(Ability("SeededCloud", D_CastVFX, D_CastOffset, D_HitVFX, D_HitOffset, opponentBody, "Hit_4", false));
                if (key == "R") StartCoroutine(Ability("StrongBreeze", R_CastVFX, R_CastOffset, R_HitVFX, R_HitOffset, opponentBody, "Hit_5", false));
                break;
        }
    }

    IEnumerator Ability(string anim, GameObject castFx, Vector3 castOffset,
        GameObject hitFx, Vector3 hitOffset, Transform hitPoint,
        string opponentHitTrigger, bool projectile)
    {
        isBusy = true;

        animator.SetTrigger(anim);

        if (castFx)
            Instantiate(castFx, transform.TransformPoint(castOffset), transform.rotation);

        yield return new WaitForSeconds(hitDelay);

        // Play Opponent Hit Animation at same time as Hit VFX
        if (opponentAnimator)
            opponentAnimator.SetTrigger(opponentHitTrigger);

        if (hitFx)
        {
            if (projectile)
            {
                GameObject proj = Instantiate(hitFx, transform.TransformPoint(castOffset), Quaternion.identity);
                StartCoroutine(MoveProjectile(proj, hitPoint.TransformPoint(hitOffset)));
            }
            else
            {
                Instantiate(hitFx, hitPoint.TransformPoint(hitOffset), hitPoint.rotation);
            }
        }

        yield return new WaitForSeconds(animationLockTime);

        isBusy = false;
    }

    IEnumerator MoveProjectile(GameObject fx, Vector3 target)
    {
        while (fx && Vector3.Distance(fx.transform.position, target) > 0.1f)
        {
            fx.transform.position = Vector3.MoveTowards(fx.transform.position, target, projectileSpeed * Time.deltaTime);
            yield return null;
        }
        if (fx) Destroy(fx, 2f);
    }
}
