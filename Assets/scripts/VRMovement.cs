using UnityEngine;
using UnityEngine.InputSystem;

public class VRMovement : MonoBehaviour
{
    public float speed = 2f;
    public Transform head;

    public InputActionProperty moveInput;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector2 input = moveInput.action.ReadValue<Vector2>();

        Vector3 direction = new Vector3(input.x, 0, input.y);

        // Move relative to where your head is facing
        direction = head.forward * direction.z + head.right * direction.x;
        direction.y = 0;

        controller.Move(direction * speed * Time.deltaTime);
    }
}