using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPortion : ItemManager
{
    public int jumpPower;
    public float buffTime;

    protected override void Action(GameObject Buff)
    {
        Buff.GetComponent<BuffManager>().BuffStart("jumpPower", time: buffTime, jumpPower: jumpPower);

        this.animator.Play("JumpPortion");
        this.rigid.AddForce(new Vector2(0, 2f), ForceMode2D.Impulse);
        this.rigid.gravityScale = 1;
        Destroy(this.gameObject, 0.5f);
    }
}
