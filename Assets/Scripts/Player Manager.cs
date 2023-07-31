using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class PlayerManager : MonoBehaviour
{

	[Header("PlayerInfo")]
	public float movePower = 1f;
	public float jumpPower = 1f;
	public float doubleJumpPower = 1f;
	public int MaxHP = 10;
	private int HP = 10;
	private float timer = 0f;
	private float bulletDelay;

	private GameObject[] bulletPrefabs = new GameObject[3];
	private GameObject firePrefab;
	private GameObject waterPrefab;
	private GameObject thunderPrefab;

	private Rigidbody2D rigid;
	private Animator animator;
	private BoxCollider2D boxCollider;

	private GameObject playerImg;
	private GameObject transformImg;

	private bool isJump = false;
	private bool isDoubleJumpOrigin = false;
	private bool isAttackOrigin = false;
	private bool isOriginToTransform = false;

	private bool isAttackTransform = false;
	private bool isTransformToOrigin = false;

	private enum mode { origin = 0, transform};
	private mode currentMode;
	private enum bulletID { fire = 0, water };
	private bulletID currentbullet;

	private static PlayerManager instance;

	void Awake()
	{
		if (instance == null) instance = this;

		this.HP = MaxHP;
		rigid = gameObject.GetComponent<Rigidbody2D>();
		playerImg = transform.Find("PlayerImg").gameObject;
		transformImg = transform.Find("TransformImg").gameObject;
		animator = playerImg.GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider2D>();
		currentMode = mode.origin;
		currentbullet = bulletID.fire;

		bulletPrefabs[0] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullets/Fire.prefab", typeof(GameObject));
        bulletPrefabs[1] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullets/Water.prefab", typeof(GameObject));
        bulletPrefabs[2] = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullets/Thunder.prefab", typeof(GameObject));


		transformImg.SetActive(false);
	}

	void Update()
	{
		if (Input.GetButtonDown("Jump"))
		{
			switch(currentMode)
			{
				case mode.origin:
					if (!animator.GetBool("isJumping"))
					{
						isJump = true;
						animator.SetTrigger("doJumping");
						animator.SetBool("isJumping", true);
					}

					else if (!animator.GetBool("isTeleport"))
					{
						isDoubleJumpOrigin = true;
						animator.SetTrigger("doTeleport");
						animator.SetBool("isTeleport", true);
					}
					break;
				case mode.transform:
					isJump = true;
					animator.SetBool("isFloating", true);
					break;
            }
		}

		if (Input.GetButton("Fire1"))
		{
            switch (currentMode)
            {
                case mode.origin:
                    isAttackOrigin = true;
                    animator.SetTrigger("attack");
                    break;
                case mode.transform:
					isAttackTransform = true;
                    break;
            }
        }
		else if (Input.GetButtonDown("Fire2"))
		{
            switch (currentMode)
            {
                case mode.origin:
                    isOriginToTransform = true;
                    break;
                case mode.transform:
					isTransformToOrigin = true;
                    break;
            }
        }
		else if (Input.GetButtonDown("Fire3"))
		{
			switch (currentMode)
			{
				case mode.origin:
					if(currentbullet == bulletID.fire)currentbullet = bulletID.water;
					else currentbullet = bulletID.fire;
				break;
			}
		}

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y < 0f) pos.y = 0f;
        if (pos.y > 1f) pos.y = 1f;

        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

	void FixedUpdate()
	{
		Move();
        Jump();
        Transform();
        switch (currentMode)
		{
			case mode.origin:
                DoubleJumpOrigin();
                AttackOrigin();
				break;
			case mode.transform:
				AttackTransform();
                break;
        }
	}

	void Move()
	{
		if (!isAttackOrigin ||animator.GetBool("isJumping"))
		{
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


			transform.position += moveVelocity * movePower * Time.deltaTime;
		}
	}

	void Jump()
	{
		if (isJump)
		{
			rigid.velocity = Vector2.zero;
			Vector2 jumpVelocity = new Vector2(0, jumpPower);
			rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

			isJump = false;
		}
	}

	void DoubleJumpOrigin()
	{
		if (isDoubleJumpOrigin)
		{
			rigid.velocity = Vector2.zero;

			//[editable start]
			Vector3 teleport = new Vector3(doubleJumpPower, 0, 0);
			if (transform.localScale.x < 0) this.transform.position -= teleport;
			else this.transform.position += teleport;
			//Vector2 jumpVelocity = new Vector2(doubleJumpOriginPower, doubleJumpOriginPower / 2);
			//if (transform.localScale.x < 0) jumpVelocity = new Vector2(doubleJumpOriginPower * (-1), doubleJumpOriginPower / 2);
			//rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

			//[editable end]

			isDoubleJumpOrigin = false;
		}
	}

	void AttackOrigin()
	{
		GameObject bullet = bulletPrefabs[(int)currentbullet];
		bulletDelay = bullet.GetComponent<BulletManager>().bulletDelay;
		timer += Time.deltaTime;
		if (isAttackOrigin)
		{
			if (timer > bulletDelay)
			{
				Instantiate(bullet, this.transform.position + new Vector3(this.transform.localScale.x*0.7f, 0, 0), this.transform.rotation);
				timer = 0;
			}
			isAttackOrigin = false;
		}
	}

    void AttackTransform()
    {
        bulletDelay = bulletPrefabs[2].GetComponent<BulletManager>().bulletDelay;
        timer += Time.deltaTime;
        if (isAttackTransform)
        {
            if (timer > bulletDelay)
            {
				Instantiate(bulletPrefabs[2], this.transform.position + new Vector3(this.transform.localScale.x, 0, 0), this.transform.rotation);
                timer = 0;
            }
            isAttackTransform = false;
        }
    }

    void Transform()
	{
		if (isOriginToTransform)
		{
			transformImg.SetActive(true);
			playerImg.SetActive(false);
			isOriginToTransform = false;
			animator = transformImg.GetComponent<Animator>();
			rigid.gravityScale = 0.25f;
			boxCollider.size = new Vector2(0.7f, 0.7f);
			this.jumpPower /= 6;
			this.movePower /= 2;
			currentMode = mode.transform;
        }

		else if (isTransformToOrigin)
		{
            transformImg.SetActive(false);
            playerImg.SetActive(true);
            isTransformToOrigin = false;
            animator = playerImg.GetComponent<Animator>();
            rigid.gravityScale = 3.5f;
            boxCollider.size = new Vector2(0.7f, 1f);
            this.jumpPower *= 6;
            this.movePower *= 2;
            currentMode = mode.origin;

			transformImg.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		if(collision.tag == "Ground")
		{
            switch (currentMode)
            {
                case mode.origin:
                    animator.SetBool("isJumping", false);
                    animator.SetBool("isTeleport", false);
                    break;
                case mode.transform:
                    animator.SetBool("isFloating", false);
                    break;
            }
        }
	}

	public void Damaged(int damage)
	{
		this.HP -= damage;

        switch (currentMode)
        {
            case mode.origin:
                rigid.AddForce(new Vector2(this.transform.localScale.x * (-2f), 4f), ForceMode2D.Impulse);
                break;
            case mode.transform:
                break;
        }

    }
    public static PlayerManager Instance
	{
		get { return instance; }
	}
}