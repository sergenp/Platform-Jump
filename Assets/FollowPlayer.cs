using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [Header("Camera Follow Settings")]
    public Transform gameObjectToFollow;

    public Vector3 cameraOffset = Vector3.zero;
    public Vector3 rotation;

    public bool lookAtGameObject = true;

    public bool shakeCam = false;

    public float shakeFactor = 0.5f;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, gameObjectToFollow.position.y + cameraOffset.y, transform.position.z);
        //this.transform.eulerAngles = rotation;
    }

    void LateUpdate()
    {
        // if the object isn't destroyed, follow it
        if (gameObjectToFollow != null)
        {
            transform.position = new Vector3(transform.position.x, gameObjectToFollow.position.y + cameraOffset.y, transform.position.z);
            //this.transform.eulerAngles = rotation;

            if (shakeCam) 
            {
                this.transform.position = this.transform.position + Random.insideUnitSphere * shakeFactor;
            }

            //Vector3.Slerp(transform.position, newPos, smoothMultiplier * Time.deltaTime);

            if (lookAtGameObject)
            {
                this.transform.LookAt(gameObjectToFollow);
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