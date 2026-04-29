using System.Collections;
using UnityEngine;

public class DoorNewController : MonoBehaviour
{
    [Header("Door Positions")]
    public Vector3 closedPosition = new Vector3(-15.0407515f, -0.0598611832f, -4.07261753f);
    public Vector3 closedRotation = new Vector3(-0.0000019f, 331.927521f, 180f);

    public Vector3 openPosition = new Vector3(-18.7490005f, -0.00999999978f, -3.0250001f);
    public Vector3 openRotation = new Vector3(-0.0000019f, 269.111084f, 180f);

    public float moveDuration = 1f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;
    private bool isUnlocked = false;
    private bool isMoving = false;

    void Start()
    {
        transform.position = closedPosition;
        transform.rotation = Quaternion.Euler(closedRotation);
    }

    // Called from plate script
    public void UnlockDoor()
    {
        isUnlocked = true;
        Debug.Log("DOOR UNLOCKED");
    }

    // PC
    void OnMouseDown()
    {
        ToggleDoor();
    }

    // VR
    public void OpenDoor()
    {
        ToggleDoor();
    }

    void ToggleDoor()
    {
        if (!isUnlocked)
        {
            Debug.Log("Door is locked");
            return;
        }

        if (isMoving)
        {
            Debug.Log("Door is moving");
            return;
        }

        if (isOpen)
        {
            Debug.Log("CLOSING DOOR");

            //   Play close sound
            if (audioSource != null && closeSound != null)
                audioSource.PlayOneShot(closeSound);

            StartCoroutine(MoveDoor(closedPosition, closedRotation, false));
        }
        else
        {
            Debug.Log("OPENING DOOR");

            //   Play open sound
            if (audioSource != null && openSound != null)
                audioSource.PlayOneShot(openSound);

            StartCoroutine(MoveDoor(openPosition, openRotation, true));
        }
    }

    IEnumerator MoveDoor(Vector3 targetPos, Vector3 targetRot, bool opening)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Quaternion targetQuat = Quaternion.Euler(targetRot);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;

            Vector3 newPos = Vector3.Lerp(startPos, targetPos, t);
            Quaternion newRot = Quaternion.Lerp(startRot, targetQuat, t);

            transform.SetPositionAndRotation(newPos, newRot);

            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetQuat;

        isOpen = opening;
        isMoving = false;

        Debug.Log(opening ? "DOOR OPEN COMPLETE" : "DOOR CLOSE COMPLETE");
    }
}