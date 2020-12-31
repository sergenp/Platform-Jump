using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [Header("Camera Follow Settings")]
    public Transform gameObjectToFollow;

    public float smoothTime;
    public Vector3 cameraOffset = Vector3.zero;
    public Vector3 rotation;

    public bool lookAtGameObject = true;

    [Header("Camera Shake")]
    public bool shakeCam = false;

    public float shakeFactor = 0.5f;


    private Vector3 velocity = Vector3.zero;

    private void OnEnable()
    {
        CubeJump cubeJump = FindObjectOfType<CubeJump>();
        if (cubeJump != null)
        {
            cubeJump.jumped += StopShaking;
            cubeJump.jumpCharging += ShakeCam;
        }
    }

    private void OnDisable()
    {
        CubeJump cubeJump = FindObjectOfType<CubeJump>();
        if (cubeJump != null)
        {
            cubeJump.jumped -= StopShaking;
            cubeJump.jumpCharging -= ShakeCam;
        }
    }

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, gameObjectToFollow.position.y + cameraOffset.y, transform.position.z);
    }

    void LateUpdate()
    {
        // if the object isn't destroyed, follow it
        if (gameObjectToFollow != null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, 
                new Vector3(transform.position.x, gameObjectToFollow.position.y + cameraOffset.y, transform.position.z), ref velocity, smoothTime);

            if (shakeCam) 
            {
                transform.position = transform.position + Random.insideUnitSphere * shakeFactor;
            }

            //Vector3.Slerp(transform.position, newPos, smoothMultiplier * Time.deltaTime);

            if (lookAtGameObject)
            {
                transform.LookAt(gameObjectToFollow);
            }
        }
    }

    public void ShakeCam() 
    {
        shakeCam = true;
    }

    public void StopShaking()
    {
        shakeCam = false;
    }
}