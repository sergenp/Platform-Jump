using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UpgradeNames
{
    MinJumpSpeed,
    MaxJumpSpeed,
    JumpForceGainMultiplier,
    MaxInAirJumpCount,
    ArrowRotationSpeed
}

[RequireComponent(typeof(Rigidbody))]
public class CubeJump : MonoBehaviour
{

    [Header("Graphics")]
    public GameObject JumpTarget;

    public GameObject GraphicsCube;

    public GameObject DestroyedCube;

    public Dictionary<UpgradeNames, float> Stats;

    [Header("Ground Detection")]
    public Transform feet;

    public LayerMask GroundMask;

    [Header("Graphics Materials")]
    public Material ChargingJumpMat;
    private Material defaultMaterial;


    [Header("Particle Systems")]
    public ParticleSystem dustParticle;
    public GameObject smokeParticle;

    [Header("Colors for Floating Texts")]
    public Color jumpCountTextColor;
    public Color fullyChargedTextColor;
    public Color buffEndTextColor;


    [Header("Buff Settings")]
    public int infiniteJumpBuffSeconds = 5;
    public int minimumJumpSpeedBuffSeconds = 5;
    public int jumpChargeBuffSeconds = 5;

    public delegate void JumpDelegate();
    public JumpDelegate jumpCharging;
    public JumpDelegate jumped;

    public delegate void PlayerDiedDelegate();
    public PlayerDiedDelegate playerDied;

    private Animator _animator;
    private Rigidbody _rb;

    private Renderer cubeRenderer;

    private bool jumpTriggered = false;
    private bool isGrounded = true;
    private float jumpForce;
    private int jumpCount;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GraphicsCube.GetComponent<Animator>();
        cubeRenderer = GraphicsCube.GetComponent<Renderer>();
        cubeRenderer.material.color = PlayerDataManager.instance.GetCubeColor();
        defaultMaterial = new Material(cubeRenderer.material);

        Stats = UpgradesManager.instance.ConvertStatsToDictionary();
        jumpCount = (int) Stats[UpgradeNames.MaxInAirJumpCount];
        jumpForce = Stats[UpgradeNames.MinJumpSpeed];
        JumpTarget.GetComponent<RotateAroundPlayer>().rotateSpeed = Stats[UpgradeNames.ArrowRotationSpeed];
        StartCoroutine(NotifyFullyCharged());
    }

    void Update()
    {
        isGrounded = Physics.Raycast(feet.position, Vector3.down * 2, 2f, GroundMask);
        _animator.SetBool("IsGrounded", isGrounded);

        if (Input.GetMouseButton(0) && isGrounded)
        {        
            jumpForce = Mathf.Lerp(jumpForce, Stats[UpgradeNames.MaxJumpSpeed], Stats[UpgradeNames.JumpForceGainMultiplier] / 20);
            _animator.SetBool("IsCharging", true);
            _animator.speed = 1f + (jumpForce / 50f);
            cubeRenderer.material.Lerp(cubeRenderer.material, ChargingJumpMat, Stats[UpgradeNames.JumpForceGainMultiplier] / 20);
            jumpCharging?.Invoke();
        }

        if (Input.GetMouseButtonUp(0) && (isGrounded || jumpCount > 0))
        {
            jumpTriggered = true;
            _animator.SetBool("IsCharging", false);
            dustParticle.Play();
            AudioManager.instance.PlayAudioOneShot("Player Jump");
            jumpCount--;
            if (jumpCount == 1)
                GameManager.instance.CreateFloatingText("Last Jump", jumpCountTextColor);
            else
                GameManager.instance.CreateFloatingText($"{jumpCount}", jumpCountTextColor);
            jumped?.Invoke();
        }

        if (isGrounded)
        {
            ResetJumpCount();
            if (!_animator.GetBool("IsCharging"))
                cubeRenderer.material.Lerp(cubeRenderer.material, defaultMaterial, .2f);
        }
        else
        {
            if (_rb.velocity.y >= -0.5f)
            {
                cubeRenderer.material.Lerp(cubeRenderer.material, defaultMaterial, .2f);
            }
            else if (_rb.velocity.y <= -18f)
            {
                cubeRenderer.material.Lerp(cubeRenderer.material, ChargingJumpMat, .03f);
            }
        }

        if (jumpCount > 0)
            JumpTarget.SetActive(true);
        else
            JumpTarget.SetActive(false);

        GraphicsCube.transform.localEulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z);
    
        if (_rb.velocity.y < -32f)
        {
            DestroyedCube.SetActive(true);
            foreach (var rb in DestroyedCube.GetComponentsInChildren<Rigidbody>())
            {
                rb.AddExplosionForce(20f, transform.position, 20f, 0f, ForceMode.Impulse);
            }
            AudioManager.instance.PlayAudioOneShot("Mine");
            playerDied?.Invoke();
            KillCube();
        }
    }
    void FixedUpdate()
    {
        if (jumpTriggered)
        {
            Jump();
            jumpTriggered = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Platform"))
        {
            Instantiate(smokeParticle, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
        }
    }

    private IEnumerator NotifyFullyCharged()
    {
        while (true)
        {
            if (Mathf.Abs(jumpForce-Stats[UpgradeNames.MaxJumpSpeed]) <= 0.1f)
            {
                GameManager.instance.CreateFloatingText("Fully Charged", fullyChargedTextColor);
                yield return new WaitForSeconds(5f);
            }
            yield return null;
        }
    }

    public void KillCube()
    {
        Destroy(GetComponent<BoxCollider>());
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        Destroy(GraphicsCube);
        Destroy(JumpTarget);
        Destroy(this);
    }

    public void ResetJumpCount()
    {
        jumpCount = (int) Stats[UpgradeNames.MaxInAirJumpCount];
    }

    void Jump()
    {
        Vector3 jumpDir = (JumpTarget.transform.position - transform.position).normalized * jumpForce;
        _rb.velocity = new Vector3(jumpDir.x, jumpForce, 0f);
        _rb.angularVelocity = new Vector3(0f, 0f, Mathf.Clamp(-jumpDir.x, -2f, 2f));
        jumpForce = Stats[UpgradeNames.MinJumpSpeed];
        _animator.speed = 1f;
    }

    /// <summary>
    /// Buffs that can be applied to player by pickables (or other things)
    /// </summary>

    public void InfiniteJumpsBuff()
    {
        StartCoroutine(InfiniteJumpsBuffCoroutine());
    }

    public void MinJumpSpeedBuff()
    {
        StartCoroutine(MinJumpSpeedBuffCoroutine());
    }    
    
    public void JumpChargeBuff()
    {
        StartCoroutine(JumpChargeBuffCoroutine());
    }

    private IEnumerator JumpChargeBuffCoroutine()
    {
        float prevChargeSpeed = Stats[UpgradeNames.JumpForceGainMultiplier];
        Stats[UpgradeNames.JumpForceGainMultiplier] = 20;
        yield return new WaitForSeconds(jumpChargeBuffSeconds);
        Stats[UpgradeNames.JumpForceGainMultiplier] = prevChargeSpeed;
        GameManager.instance.CreateFloatingText("Instant Charge End", buffEndTextColor);
    }

    IEnumerator InfiniteJumpsBuffCoroutine()
    {
        int prevMaxJumpCount = (int) Stats[UpgradeNames.MaxInAirJumpCount];
        int prevJumpCount = jumpCount;
        Stats[UpgradeNames.MaxInAirJumpCount] = 9999;
        ResetJumpCount();
        yield return new WaitForSeconds(infiniteJumpBuffSeconds);
        Stats[UpgradeNames.MaxInAirJumpCount] = prevMaxJumpCount;
        jumpCount = prevMaxJumpCount;
        GameManager.instance.CreateFloatingText("Infinite Jump End", buffEndTextColor);
    }

    IEnumerator MinJumpSpeedBuffCoroutine()
    {
        float prevMinSpeed = (int) Stats[UpgradeNames.MinJumpSpeed];
        float prevMaxSpeed = (int) Stats[UpgradeNames.MaxJumpSpeed];
        Stats[UpgradeNames.MinJumpSpeed] = Stats[UpgradeNames.MaxJumpSpeed];
        Stats[UpgradeNames.MaxJumpSpeed] += Stats[UpgradeNames.MaxJumpSpeed];
        yield return new WaitForSeconds(minimumJumpSpeedBuffSeconds);
        Stats[UpgradeNames.MinJumpSpeed] = prevMinSpeed;
        Stats[UpgradeNames.MaxJumpSpeed] = prevMaxSpeed;
        jumpForce = prevMinSpeed;
        GameManager.instance.CreateFloatingText("Jump Buff End", buffEndTextColor);
    }
}
