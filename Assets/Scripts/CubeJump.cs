using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class CubeJump : MonoBehaviour
{

    public GameObject JumpTarget;

    public GameObject GraphicsCube;

    [Header("Jump Settings")]

    public float minJumpSpeed = 15f;

    public float maxJumpSpeed = 35f;

    public float jumpForceGainMultiplier = 1f;

    public int maxInAirJumpCount = 2;

    [Header("Ground Detection")]
    public Transform feet;

    public LayerMask GroundMask;

    [Header("Graphics Materials")]
    public Material defaultMaterial;
    public Material ChargingJumpMat;


    [Header("Particle Systems")]
    public ParticleSystem dustParticle;


    public delegate void JumpDelegate();
    public JumpDelegate jumpCharging;
    public JumpDelegate jumped;

    private Animator _animator;
    private Rigidbody _rb;

    private Renderer cubeRenderer;

    private bool jumpTriggered = false;
    private bool canJump = true;
    private float jumpForce;
    private int jumpCount;

    void Start()
    {
        jumpForce = minJumpSpeed;
        _rb = GetComponent<Rigidbody>();
        _animator = GraphicsCube.GetComponent<Animator>();
        cubeRenderer = GraphicsCube.GetComponent<Renderer>();
        jumpCount = maxInAirJumpCount;
    }

    void Update()
    {
        canJump = Physics.Raycast(feet.position, Vector3.down * 2, 2f, GroundMask);
        _animator.SetBool("IsGrounded", canJump);
        Debug.DrawRay(feet.position, Vector3.down * 2, Color.red);

        if (Input.GetMouseButton(0) && canJump)
        {
            jumpForce += Time.deltaTime * 10 * jumpForceGainMultiplier;
            jumpForce = Mathf.Clamp(jumpForce, minJumpSpeed, maxJumpSpeed);
            _animator.SetFloat("Jump Force", jumpForce);
            _animator.speed = 1f + (jumpForce / 70f);
            cubeRenderer.material.Lerp(cubeRenderer.material, ChargingJumpMat, Time.deltaTime * _animator.speed);
            jumpCharging?.Invoke();
        }

        if (Input.GetMouseButtonUp(0) && (canJump || jumpCount > 0))
        {
            jumpTriggered = true;
            CreateDust();
            jumped?.Invoke();
        }

        if (canJump)
            jumpCount = maxInAirJumpCount;
        else
            cubeRenderer.material.Lerp(cubeRenderer.material, defaultMaterial, Time.deltaTime * 3);
        if (jumpCount >= 1)
            JumpTarget.SetActive(true);
        else
            JumpTarget.SetActive(false);
        

        GraphicsCube.transform.localEulerAngles = Vector3.Lerp(GraphicsCube.transform.localEulerAngles, new Vector3(0f, 0f, transform.eulerAngles.z), 3f);
    }

    void FixedUpdate()
    {
        if (jumpTriggered)
        {
            Jump();
            jumpTriggered = false;
            jumpCount--;
        }
    }
    void Jump()
    {
        Vector3 jumpDir = (JumpTarget.transform.position - transform.position).normalized * jumpForce;
        _rb.velocity = new Vector3(jumpDir.x, jumpForce, 0f);
        _rb.angularVelocity = new Vector3(0f, 0f, Mathf.Clamp(-jumpDir.x, -2f, 2f));
        jumpForce = minJumpSpeed;
        _animator.SetFloat("Jump Force", jumpForce);
        _animator.speed = 1f;
    }

    void CreateDust()
    {
        dustParticle.Play();
    }
}
