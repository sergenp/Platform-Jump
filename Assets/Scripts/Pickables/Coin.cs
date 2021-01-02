using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Pickable
{
    public int value;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GotoPlayer();
        }
    }

    public override void Buff()
    {
        GameManagerScript.instance.AddCoinToPlayer(value);
    }
}
