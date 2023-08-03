using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    public int coinValue = 1;
    protected override void Action(GameObject player)
    {
        this.animator.SetFloat("coinSpeed", 4);
        this.rigid.AddForce(new Vector2(0, 2f), ForceMode2D.Impulse);

        GameManager.Instance.AddCoins(coinValue);
        this.rigid.gravityScale = 1;
        Destroy(this.gameObject, 0.5f);
    }
}
