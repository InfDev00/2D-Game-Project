using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class PlayerManager : CharacterManager, IInteractable
{
    [SerializeField] protected int jumpPower;
    [SerializeField] protected int doubleJumpPower;
	private float timer = 0f;
	private float bulletDelay;

	private GameObject[] bulletPrefabs = new GameObject[3];

    private enum Jump { ground, jumpping, doubleJumpping };
    private Jump jump;
    private bool isJump = false;
    private bool isKnockBack = false;

    private static PlayerManager instance;

	void Awake()
	{
		if (instance == null) instance = this;

		this.hp = maxHP;
		rb = gameObject.GetComponent<Rigidbody2D>();
		animator = this.GetComponent<Animator>();
        this.jump = Jump.ground;
        ChangeState(State.Idle);

		bulletPrefabs[0] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullets/Fire.prefab", typeof(GameObject));
        bulletPrefabs[1] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullets/Water.prefab", typeof(GameObject));
        bulletPrefabs[2] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullets/Thunder.prefab", typeof(GameObject));

		StartCoroutine(this.StateMachine());
	}

    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetButtonDown("Jump")) isJump = true;

        if (Input.GetButtonDown("Fire1")) ChangeState(State.Attack);

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y > 1f) pos.y = 1f;

        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void FixedUpdate()
    {
        Move();
    }

    protected override IEnumerator Idle()
	{
        if (Input.GetButtonDown("Jump") || Input.GetButton("Horizontal"))
        {
            yield break;
        }

        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) animator.Play("Idle");
        yield break;
	}

    protected override IEnumerator Chase()
	{
		yield return null;
    }

    protected override IEnumerator Attack()
	{
        var curAnimStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        animator.Play("Attack");
        GameObject bullet = bulletPrefabs[0];

        bulletDelay = bullet.GetComponent<BulletManager>().bulletDelay;
        timer += Time.deltaTime;
        if (timer > bulletDelay)
        {
            Instantiate(bullet, this.transform.position + new Vector3(this.transform.localScale.x * 0.7f, 0, 0), this.transform.rotation);
            timer = 0;
        }

        ChangeState(State.Idle);
        yield return new WaitForSeconds(curAnimStateInfo.length *2f);
    }

	void Move()
    {
        if (isKnockBack) return;

        Vector2 jumpVelocity = Vector2.zero;
        if (isJump)
        {
            switch (jump)
            {
                case Jump.ground:
                    animator.Play("Jump", -1, 0f);
                    rb.velocity = Vector2.zero;
                    jumpVelocity = new Vector2(0, jumpPower);
                    rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
                    jump = Jump.jumpping;
                    isJump = false;
                    break;

                case Jump.jumpping:
                    animator.Play("Jump",-1, 0f);
                    rb.velocity = Vector2.zero;
                    if(Input.GetAxisRaw("Vertical")==1) jumpVelocity = new Vector2(0, doubleJumpPower*2);
                    else if (transform.localScale.x < 0) jumpVelocity = new Vector2(doubleJumpPower * (-1), doubleJumpPower / 2);
                    else jumpVelocity = new Vector2(doubleJumpPower, doubleJumpPower);
                    rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
                    jump = Jump.doubleJumpping;
                    isJump = false;
                    break;

                case Jump.doubleJumpping:
                    break;
            }
        }

        Vector3 moveVelocity = Vector3.zero;
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal == 0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            ChangeState(State.Idle);
            return;
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                moveVelocity = Vector3.left;
                transform.localScale = new Vector3(-1, 1, 1);
            }

            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                moveVelocity = Vector3.right;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) animator.Play("Walk");
        transform.position += moveVelocity * speed * Time.deltaTime;
    }

    protected override IEnumerator Killed()
	{
        this.AddAtk(this.GetAtk());
        animator.Play("Killed");
        this.rb.AddForce(new Vector2(0, 3f), ForceMode2D.Impulse);
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.rb.gravityScale = 2;

        Destroy(this.gameObject, 5f);
        yield return null;
	}

    public override void Damaged(int damage)
    {
        this.hp -= damage;
        rb.AddForce(new Vector2(this.transform.localScale.x * (-2f), 4f), ForceMode2D.Impulse);
    }

    public IEnumerator Knockback()
    {
        isKnockBack = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(this.transform.localScale.x * (-3), 10), ForceMode2D.Impulse);
        yield return new WaitUntil(() => jump == Jump.ground);
        isKnockBack = false;    
    }

    public void Interact(Transform target)
    {
        switch(target.tag)
        {
            case "Ground":
                rb.velocity = Vector2.zero;
                jump = Jump.ground;
                isJump = false;
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) animator.Play("Walk");
                break;
            case "Enemy":
                StartCoroutine(Knockback());
                target.gameObject.GetComponent<CharacterManager>().Damaged(this.atk);
                break;
        }
    }

    public void Detect(Transform target)
    {

    }

    public void Stay(Transform target)
    {

    }

	public int GetHP() { return this.hp; }

    public static PlayerManager Instance
	{
		get { return instance; }
	}
}