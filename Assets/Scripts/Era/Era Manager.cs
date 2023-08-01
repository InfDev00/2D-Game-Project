using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EraManager : CharacterManager, IInteractable
{ 
    public float attackRange = 1f;
    private float eggTimer;
    private float eggDelay = 1f;
    private Transform target;

    private GameObject EggPrefab;
    private GameObject SoundPrefab;

    private enum mode { normal = 0, angry };
    private mode currentMode = mode.normal;

    void Awake()
    {
        this.hp = maxHP;
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        EggPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Enemy/Era/Egg.prefab", typeof(GameObject));
        SoundPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Enemy/Era/Sound.prefab", typeof(GameObject));

        StartCoroutine(StateMachine());
    }

    void Update()
    {
        if (this.hp > 0)
        {
            eggTimer += Time.deltaTime;
            this.rb.velocity = Vector3.zero;
            this.rb.angularVelocity = 0f;
        }

        if(this.hp != this.maxHP)
        {
            currentMode = mode.angry;
            animator.Play("EraAngry");
            this.transform.Find("OnDetect").gameObject.GetComponent<CircleCollider2D>().radius = 10;
        }
    }

    protected override IEnumerator Idle()
    {
        switch (currentMode)
        {
            case mode.normal:
                animator.Play("EraIdle");
                break;
            case mode.angry:
                animator.Play("EraAngry");
                break;
        }

        yield break;
    }

    protected override IEnumerator Killed()
    {
        this.animator.Play("EraKilled");
        this.rb.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.rb.gravityScale = 2;

        Destroy(this.gameObject, 1f);
        yield break;
    }
    protected override IEnumerator Chase()
    {
        if (target == null)
        {
            ChangeState(State.Idle);
            yield break;
        }


        Vector3 moveVelocity = Vector3.zero;
        Vector3 targetPos = target.position;

        switch (currentMode)
        {
            case mode.normal:
                animator.Play("EraMove");
                targetPos += new Vector3(0, attackRange, 0);
                break;
            case mode.angry:
                targetPos += new Vector3(transform.localScale.x * attackRange*0.5f, 0, 0);
                break;
        }

        if (isAttack())
        {
            ChangeState(State.Attack);
            yield break;
        }

        if (targetPos.x < this.transform.position.x-0.5f)
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(targetPos.x < this.transform.position.x)
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (targetPos.y < this.transform.position.y) moveVelocity += Vector3.down;
        else moveVelocity += Vector3.up;

        this.transform.position += moveVelocity * speed * Time.deltaTime;
    }

    protected override IEnumerator Attack()
    {
        if (!isAttack())
        {
            ChangeState(State.Chase);
            yield break;
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
                animator.Play("EraAngryAttack");
                if (eggTimer > eggDelay)
                {
                    float dir = transform.localScale.x > 0 ? 1 : 0;
                    Instantiate(SoundPrefab, this.transform.position + new Vector3(this.transform.localScale.x * (-1f), 0, 0), Quaternion.Euler(0, 180f * dir, 0));
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
                if (transform.position.x - 0.5f < target.position.x && target.position.x < transform.position.x + 0.5f &&
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

    public void Detect(Transform target)
    {
        this.target = target;
        ChangeState(State.Chase);
    }

    public void Interact(Transform target)
    {
        Rigidbody2D targetRb = target.gameObject.GetComponent<Rigidbody2D>();

        targetRb.velocity = Vector2.zero;
        targetRb.AddForce(new Vector2(target.localScale.x * (-7), 7), ForceMode2D.Impulse);
        int damage = target.GetComponent<PlayerManager>().GetAtk();
        this.hp -= damage;
    }

    public override void DamageByBullet()
    {

    }
    public override void DamageToWeakPoint()
    {

    }
}