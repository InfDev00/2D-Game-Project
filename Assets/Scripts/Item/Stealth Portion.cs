using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthPortion : ItemManager
{
    public float buffTime;

    protected override void Action(GameObject Buff)
    {
        Buff.GetComponent<BuffManager>().BuffStart("stealth", time: buffTime);

        this.animator.Play("StealthPortion");
        this.rigid.AddForce(new Vector2(0, 2f), ForceMode2D.Impulse);
        this.rigid.gravityScale = 1;
        Destroy(this.gameObject, 0.5f);
    }
}
