using UnityEngine;

public class FollowWheelPivot : MonoBehaviour
{
    public Transform pivot;

    void LateUpdate()
    {
        if (pivot != null)
        {
            transform.position = pivot.position;
            transform.rotation = pivot.rotation;
        }
    }
}