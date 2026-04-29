using UnityEngine;

public class WaterWheelSpin : MonoBehaviour
{
    public Transform pivotPoint;
    public Vector3 rotationAxis = new Vector3(0f, 0f, 1f);

    public float maxSpeed = 40f;
    public float acceleration = 10f;

    private float currentSpeed = 0f;
    private float targetSpeed = 0f;

    void Update()
    {
        if (pivotPoint == null) return;

        // Smoothly move toward target speed
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        // Rotate
        transform.RotateAround(pivotPoint.position, rotationAxis, currentSpeed * Time.deltaTime);
    }

    public void SetSpeed(float speedPercent)
    {
        targetSpeed = maxSpeed * speedPercent;
    }
}