using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterManager : MonoBehaviour
{
    protected PlayerManager playerManager = PlayerManager.Instance;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected bool isGround;
    [SerializeField] Transform rayPoint;
    [SerializeField] protected int maxHP;
    [SerializeField] protected int hp;
    [SerializeField] protected int atk;
    [SerializeField] protected int speed;

    protected enum State { Idle, Chase, Attack, Killed}
    protected State state;

    protected IEnumerator StateMachine()
    {
        while (hp > 0) 
        {
            yield return StartCoroutine(state.ToString());
        }

        StartCoroutine(Killed());
    }

    protected abstract IEnumerator Idle();
    protected abstract IEnumerator Chase();
    protected abstract IEnumerator Attack();
    protected abstract IEnumerator Killed();

    protected void ChangeState(State newState)
    {
        state = newState;
    }
        

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

    public abstract void DamageByBullet();
    public abstract void DamageToWeakPoint();

    public int GetAtk() { return atk; }
    public void AddAtk(int atk) { this.atk += atk; }

    public int GetHp() { return hp; }
    public void AddHp(int hp) { this.hp += hp;}

    public int GetSpeed() { return speed; }
    public void AddSpeed(int speed) {  this.speed += speed;}
}
