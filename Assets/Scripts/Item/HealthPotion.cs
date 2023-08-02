using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : ItemManager
{
    public int hp;

    protected override void Action(GameObject player)
    {
        player.GetComponent<PlayerManager>().AddHp(hp);

        this.animator.Play("HealthPotion");
        this.rigid.AddForce(new Vector2(0, 2f), ForceMode2D.Impulse);
        this.rigid.gravityScale = 1;
        Destroy(this.gameObject, 0.5f);
    }
}
