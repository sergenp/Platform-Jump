using UnityEngine;

public class RotateAroundPlayer : MonoBehaviour
{

    public Transform rotateTransform;

    public float rotateSpeed = 0.5f;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(60f, -60f, Mathf.PingPong(Time.time * rotateSpeed, 1f)));
        transform.position = rotateTransform.position + new Vector3(Mathf.Lerp(-2.5f, 2.5f, Mathf.PingPong(Time.time * rotateSpeed, 1f)), 4f, 0f);
    }

    private void DisableArrow() {
        _animator.SetBool("Disabled", true);
    }

    private void EnableArrow()
    {
        _animator.SetBool("Disabled", false);
    }
}
