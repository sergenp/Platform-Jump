using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Pickable
{
    public int value;

    public override void Buff()
    {
        GameManager.instance.AddCoinToPlayer(value);
    }
}
