using System.Collections;
using UnityEngine;

public class InfiniteJumpsBuff : Pickable
{
    public override void Buff()
    {
        playerTransform.GetComponent<CubeJump>().InfiniteJumpsBuff();
        GameManager.instance.CreateFloatingText("Infinite Jumps");
        AudioManager.instance.PlayAudioOneShot("Powerup");
    }
}