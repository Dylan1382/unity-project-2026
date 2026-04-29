using UnityEngine;

public class BedInteract : MonoBehaviour
{
    public Transform xrOrigin;        // XR Origin (XR Rig)
    public Transform cameraOffset;    // Camera Offset (inside XR Origin)
    public Transform liePosition;     // Bed_Lie_Position

    private Vector3 originalPos;
    private Quaternion originalRot;
    private Vector3 originalCameraOffset;

    private bool isLying = false;

    public void ToggleBed()
    {
        if (!isLying)
        {
            // Save current position
            originalPos = xrOrigin.position;
            originalRot = xrOrigin.rotation;
            originalCameraOffset = cameraOffset.localPosition;

            // Move player to bed
            xrOrigin.position = liePosition.position;
            xrOrigin.rotation = liePosition.rotation;

            // Flatten camera so you don't stand up
            cameraOffset.localPosition = Vector3.zero;

            isLying = true;
        }
        else
        {
            // Return to original position
            xrOrigin.position = originalPos;
            xrOrigin.rotation = originalRot;
            cameraOffset.localPosition = originalCameraOffset;

            isLying = false;
        }
    }
}