using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

	public float movePower = 1f;
	public float jumpPower = 1f;
	public float doubleJumpPower = 1f;

	private Rigidbody2D rigid;
	private Animator animator;

	private bool isJumping = false;
    private bool isGround = true;
    private bool isDoubleJumping = false;


	void Start ()
	{
		rigid = gameObject.GetComponent<Rigidbody2D> ();
		animator = gameObject.GetComponent<Animator> ();
	}

	void Update ()
	{
		if (Input.GetButtonDown ("Jump")) {
			if(isGround)
			{
                isJumping = true;
                animator.SetTrigger("jump");
            }
			else
			{
				isDoubleJumping= true;
                animator.SetTrigger("jump");
            }
		}
	}

	void FixedUpdate ()
	{
		Move ();
		Jump ();
		DoubleJumping ();
	}

	void Move ()
	{		
		Vector3 moveVelocity= Vector3.zero;
		float horizontal = Input.GetAxisRaw("Horizontal");

		if(horizontal == 0)
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

	void Jump ()
	{
		if (isJumping && isGround)
		{
            rigid.velocity = Vector2.zero;
			isGround = false;
            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

            isJumping = false;
        }
	}

	void DoubleJumping()
	{
		if(isDoubleJumping && !isJumping)
		{
            rigid.velocity = Vector2.zero;
            isJumping = true;

			//[editable start]
			Vector3 teleport = new Vector3(doubleJumpPower, 0, 0);
			if (transform.localScale.x < 0) this.transform.position -= teleport;
            else this.transform.position += teleport;
            //Vector2 jumpVelocity = new Vector2(doubleJumpPower, doubleJumpPower / 2);
            //if (transform.localScale.x < 0) jumpVelocity = new Vector2(doubleJumpPower * (-1), doubleJumpPower / 2);
            //rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

			//[editable end]

            isDoubleJumping = false;
        }
	}


	void OnTriggerEnter2D (Collider2D collision)
	{
        rigid.velocity = Vector2.zero;
        isJumping = false;
		isDoubleJumping = false;
		isGround = true;
    }
}