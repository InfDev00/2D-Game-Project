using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPortion : ItemManager
{
    public int heal;

    protected override void Action(GameObject Buff)
    {
        Buff.GetComponent<BuffManager>().BuffStart("heal", heal: heal);

        this.animator.Play("HealthPortion");
        this.rigid.AddForce(new Vector2(0, 2f), ForceMode2D.Impulse);
        this.rigid.gravityScale = 1;
        Destroy(this.gameObject, 0.5f);
    }
}
