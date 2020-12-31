using UnityEngine;

public class RotateAroundPlayer : MonoBehaviour
{

    public Transform rotateTransform;

    public float rotateSpeed = 0.5f;

    public float maxX = 3f;

    public float maxXAngle = 60f;

    // arrow that looks exactly like this object and gets created in the rotation and position of cube's jump and fades away slowly
    public GameObject fakeArrow;

    private void OnEnable()
    {
        CubeJump cubeJump = FindObjectOfType<CubeJump>();
        if (cubeJump != null)
        {
            cubeJump.jumped += CreateFakeArrow;
        }
    }
    private void OnDisable()
    {
        CubeJump cubeJump = FindObjectOfType<CubeJump>();
        if (cubeJump != null)
        {
            cubeJump.jumped -= CreateFakeArrow;
        }
    }

    void Update()
    {
        transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(maxXAngle, -maxXAngle, Mathf.PingPong(Time.time * rotateSpeed, 1f)));
        transform.position = rotateTransform.position + new Vector3(Mathf.Lerp(-maxX, maxX, Mathf.PingPong(Time.time * rotateSpeed, 1f)), 4f, 0f);
    }

    private void CreateFakeArrow() {
        Instantiate(fakeArrow, transform.position, transform.rotation);
    }
}
