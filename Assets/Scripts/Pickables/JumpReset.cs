using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpReset : Pickable
{
    public override void Buff()
    {
        playerTransform.GetComponent<CubeJump>().ResetJumpCount();
    }
}
