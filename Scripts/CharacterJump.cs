using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    private readonly float GravityScale = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;

    [SerializeField] private Animator anim;
    [SerializeField] private float jumpHeight = 1.0f; 

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (controller.isGrounded && playerVelocity.y < 0) playerVelocity.y = 0f;

        playerVelocity.y += GravityScale * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * GravityScale);
        anim.SetTrigger("jump");
    }
}