using UnityEngine;

public class CharacterAbilityController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private Quaternion originalRotation;

    void Start()
    {
        animator = GetComponent<Animator>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayAbility("Halo");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            PlayAbility("Fire");
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            PlayAbility("Spark");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            PlayAbility("Dirt");
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            PlayAbility("RedLight");
        }
    }

    void PlayAbility(string triggerName)
    {
        // Trigger the animation
        animator.SetTrigger(triggerName);

        // Reset after 1 second
        CancelInvoke(nameof(ResetToIdle));
        Invoke(nameof(ResetToIdle), 1f);
    }

    void ResetToIdle()
    {
        // Reset position and rotation
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // Go back to Idle
        animator.SetTrigger("Idle");
    }
}
