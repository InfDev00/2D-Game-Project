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
    private Jump jumpState;

    private static PlayerManager instance;

	void Awake()
	{
		if (instance == null) instance = this;

		this.hp = maxHP;
		rb = gameObject.GetComponent<Rigidbody2D>();
		animator = this.GetComponent<Animator>();
        this.jumpState = Jump.ground;
		this.state = State.Idle;

		bulletPrefabs[0] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullets/Fire.prefab", typeof(GameObject));
        bulletPrefabs[1] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullets/Water.prefab", typeof(GameObject));
        bulletPrefabs[2] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullets/Thunder.prefab", typeof(GameObject));

		StartCoroutine(this.StateMachine());
	}

    void Update()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y < 0f) pos.y = 0f;
        if (pos.y > 1f) pos.y = 1f;

        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }


    protected override IEnumerator Idle()
	{
        animator.Play("Idle");

        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Horizontal"))
        {
            this.state = State.Move;
            yield break;
        }

        else if (Input.GetButton("Fire1"))
        {
            this.state = State.Attack;
            yield break;
        }

        yield return null;
	}

    protected override IEnumerator Chase()
	{
		yield return null;
    }

    protected override IEnumerator Attack()
	{
		animator.Play("Attack");
        GameObject bullet = bulletPrefabs[0];

        bulletDelay = bullet.GetComponent<BulletManager>().bulletDelay;
        timer += Time.deltaTime;
        if (timer > bulletDelay)
        {
            Instantiate(bullet, this.transform.position + new Vector3(this.transform.localScale.x * 0.7f, 0, 0), this.transform.rotation);
            timer = 0;
        }

        this.state = State.Idle;
        yield return null;
    }

	protected override IEnumerator Move()
	{
        Vector2 jumpVelocity = Vector2.zero;
        if (Input.GetButtonDown("Jump"))
        {
            switch (jumpState)
            {
                case Jump.ground:
                    rb.velocity = Vector2.zero;
                    jumpVelocity = new Vector2(0, jumpPower);
                    rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
                    jumpState = Jump.jumpping;
                    yield return null;
                    break;

                case Jump.jumpping:
                    rb.velocity = Vector2.zero;
                    //Vector3 teleport = new Vector3(doubleJumpPower, 0, 0);
                    //if (transform.localScale.x < 0) this.transform.position -= teleport;
                    //else this.transform.position += teleport;
                    jumpVelocity = new Vector2(doubleJumpPower, doubleJumpPower);
                    if (transform.localScale.x < 0) jumpVelocity = new Vector2(doubleJumpPower * (-1), doubleJumpPower / 2);
                    rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
                    jumpState = Jump.doubleJumpping;
                    yield return null;
                    break;

                case Jump.doubleJumpping:
                    yield return null;
                    break;
            }
        }

        Vector3 moveVelocity = Vector3.zero;
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal == 0)
        {
            animator.SetBool("move", false);
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                moveVelocity = Vector3.left;
                transform.localScale = new Vector3(-1, 1, 1);
                animator.SetBool("move", true);
            }

            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                moveVelocity = Vector3.right;
                transform.localScale = new Vector3(1, 1, 1);
                animator.SetBool("move", true);
            }
        }

        transform.position += moveVelocity * speed * Time.deltaTime;
    }

    protected override IEnumerator Killed()
	{
        yield return null;
	}

	public override void DamageByBullet()
	{

	}
	public override void DamageToWeakPoint()
	{

	}

    public void Interact()
    {

    }

    public void Detect(Transform target)
    {
        Debug.Log("aa");

        if (target != null)
        {
            rb.velocity = Vector2.zero;
            jumpState = Jump.ground;
        }
    }

	public void Damaged(int damage)
	{
		this.hp -= damage;
        rb.AddForce(new Vector2(this.transform.localScale.x * (-2f), 4f), ForceMode2D.Impulse);
    }

	public int GetHP() { return this.hp; }

    public static PlayerManager Instance
	{
		get { return instance; }
	}
}