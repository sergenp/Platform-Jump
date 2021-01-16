using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterablePlatform : MonoBehaviour
{
    [Tooltip("Amount of coins to create")]
    public int coinAmount;
    [Tooltip("Value of each created coin")]
    public int coinValue;

    public GameObject Shattered;

    private GameObject player;

    private void Start()
    {
        player = GameManager.instance.GetPlayerTransform().gameObject;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject shatteredObj = Instantiate(Shattered, transform.position, transform.rotation);
            Vector3 vel = player.GetComponent<Rigidbody>().velocity;
            foreach(var rb in shatteredObj.GetComponentsInChildren<Rigidbody>())
            {
                rb.AddExplosionForce(vel.y * 1.25f, player.transform.position, 10f, vel.y * 0.7f * Mathf.Sign(vel.y), ForceMode.Impulse);
            }
            Destroy(shatteredObj, 10f);
            GameManager.instance.AddCoinToPlayer(coinValue, coinAmount);
            Destroy(gameObject);
        }
    }
}
