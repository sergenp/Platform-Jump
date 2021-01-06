using System.Collections;
using UnityEngine;

public class IncreaseMinJumpBuff : Pickable
{
    public override void Buff()
    {
        playerTransform.GetComponent<CubeJump>().MinJumpSpeedBuff();
        GameManager.instance.CreateFloatingText("Speed Boost");
        AudioManager.instance.PlayAudioOneShot("Powerup");
    }
}
