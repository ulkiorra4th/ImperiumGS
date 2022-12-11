using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(CharacterJump), typeof(CheckGround))]
public class CharacterMovement : MonoBehaviour
{
    #region Fields

    [SerializeField] private float speed = 5f;

    [Header("Run Settings")]
    [SerializeField] private float runSpeed;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;

    [Header("Sounds")]
    [SerializeField] private AudioSource stepSound;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource landingSound;

    public bool IsRunning { get; private set; }
    public float CurrentSpeed { get; private set; }

    [SerializeField] private Animator anim;

    private CharacterController controller;
    private CharacterJump characterJump;
    private CheckGround checkGround;

    private Vector3 direction;
    private Vector3 lastPosition;

    private Vector3 currentMoveVelocity;

    private float smoothMovingVelocity = 0.07f;
    private float smoothMovingAnimVelocity = 0.05f;
    private float smoothRunningAnimVelocity = 0.02f;

    private float animSpeedValue = 0f;

    private float smoothTime;
    private float smoothVelocity;

    #endregion

    #region Unity

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        controller = GetComponent<CharacterController>();
        characterJump = GetComponent<CharacterJump>();
        checkGround = GetComponent<CheckGround>();

        lastPosition = transform.position;
    }

    private void Update()
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        IsRunning = Input.GetKey(runKey) && checkGround.IsGrounded;

        if (direction.magnitude != 0)
        {
            if (!stepSound.isPlaying) stepSound.Play();

            var rotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.rotation.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, rotationAngle, ref smoothVelocity, smoothTime);

            transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);

            Vector3 moveVelocity = (Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward).normalized;
            currentMoveVelocity = !IsRunning ? Vector3.Lerp(currentMoveVelocity, moveVelocity * speed, smoothMovingVelocity) : Vector3.Lerp(currentMoveVelocity, moveVelocity * runSpeed, smoothMovingVelocity);

            controller.Move(currentMoveVelocity * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && checkGround.IsGrounded) characterJump.Jump();
    }

    private void LateUpdate()
    {
        CurrentSpeed = Mathf.Round(Vector3.Distance(transform.position, lastPosition) / Time.deltaTime);

        animSpeedValue = !IsRunning ? Mathf.Lerp(animSpeedValue, CurrentSpeed, smoothMovingAnimVelocity * Time.deltaTime * 120) : Mathf.Lerp(animSpeedValue, CurrentSpeed, smoothRunningAnimVelocity * 90 * Time.deltaTime);
        if (direction.magnitude == 0) animSpeedValue = Mathf.Lerp(animSpeedValue, 0, smoothMovingAnimVelocity * Time.deltaTime * 30);
        if (!IsRunning && CurrentSpeed > 2) animSpeedValue = Mathf.Lerp(animSpeedValue, speed, smoothMovingAnimVelocity * Time.deltaTime * 30);
        anim.SetFloat("speed", animSpeedValue);

        lastPosition = transform.position;
    }

    #endregion
}