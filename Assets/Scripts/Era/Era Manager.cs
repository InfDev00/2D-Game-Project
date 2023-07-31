using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EraManager : MonoBehaviour
{
    [Header("EraInfo")]
    public float attackRange = 1f;
    public float movePower = 1f;
    public int MaxHP = 10;
    private int HP = 10;
    private float eggTimer;
    private float eggDelay = 1f;

    private GameObject EggPrefab;
    private GameObject SoundPrefab;

    private Rigidbody2D rigid;
    private Animator animator;
    private Transform target;

    private EraImg eraImg;

    private enum movementFlag { Idle = 0, Move, Attack, Killed };
    private movementFlag currentFlag = movementFlag.Idle;
    
    private enum mode { normal = 0, angry};
    private mode currentMode = mode.normal;

    void Awake()
    {
        this.HP = MaxHP;
        eraImg = transform.Find("EraImg").gameObject.GetComponent<EraImg>();
        animator = transform.Find("EraImg").gameObject.GetComponent<Animator>();
        rigid = gameObject.GetComponent<Rigidbody2D>();

        EggPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Enemy/Era/Egg.prefab", typeof(GameObject));
        SoundPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Enemy/Era/Sound.prefab", typeof(GameObject));

        StartCoroutine(Flag());
    }

    void Update()
    {
        if(this.HP > 0)
        {
            this.rigid.velocity = Vector3.zero;
            this.rigid.angularVelocity = 0f;
        }
    }

    IEnumerator Flag()
    {
        while(HP> 0)
        {
            eggTimer += Time.deltaTime;

            yield return StartCoroutine(currentFlag.ToString());

            switch(currentMode)
            {
                case mode.normal:
                    if (this.HP != MaxHP)
                    {
                        this.currentMode = mode.angry;
                        this.movePower *= 2;
                        this.animator.SetTrigger("Angry");
                        this.transform.Find("EraImg").gameObject.GetComponent<CircleCollider2D>().radius *= 10;
                    }
                    break;
                case mode.angry:
                    break;
            }
        }

        StartCoroutine("Killed");
    }

    IEnumerator Idle()
    {
        if (eraImg.isMove)
        {
            this.currentFlag = movementFlag.Move;
            this.target = eraImg.target;
            yield break;
        }
        animator.SetBool("isMove", false);


        yield break;
    }

    IEnumerator Killed()
    {
        this.animator.SetTrigger("Die");
        this.rigid.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.rigid.gravityScale = 2;

        Destroy(this.gameObject, 1f);
        yield break;
    }
    IEnumerator Move()
    {
        if (target == null) yield break;
        if (!eraImg.isMove)
        {
            this.currentFlag = movementFlag.Idle;
            yield break;
        }

        animator.SetBool("isMove", true);
        Vector3 moveVelocity = Vector3.zero;
        Vector3 targetPos = target.position;

        switch (currentMode)
        {
            case mode.normal:
                targetPos += new Vector3(0, attackRange, 0);
                break;
            case mode.angry:
                targetPos += new Vector3(transform.localScale.x *attackRange, 0, 0);
                break;
        }

        if (this.isAttack())
        {
            this.currentFlag = movementFlag.Attack;
            yield break;
        }

        if (targetPos.x < this.transform.position.x)
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (targetPos.y < this.transform.position.y) moveVelocity += Vector3.down;
        else moveVelocity += Vector3.up;

        this.transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    IEnumerator Attack()
    {
        if (!isAttack())
        {
            this.currentFlag = movementFlag.Move;
            yield return null;
        }


        switch (currentMode)
        {
            case mode.normal:
                if (eggTimer > eggDelay)
                {
                    Instantiate(EggPrefab, this.transform.position + new Vector3(0, -0.5f, 0), this.transform.rotation);
                    eggTimer = 0;
                }
                break;
            case mode.angry:
                animator.SetTrigger("doAttack");
                if (eggTimer > eggDelay)
                {
                    float dir = transform.localScale.x > 0 ? 1 : 0;
                    Instantiate(SoundPrefab, this.transform.position + new Vector3(this.transform.localScale.x * (-1f), 0, 0), Quaternion.Euler(0,180f * dir, 0));
                    eggTimer = 0;
                }
                break;
        }

        yield return null;
    }

    bool isAttack()
    {
        switch (currentMode)
        {
            case mode.normal:
                if (transform.position.x - 0.5f < target.position.x && target.position.x < transform.position.x+0.5f && 
                    transform.position.y - attackRange < target.position.y && target.position.y < transform.position.y)
                {
                    return true;
                }
                break;
            case mode.angry:
                if (transform.position.x - attackRange < target.position.x && target.position.x < transform.position.x + attackRange &&
                    transform.position.y - 1f < target.position.y && target.position.y < transform.position.y + 1f) 
                {
                    return true;
                }
                break;
        }

        return false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            this.HP -= collision.gameObject.GetComponent<BulletManager>().bulletDamage;
        }
    }
}