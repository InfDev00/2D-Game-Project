using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraManager : MonoBehaviour
{
    public float movePower = 1f;
    private Animator animator;
    private Transform target;
    private enum movementFlag { Idle = 0, Move, Attack };
    private movementFlag currentFlag = movementFlag.Idle;
    
    private enum mode { normal = 0, angry};
    private mode currentMode = mode.normal;

    public GameObject EggPrefab;

    private int HP = 10;

    private GameObject eraImg;
    private EraAttack eraAttack;


    void Awake()
    {
        eraImg = transform.Find("EraImg").gameObject;
        eraAttack = transform.Find("EraAttack").gameObject.GetComponent<EraAttack>();
        animator = eraImg.GetComponent<Animator>();

        StartCoroutine(Flag());
    }


    IEnumerator Flag()
    {
        while(HP> 0)
        {
            yield return StartCoroutine(currentFlag.ToString());
        }
    }

    IEnumerator Idle()
    {
        animator.SetBool("isMove", false);

        yield break;
    }

    IEnumerator Move()
    {
        if (target == null) yield break;

        animator.SetBool("isMove", true);
        Vector3 moveVelocity = Vector3.zero;
        Vector3 targetPos = target.position;

        if (eraAttack.isAttack)
        {
            this.currentFlag = movementFlag.Attack;
            yield return null;
        }

        if (targetPos.x < this.transform.position.x)
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (targetPos.y < this.transform.position.y-1f) moveVelocity += Vector3.down;
        else moveVelocity += Vector3.up;

        this.transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    IEnumerator Attack()
    {
        if (!eraAttack.isAttack)
        {
            this.currentFlag = movementFlag.Move;
            yield return null;
        }

        Debug.Log("attack");

        yield return null;
    }


    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            this.target = collision.transform;
            this.currentFlag = movementFlag.Move;
            StopCoroutine("Idle");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            this.target = null;
            this.currentFlag = movementFlag.Idle;
        }
    }
}