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

    private GameObject bulletPrefab;
    private GameObject killedPrefab;

    private enum Weapon { none, sword};
    private Weapon weapon;

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
        this.weapon = Weapon.none;
        ChangeState(State.Idle);

		bulletPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullets/Fire.prefab", typeof(GameObject));
        killedPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Killed.prefab", typeof(GameObject));

        StartCoroutine(this.StateMachine());
	}

    void Update()
    {
        timer += Time.deltaTime;

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y > 1f) pos.y = 1f;

        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    protected override IEnumerator Idle()
	{
        if (Input.GetButtonDown("Jump"))
        {
            isJump = true;
            ChangeState(State.Chase);
        }
        if (Input.GetAxisRaw("Horizontal") != 0) ChangeState(State.Chase);
        if (Input.GetButtonDown("Fire1")) ChangeState(State.Attack);
        animator.Play("Idle");
        yield return null;
	}

    protected override IEnumerator Chase()
	{
        Vector2 jumpVelocity = Vector2.zero;
        if (isJump || Input.GetButtonDown("Jump"))
        {
            switch (jump)
            {
                case Jump.ground:
                    animator.Play("Jump", -1, 0f);
                    GameManager.Instance.soundControl.PlaySfx("jump");
                    rb.velocity = Vector2.zero;
                    jumpVelocity = new Vector2(0, jumpPower);
                    rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
                    jump = Jump.jumpping;
                    isJump = false;
                    break;

                case Jump.jumpping:
                    animator.Play("Jump", -1, 0f);
                    GameManager.Instance.soundControl.PlaySfx("dash");
                    rb.velocity = Vector2.zero;
                    if (Input.GetAxisRaw("Vertical") == 1) jumpVelocity = new Vector2(0, doubleJumpPower * 2);
                    else if (transform.localScale.x < 0) jumpVelocity = new Vector2(doubleJumpPower * (-1), doubleJumpPower / 2);
                    else jumpVelocity = new Vector2(doubleJumpPower, doubleJumpPower);
                    rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
                    jump = Jump.doubleJumpping;
                    isJump = false;
                    break;

                case Jump.doubleJumpping:
                    break;
            }
            yield return null;
        }

        Vector3 moveVelocity = Vector3.zero;
        float horizontal = Input.GetAxisRaw("Horizontal");

        switch (horizontal)
        {
            case 0:
                ChangeState(State.Idle);
                yield return null;
                break;
            case 1:
                moveVelocity = Vector3.right;
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case -1:
                moveVelocity = Vector3.left;
                transform.localScale = new Vector3(-1, 1, 1);
                break;
        }

         animator.Play("Walk");
        transform.position += moveVelocity * speed * Time.deltaTime;
    }

    protected override IEnumerator Attack()
	{
        switch(this.weapon)
        {
            case Weapon.none:
                animator.Play("Attack");

                GameObject bullet = bulletPrefab;

                bulletDelay = bullet.GetComponent<BulletManager>().bulletDelay;
                if (timer > bulletDelay)
                {
                    Instantiate(bullet, this.transform.position + new Vector3(this.transform.localScale.x, 0, 0), this.transform.rotation);
                    timer = 0;
                }

                break;
            case Weapon.sword:
                var sword = this.transform.Find("Sword(Clone)");
                this.bulletDelay = 0.1f;
                if (timer > bulletDelay)
                {
                    sword.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    var curRotation = sword.transform.localRotation;
                    var curPosition = sword.transform.localPosition;
                    for (int i = 0; i < 3; ++i)
                    {
                        sword.localRotation = Quaternion.Euler(0, 0, -20);
                        yield return new WaitForSeconds(0.05f);
                        sword.localRotation = curRotation;
                        yield return new WaitForSeconds(0.05f);
                    }
                    sword.localPosition = new Vector3(0, 0.5f, 0);
                    sword.localRotation = Quaternion.Euler(0, 0, 120);
                    yield return new WaitForSeconds(0.05f);
                    sword.localRotation = curRotation;
                    sword.localPosition = curPosition;
                    sword.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    timer = 0;
                }
                break;
        }

        ChangeState(State.Idle);
        yield return new WaitForSeconds(0.1f);
    }

    protected override IEnumerator Killed()
	{
        this.AddAtk(this.GetAtk() * (-1));
        animator.Play("Killed");
        rb.bodyType = RigidbodyType2D.Static;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Instantiate(killedPrefab, this.transform.position + new Vector3(0, 3, 0), this.transform.rotation);
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
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
        rb.AddForce(new Vector2(this.transform.localScale.x * (-5), 10), ForceMode2D.Impulse);
        yield return new WaitUntil(() => jump == Jump.ground);
        isKnockBack = false;    
    }

    public void Interact(Transform target)
    {
        if (target == null) return;
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

    public void SetHp(int hp) { this.hp =  hp; }

    public void SetWeapon(string weapon) { this.weapon = (Weapon)Weapon.Parse(typeof(Weapon),weapon); }

    public int GetJumpPower() { return this.jumpPower; }
    public void AddJumpPower(int power) { this.jumpPower += power; }

    public static PlayerManager Instance
	{
		get { return instance; }
	}
}