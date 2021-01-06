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
    public Material defaultMaterial;
    public Material ChargingJumpMat;


    [Header("Particle Systems")]
    public ParticleSystem dustParticle;


    [Header("Buff Settings")]
    public int infiniteJumpBuffSeconds = 5;
    public int minimumJumpSpeedBuffSeconds = 5;

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

        Stats = UpgradesManager.instance.ConvertStatsToDictionary();
        jumpCount = (int) Stats[UpgradeNames.MaxInAirJumpCount];
        jumpForce = Stats[UpgradeNames.MinJumpSpeed];
        JumpTarget.GetComponent<RotateAroundPlayer>().rotateSpeed = Stats[UpgradeNames.ArrowRotationSpeed];
    }

    void Update()
    {
        isGrounded = Physics.Raycast(feet.position, Vector3.down * 2, 2f, GroundMask);
        _animator.SetBool("IsGrounded", isGrounded);

        if (Input.GetMouseButton(0) && isGrounded)
        {
            jumpForce += Time.deltaTime * 10 * Stats[UpgradeNames.JumpForceGainMultiplier];
            jumpForce = Mathf.Clamp(jumpForce, Stats[UpgradeNames.MinJumpSpeed], Stats[UpgradeNames.MaxJumpSpeed]);
            _animator.SetBool("IsCharging", true);
            _animator.speed = 1f + (jumpForce / 50f);
            cubeRenderer.material.Lerp(cubeRenderer.material, ChargingJumpMat, _animator.speed/40f);
            jumpCharging?.Invoke();
        }

        if (Input.GetMouseButtonUp(0) && (isGrounded || jumpCount > 0))
        {
            jumpTriggered = true;
            _animator.SetBool("IsCharging", false);
            dustParticle.Play();
            AudioManager.instance.PlayAudioOneShot("Player Jump");
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
            else if (_rb.velocity.y <= -15f)
            {
                cubeRenderer.material.Lerp(cubeRenderer.material, ChargingJumpMat, .04f);
            }
        }

        if (jumpCount >= 1)
            JumpTarget.SetActive(true);
        else
            JumpTarget.SetActive(false);

        GraphicsCube.transform.localEulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z);
    
        if (_rb.velocity.y < -30f)
        {
            DestroyedCube.SetActive(true);
            foreach (var rb in DestroyedCube.GetComponentsInChildren<Rigidbody>())
            {
                rb.AddExplosionForce(50f, transform.position, 20f, -rb.velocity.y);
            }
            _rb.isKinematic = true;
            Destroy(GraphicsCube);
            Destroy(JumpTarget.gameObject);
            Destroy(this);
            AudioManager.instance.PlayAudioOneShot("Mine");
            playerDied?.Invoke();
        }
    }


    public void ResetJumpCount()
    {
        jumpCount = (int) Stats[UpgradeNames.MaxInAirJumpCount];
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
        jumpForce = Stats[UpgradeNames.MinJumpSpeed];
        _animator.speed = 1f;
    }

    public void ApplyUpgrades(List<UpgradesManager.PlayerUpgrades> upgrades)
    {
        foreach (var upgrade in upgrades)
        {
            Stats[upgrade.upgradeName] = upgrade.upgradeValue;
        }
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

    IEnumerator InfiniteJumpsBuffCoroutine()
    {
        int prevMaxJumpCount = (int) Stats[UpgradeNames.MaxInAirJumpCount];
        int prevJumpCount = jumpCount;
        Stats[UpgradeNames.MaxInAirJumpCount] = 9999;
        ResetJumpCount();
        yield return new WaitForSeconds(infiniteJumpBuffSeconds);
        Stats[UpgradeNames.MaxInAirJumpCount] = prevMaxJumpCount;
        jumpCount = prevJumpCount;
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
    }
}
