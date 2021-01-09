using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotateSpeed;

    Vector3 rotateVector = Vector3.right;

    private void Start()
    {
        StartCoroutine(Rotator());
    }

    private void Update()
    {
        transform.Rotate(rotateVector * rotateSpeed * Time.deltaTime);
    }

    IEnumerator Rotator()
    {
        int rotateAroundAxis = UnityEngine.Random.Range(0, 3);
        while (true)
        {
            if (rotateAroundAxis == 0)
            {
                rotateVector = Vector3.right;
                yield return new WaitForSeconds(12f);
            }
            else if (rotateAroundAxis == 1)
            {
                rotateVector = Vector3.up;
                yield return new WaitForSeconds(12f);
            } 
            else
            {
                rotateVector = Vector3.forward;
                yield return new WaitForSeconds(12f);
                rotateAroundAxis = 0;
            }
            rotateAroundAxis++;
        }
    }
}
