using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [Header("InteractableInfo")]
    public float sphereCastRadius = 0.5f;
    public int interactableLayerIndex;
    public GameObject lookObject;
    private FPSGrab physicsObject;
    [SerializeField] Camera mainCamera;

    [Header("Pickup")]
    [SerializeField] private Transform pickupParent;
    public GameObject currentlyPickedUpObject;
    private Rigidbody pickupRB;

    [Header("ObjectFollow")]
    [SerializeField] private float minSpeed = 0;
    [SerializeField] private float maxSpeed = 300f;
    [SerializeField] private float maxDistance = 10f;
    private float currentSpeed = 0f;
    private float currentDist = 0f;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    Quaternion lookRot;

    private void OnDrawGizmos()
    {
        if (pickupParent != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(pickupParent.position, 0.2f);
        }
    }

    void Update()
    {
        // 🔥 FIXED RAY START POSITION
        Vector3 rayOrigin = mainCamera.transform.position;
        Vector3 direction = mainCamera.transform.forward;

        RaycastHit hit;

        Debug.DrawRay(rayOrigin, direction * maxDistance, Color.green);

        if (Physics.SphereCast(rayOrigin, sphereCastRadius, direction, out hit, maxDistance, 1 << interactableLayerIndex))
        {
            lookObject = hit.collider.transform.root.gameObject;
            Debug.Log("Looking at: " + lookObject.name);
        }
        else
        {
            lookObject = null;
        }
    }

    public void OnGrabPressed()
    {
        Debug.Log("GRAB PRESSED");

        if (currentlyPickedUpObject == null)
        {
            if (lookObject != null)
            {
                Debug.Log("Picking up: " + lookObject.name);
                PickUpObject();
            }
            else
            {
                Debug.Log("Nothing to pick up");
            }
        }
        else
        {
            Debug.Log("Dropping object");
            BreakConnection();
        }
    }

    private void FixedUpdate()
    {
        if (currentlyPickedUpObject != null)
        {
            currentDist = Vector3.Distance(pickupParent.position, pickupRB.position);
            currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist / maxDistance);
            currentSpeed *= Time.fixedDeltaTime;

            Vector3 direction = pickupParent.position - pickupRB.position;
            pickupRB.linearVelocity = direction.normalized * currentSpeed;

            lookRot = Quaternion.LookRotation(mainCamera.transform.position - pickupRB.position);
            lookRot = Quaternion.Slerp(mainCamera.transform.rotation, lookRot, rotationSpeed * Time.fixedDeltaTime);
            pickupRB.MoveRotation(lookRot);
        }
    }

    public void BreakConnection()
    {
        pickupRB.constraints = RigidbodyConstraints.None;
        currentlyPickedUpObject = null;

        if (physicsObject != null)
            physicsObject.pickedUp = false;

        currentDist = 0;
    }

    public void PickUpObject()
    {
        physicsObject = lookObject.GetComponentInChildren<FPSGrab>();

        if (physicsObject == null)
        {
            Debug.LogError("NO FPSGrab FOUND ON OBJECT!");
            return;
        }

        pickupRB = lookObject.GetComponent<Rigidbody>();

        if (pickupRB == null)
        {
            Debug.LogError("NO RIGIDBODY FOUND ON OBJECT!");
            return;
        }

        currentlyPickedUpObject = lookObject;

        pickupRB.constraints = RigidbodyConstraints.FreezeRotation;
        physicsObject.playerInteractions = this;

        StartCoroutine(physicsObject.PickUp());
    }
}