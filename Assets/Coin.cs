using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int amount;

    private bool goToPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            goToPlayer = true;
        }
    }

    private void Update()
    {
        if (goToPlayer)
        {
            Transform playerTransform = GameManagerScript.instance.GetPlayerTransform();
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, 20f*Time.deltaTime);
            if (Vector3.Distance(transform.position, playerTransform.position) <= 0.5f)
            {
                GameManagerScript.instance.AddCoinToPlayer(amount);
                Destroy(gameObject);
            }
        }
    }
}
