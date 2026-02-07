using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterPreviewAnimator : MonoBehaviour
{
    public Image previewImage;

    [Header("Animation Settings")]
    public float fadeSpeed = 6f;
    public float scalePop = 1.1f;
    public float animationDuration = 0.25f;

    Coroutine animationRoutine;

    public void ChangeSpriteAnimated(Sprite newSprite)
    {
        if (animationRoutine != null)
            StopCoroutine(animationRoutine);

        animationRoutine = StartCoroutine(AnimateChange(newSprite));
    }

    IEnumerator AnimateChange(Sprite newSprite)
    {
        float time = 0;

        Color startColor = previewImage.color;
        Vector3 startScale = transform.localScale;

        // Fade OUT + Shrink
        while (time < animationDuration)
        {
            time += Time.deltaTime * fadeSpeed;

            float t = time / animationDuration;

            previewImage.color = new Color(1, 1, 1, 1 - t);
            transform.localScale = Vector3.Lerp(startScale, Vector3.one * 0.9f, t);

            yield return null;
        }

        // Change sprite
        previewImage.sprite = newSprite;

        time = 0;

        // Fade IN + Pop
        while (time < animationDuration)
        {
            time += Time.deltaTime * fadeSpeed;

            float t = time / animationDuration;

            previewImage.color = new Color(1, 1, 1, t);
            transform.localScale = Vector3.Lerp(Vector3.one * scalePop, Vector3.one, t);

            yield return null;
        }

        previewImage.color = Color.white;
        transform.localScale = Vector3.one;
    }
}
