using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterManager : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;

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

    public abstract void Damaged(int damage);

    public int GetAtk() { return atk; }
    public void AddAtk(int atk) { this.atk += atk; }

    public int GetHp() { return hp; }
    public void AddHp(int hp) { this.hp += hp;}

    public int GetSpeed() { return speed; }
    public void AddSpeed(int speed) {  this.speed += speed;}
}
