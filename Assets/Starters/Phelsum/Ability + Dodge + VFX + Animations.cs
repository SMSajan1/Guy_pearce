using System.Collections;
using UnityEngine;

public class GuyPearceAbilityController : MonoBehaviour
{
    public enum CharacterType { PHELSUM, OROBORO, CARAKARA, CERCI, MBENGA, RYUUDE }
    public CharacterType characterType;

    [Header("Player or Enemy")]
    public bool isPlayer = true;

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

    [Header("Ability Damage (Q / E / A / D / R)")]
    public float Q_Damage = 5f;
    public float E_Damage = 5f;
    public float A_Damage = 5f;
    public float D_Damage = 5f;
    public float R_Damage = 5f;

    [HideInInspector] public CharacterHealth currentOpponentHealth;


    [HideInInspector] public CharacterHitReactions opponentHitReactions;


    private bool isBusy;

    void Update()
    {
        if (!isPlayer) return;
        if (isBusy) return;

        // Block player input if it's not their turn
        if (!TurnManager.Instance.IsPlayerTurn()) return;

        if (Input.GetKeyDown(KeyCode.Q) && EffortBar.Instance.TryUseAbility("Q")) TriggerAbility("Q");
        if (Input.GetKeyDown(KeyCode.E) && EffortBar.Instance.TryUseAbility("E")) TriggerAbility("E");
        if (Input.GetKeyDown(KeyCode.A) && EffortBar.Instance.TryUseAbility("A")) TriggerAbility("A");
        if (Input.GetKeyDown(KeyCode.D) && EffortBar.Instance.TryUseAbility("D")) TriggerAbility("D");
        if (Input.GetKeyDown(KeyCode.R) && EffortBar.Instance.TryUseAbility("R")) TriggerAbility("R");
    }




    public void TriggerAbility(string key)
    {
        float damage = 5f;
        if (key == "Q") damage = Q_Damage;
        else if (key == "E") damage = E_Damage;
        else if (key == "A") damage = A_Damage;
        else if (key == "D") damage = D_Damage;
        else if (key == "R") damage = R_Damage;

        switch (characterType)
        {
            case CharacterType.PHELSUM:
                if (key == "Q") StartCoroutine(Ability("SparkDazzle", Q_CastVFX, Q_CastOffset, Q_HitVFX, Q_HitOffset, opponentHead, "Hit_1", false, damage));
                if (key == "E") StartCoroutine(Ability("Thunderbox", E_CastVFX, E_CastOffset, E_HitVFX, E_HitOffset, opponentHead, "Hit_2", false, damage));
                if (key == "A") StartCoroutine(Ability("ArcDischarge", A_CastVFX, A_CastOffset, A_HitVFX, A_HitOffset, opponentHead, "Hit_3", true, damage));
                if (key == "D") StartCoroutine(Ability("InducedCurrent", D_CastVFX, D_CastOffset, D_HitVFX, D_HitOffset, opponentBody, "Hit_4", true, damage));
                if (key == "R") StartCoroutine(Ability("VoltageSpike", R_CastVFX, R_CastOffset, R_HitVFX, R_HitOffset, opponentHead, "Hit_5", true, damage));
                break;

            case CharacterType.OROBORO:
                if (key == "Q") StartCoroutine(Ability("AnthelionBlast", Q_CastVFX, Q_CastOffset, Q_HitVFX, Q_HitOffset, opponentHead, "Hit_1", false, damage));
                if (key == "E") StartCoroutine(Ability("CrownFire", E_CastVFX, E_CastOffset, E_HitVFX, E_HitOffset, opponentHead, "Hit_2", false, damage));
                if (key == "A") StartCoroutine(Ability("Combust", A_CastVFX, A_CastOffset, A_HitVFX, A_HitOffset, opponentHead, "Hit_3", true, damage));
                if (key == "D") StartCoroutine(Ability("Tunnel", D_CastVFX, D_CastOffset, D_HitVFX, D_HitOffset, opponentBody, "Hit_4", false, damage));
                if (key == "R") StartCoroutine(Ability("RedFlag", R_CastVFX, R_CastOffset, R_HitVFX, R_HitOffset, opponentBody, "Hit_5", false, damage));
                break;

            case CharacterType.CARAKARA:
                if (key == "Q") StartCoroutine(Ability("Squall", Q_CastVFX, Q_CastOffset, Q_HitVFX, Q_HitOffset, opponentBody, "Hit_1", true, damage));
                if (key == "E") StartCoroutine(Ability("Aerodynamic", E_CastVFX, E_CastOffset, E_HitVFX, E_HitOffset, opponentBody, "Hit_2", false, damage));
                if (key == "A") StartCoroutine(Ability("DeftSwipe", A_CastVFX, A_CastOffset, A_HitVFX, A_HitOffset, opponentBody, "Hit_3", false, damage));
                if (key == "D") StartCoroutine(Ability("JetMax", D_CastVFX, D_CastOffset, D_HitVFX, D_HitOffset, opponentBody, "Hit_4", false, damage));
                if (key == "R") StartCoroutine(Ability("Intensify", R_CastVFX, R_CastOffset, R_HitVFX, R_HitOffset, opponentBody, "Hit_5", false, damage));
                break;

            case CharacterType.CERCI:
                if (key == "Q") StartCoroutine(Ability("Downdraft", Q_CastVFX, Q_CastOffset, Q_HitVFX, Q_HitOffset, opponentBody, "Hit_1", false, damage));
                if (key == "E") StartCoroutine(Ability("Bluster", E_CastVFX, E_CastOffset, E_HitVFX, E_HitOffset, opponentBody, "Hit_2", true, damage));
                if (key == "A") StartCoroutine(Ability("RainBandLash", A_CastVFX, A_CastOffset, A_HitVFX, A_HitOffset, opponentBody, "Hit_3", true, damage));
                if (key == "D") StartCoroutine(Ability("SeededCloud", D_CastVFX, D_CastOffset, D_HitVFX, D_HitOffset, opponentBody, "Hit_4", false, damage));
                if (key == "R") StartCoroutine(Ability("StrongBreeze", R_CastVFX, R_CastOffset, R_HitVFX, R_HitOffset, opponentBody, "Hit_5", false, damage));
                break;

            case CharacterType.MBENGA:
                if (key == "Q") StartCoroutine(Ability("MeteoricWater", Q_CastVFX, Q_CastOffset, Q_HitVFX, Q_HitOffset, opponentBody, "Hit_1", true, damage));
                if (key == "E") StartCoroutine(Ability("Waterlog", E_CastVFX, E_CastOffset, E_HitVFX, E_HitOffset, opponentBody, "Hit_2", false, damage));
                if (key == "A") StartCoroutine(Ability("Torrent", A_CastVFX, A_CastOffset, A_HitVFX, A_HitOffset, opponentBody, "Hit_3", true, damage));
                if (key == "D") StartCoroutine(Ability("DewPoint", D_CastVFX, D_CastOffset, D_HitVFX, D_HitOffset, opponentBody, "Hit_4", false, damage));
                if (key == "R") StartCoroutine(Ability("BrumousDance", R_CastVFX, R_CastOffset, R_HitVFX, R_HitOffset, opponentBody, "Hit_5", false, damage));
                break;

            case CharacterType.RYUUDE:
                if (key == "Q") StartCoroutine(Ability("MulticellVolley", Q_CastVFX, Q_CastOffset, Q_HitVFX, Q_HitOffset, opponentBody, "Hit_1", true, damage));
                if (key == "E") StartCoroutine(Ability("CoronalRain", E_CastVFX, E_CastOffset, E_HitVFX, E_HitOffset, opponentBody, "Hit_2", true, damage));
                if (key == "A") StartCoroutine(Ability("SevereStorm", A_CastVFX, A_CastOffset, A_HitVFX, A_HitOffset, opponentBody, "Hit_3", true, damage));
                if (key == "D") StartCoroutine(Ability("ToxicShot", D_CastVFX, D_CastOffset, D_HitVFX, D_HitOffset, opponentBody, "Hit_4", true, damage));
                if (key == "R") StartCoroutine(Ability("SolarDynamo", R_CastVFX, R_CastOffset, R_HitVFX, R_HitOffset, opponentBody, "Hit_5", true, damage));
                break;
        }
    }

    IEnumerator Ability(string anim, GameObject castFx, Vector3 castOffset,
    GameObject hitFx, Vector3 hitOffset, Transform hitPoint,
    string opponentHitTrigger, bool projectile, float damage)
    {
        isBusy = true;

        // Safety check — if animator is missing skip entirely
        if (animator == null)
        {
            isBusy = false;
            if (isPlayer) TurnManager.Instance.SetEnemyTurn();
            else TurnManager.Instance.SetPlayerTurn();
            yield break;
        }

        animator.SetTrigger(anim);

        if (castFx)
        {
            GameObject cast = Instantiate(castFx, transform.TransformPoint(castOffset), transform.rotation);
            Destroy(cast, 3f);
        }

        yield return new WaitForSeconds(hitDelay);

        // Deal damage
        if (isPlayer)
            BattleManager.Instance.DamageActiveEnemy(damage);
        else
            BattleManager.Instance.DamageActivePlayer(damage);

        // Hit VFX and opponent reaction
        if (hitFx != null)
        {
            Vector3 hitWorldPos = hitPoint != null
                ? hitPoint.TransformPoint(hitOffset)
                : (opponentAnimator != null ? opponentAnimator.transform.position : transform.position);

            if (projectile)
            {
                Vector3 spawnPos = transform.TransformPoint(castOffset);
                Vector3 direction = (hitWorldPos - spawnPos).normalized;

                // Guard against zero direction
                if (direction == Vector3.zero) direction = transform.forward;

                Quaternion spawnRot = Quaternion.LookRotation(direction);
                GameObject proj = Instantiate(hitFx, spawnPos, spawnRot);

                StartCoroutine(MoveProjectile(proj, hitWorldPos, () =>
                {
                    if (opponentAnimator != null)
                    {
                        string trigger = opponentHitReactions != null
                            ? opponentHitReactions.GetTrigger(opponentHitTrigger)
                            : opponentHitTrigger;

                        if (HasAnimatorParameter(opponentAnimator, trigger))
                            opponentAnimator.SetTrigger(trigger);
                    }
                }));
            }
            else
            {
                if (opponentAnimator != null)
                {
                    string trigger = opponentHitReactions != null
                        ? opponentHitReactions.GetTrigger(opponentHitTrigger)
                        : opponentHitTrigger;

                    if (HasAnimatorParameter(opponentAnimator, trigger))
                        opponentAnimator.SetTrigger(trigger);
                }

                GameObject hit = Instantiate(hitFx, hitPoint != null
                    ? hitPoint.TransformPoint(hitOffset)
                    : transform.position,
                    hitPoint != null ? hitPoint.rotation : Quaternion.identity);
                Destroy(hit, 3f);
            }
        }

        yield return new WaitForSeconds(animationLockTime);

        isBusy = false;

        // THIS MUST ALWAYS RUN — wrapped in try/catch so nothing can stop it
        try
        {
            if (isPlayer)
                TurnManager.Instance.SetEnemyTurn();
            else
                TurnManager.Instance.SetPlayerTurn();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Turn switch failed: " + e.Message);
        }
    }


    bool HasAnimatorParameter(Animator anim, string paramName)
    {
        if (anim == null) return false;
        foreach (AnimatorControllerParameter param in anim.parameters)
            if (param.name == paramName) return true;
        return false;
    }

    IEnumerator MoveProjectile(GameObject fx, Vector3 target, System.Action onImpact = null)
        {
            if (fx == null) yield break;

            while (fx && Vector3.Distance(fx.transform.position, target) > 0.1f)
            {
                fx.transform.position = Vector3.MoveTowards(
                    fx.transform.position, target, projectileSpeed * Time.deltaTime);
                yield return null;
            }

            onImpact?.Invoke();

            if (fx) Destroy(fx, 2f);
        }


    
}