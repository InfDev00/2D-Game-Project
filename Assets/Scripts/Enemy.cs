using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected PlayerManager playerManager = PlayerManager.Instance;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected bool isGround;
    [SerializeField] Transform rayPoint;
    [SerializeField] protected int hp;
    [SerializeField] protected int atk;
    [SerializeField] protected int speed;

    public void DealDamage()
    {
        //playerManager.
    }

    public bool CheckGround()
    {

        Debug.DrawRay(rayPoint.transform.position,Vector3.down, Color.blue);
        if (Physics2D.Raycast(rayPoint.transform.position, Vector3.down, 5))
        {
            return true;
        }
        else return false;

    }

    public int DistanceToPlayer()
    {
        float distance = Vector3.Distance(this.transform.position, playerManager.transform.position);
        return (int)distance;
    }

    public abstract void DamageByBulet();
    public abstract void DamageToWeakPoint();
}
