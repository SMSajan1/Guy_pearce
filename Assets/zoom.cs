using System.Collections;
using UnityEngine;

public class CameraZoomToBillboard : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public Transform billboardTarget;

    [Header("Zoom Settings")]
    public Vector3 zoomOffset = new Vector3(0, 2, -3);
    public float zoomSpeed = 5f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isZoomed = false;
    private bool isMoving = false;

    void Start()
    {
        originalPosition = cameraTransform.position;
        originalRotation = cameraTransform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isMoving)
        {
            if (!isZoomed)
                StartCoroutine(ZoomToBillboard());
            else
                StartCoroutine(ReturnToOriginal());
        }
    }

    IEnumerator ZoomToBillboard()
    {
        isMoving = true;
        isZoomed = true;

        Vector3 targetPos = billboardTarget.position + billboardTarget.TransformDirection(zoomOffset);
        Quaternion targetRot = Quaternion.LookRotation(billboardTarget.position - targetPos);

        while (Vector3.Distance(cameraTransform.position, targetPos) > 0.05f)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, Time.deltaTime * zoomSpeed);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRot, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        isMoving = false;
    }

    IEnumerator ReturnToOriginal()
    {
        isMoving = true;
        isZoomed = false;

        while (Vector3.Distance(cameraTransform.position, originalPosition) > 0.05f)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, originalPosition, Time.deltaTime * zoomSpeed);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, originalRotation, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        isMoving = false;
    }
}