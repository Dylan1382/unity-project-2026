using UnityEngine;

public class Switch2SnapPart : MonoBehaviour
{
    public Transform snapPoint_2_1;
    public float snapDistance = 0.3f;

    public LeverSwitch leverScript; // your existing lever script

    private bool isSnapped = false;

    void Start()
    {
        // Make sure lever can't be used before snapping
        if (leverScript != null)
        {
            leverScript.enabled = false;
        }
    }

    void Update()
    {
        if (isSnapped || snapPoint_2_1 == null)
            return;

        float distance = Vector3.Distance(transform.position, snapPoint_2_1.position);

        if (distance <= snapDistance)
        {
            SnapToSwitch();
        }
    }

    void SnapToSwitch()
    {
        isSnapped = true;

        transform.position = snapPoint_2_1.position;
        transform.rotation = snapPoint_2_1.rotation;

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;
            rb.useGravity = false;
        }

        transform.SetParent(snapPoint_2_1);

        var grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != null)
        {
            grab.enabled = false;
        }

        if (leverScript != null)
        {
            leverScript.enabled = true;
        }
    }
}