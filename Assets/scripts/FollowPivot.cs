using UnityEngine;

public class FollowPivot : MonoBehaviour
{
    public Transform pivot;

    void Update()
    {
        if (pivot != null)
        {
            transform.rotation = pivot.rotation;
        }
    }
}