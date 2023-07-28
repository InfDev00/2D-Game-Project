using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected PlayerManager playerManager = PlayerManager.Instance;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected bool isGround;
    [SerializeField] Transform raypoint;
    [SerializeField] protected int hp;
    [SerializeField] protected int atk;
    [SerializeField] protected int speed;

    public void DealDamage()
    {
        //playerManager.
    }

    public bool CheckGround()
    {

        Debug.DrawRay(raypoint.transform.position,Vector3.down, Color.blue);
        if (Physics2D.Raycast(raypoint.transform.position, Vector3.down, 5))
        {
            return true;
        }
        else return false;

    }

    public abstract void DamageByBulet();
}
