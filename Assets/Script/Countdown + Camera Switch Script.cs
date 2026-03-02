using UnityEngine;
using Cinemachine;
using TMPro;
using System.Collections;

public class CameraCountdown : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineVirtualCamera countdownCam;
    public CinemachineVirtualCamera gameplayCam;

    [Header("UI")]
    public TextMeshProUGUI countdownText;

    [Header("Settings")]
    public float countdownInterval = 1f;

    private void Start()
    {
        // Make sure countdown cam is active first
        countdownCam.Priority = 20;
        gameplayCam.Priority = 10;

        StartCoroutine(CountdownRoutine());
    }



    IEnumerator CountdownRoutine()
    {
        countdownText.gameObject.SetActive(true);

        countdownText.text = "3";
        yield return new WaitForSeconds(countdownInterval);

        countdownText.text = "2";
        yield return new WaitForSeconds(countdownInterval);

        countdownText.text = "1";
        yield return new WaitForSeconds(countdownInterval);

        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.7f);

        countdownText.gameObject.SetActive(false);

        // Switch cameras (Cinemachine will blend automatically)
        countdownCam.Priority = 5;
        gameplayCam.Priority = 20;
    }
}
