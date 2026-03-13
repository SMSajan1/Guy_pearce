using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [Header("Assign your active Gameplay Virtual Camera")]
    public CinemachineVirtualCamera gameplayVirtualCamera;

    [Header("Shake Settings")]
    public float defaultDuration = 0.2f;
    public float defaultAmplitude = 2f;
    public float defaultFrequency = 2f;

    private CinemachineBasicMultiChannelPerlin perlinNoise;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (gameplayVirtualCamera == null)
        {
            Debug.LogError("CameraShake: gameplayVirtualCamera is NOT assigned!");
            return;
        }

        perlinNoise = gameplayVirtualCamera
            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (perlinNoise == null)
            Debug.LogError("CameraShake: Perlin Noise component NOT found on " + gameplayVirtualCamera.name);
        else
            Debug.Log("CameraShake: Successfully linked to " + gameplayVirtualCamera.name);

        perlinNoise.m_AmplitudeGain = 0f;
        perlinNoise.m_FrequencyGain = 0f;
    }

    // Test shake with T key
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("CameraShake: TEST SHAKE triggered");
            Shake(1f, 3f, 3f);
        }
    }

    public void Shake(float duration = -1f, float amplitude = -1f, float frequency = -1f)
    {
        if (perlinNoise == null)
        {
            Debug.LogError("CameraShake: perlinNoise is NULL — shake cannot run!");
            return;
        }

        float d = duration < 0 ? defaultDuration : duration;
        float a = amplitude < 0 ? defaultAmplitude : amplitude;
        float f = frequency < 0 ? defaultFrequency : frequency;

        Debug.Log($"CameraShake: Shaking — duration:{d} amplitude:{a} frequency:{f}");
        StartCoroutine(ShakeRoutine(d, a, f));
    }

    IEnumerator ShakeRoutine(float duration, float amplitude, float frequency)
    {
        perlinNoise.m_AmplitudeGain = amplitude;
        perlinNoise.m_FrequencyGain = frequency;

        yield return new WaitForSeconds(duration);

        perlinNoise.m_AmplitudeGain = 0f;
        perlinNoise.m_FrequencyGain = 0f;
    }
}