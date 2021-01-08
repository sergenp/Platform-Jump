using System.Collections;
using UnityEngine;

public class ChargeFasterBuff : Pickable
{
    public override void Buff()
    {
        playerTransform.GetComponent<CubeJump>().JumpChargeBuff();
        GameManager.instance.CreateFloatingText("Instant Charge");
        AudioManager.instance.PlayAudioOneShot("Powerup");
    }
}
