using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpReset : Pickable
{

    private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            GotoPlayer();
        }
    }

    public override void Buff()
    {
        player.GetComponent<CubeJump>().ResetJumpCount();
    }
}
