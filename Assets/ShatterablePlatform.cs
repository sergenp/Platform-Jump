using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterablePlatform : MonoBehaviour
{
    public GameObject Shattered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject shatteredObj = Instantiate(Shattered, transform.position, transform.rotation);
            Vector3 vel = other.gameObject.GetComponent<Rigidbody>().velocity;
            foreach(var rb in shatteredObj.GetComponentsInChildren<Rigidbody>())
            {
                rb.AddExplosionForce(vel.y * 1.5f, other.gameObject.transform.position, 10f, vel.y *0.8f * Mathf.Sign(vel.y), ForceMode.Impulse);
            }
            Destroy(shatteredObj, 10f);
            Destroy(gameObject);
        }
    }
}
