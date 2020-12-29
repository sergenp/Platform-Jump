using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float explosionForce;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, this.transform.position, explosionForce, explosionForce, ForceMode.Impulse);
            Destroy(gameObject);
        }        
    }
}
