using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GumbaEnemy : Enemy
{
    private Vector3 direction = Vector3.left;
    private int xScale = 1;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(Move());
        //Debug.Log(playerManager.name);
    }

    // Update is called once per frame
    void Update()
    {
    }

    //coroutine 적극사용
    //총알맞았을 때 효과
    IEnumerator HitCo()
    {
        StopCoroutine(Move());
        yield return null;
    }
    
    //죽었을 때 효과
    IEnumerator DieCo()
    {
        StopCoroutine(Move());
        yield return null;
    }

    IEnumerator Move()
    {
        animator.SetBool("isWalk", true);
        while (true)
        {
            transform.position += /*여기에 음수양수 바꾸는 bool 변수 추가해서 맵 끝에 도달하면 자동으로 반대방향으로 가게 하자*/
                direction * speed * Time.deltaTime;

            yield return null;

            if (CheckGround() == false)
            {
                animator.SetBool("isWalk", false);
                yield return new WaitForSeconds(3f);
                TurnAround();
                animator.SetBool("isWalk", true);
            }
        }
    }

    void TurnAround()
    {
        direction = -direction;
        xScale = -xScale;
        
        this.transform.localScale = new Vector3(xScale, 1, 1);
    }

    public override void DamageByBulet()
    {
        //dosomething
        //적의 종류마다 bullet에 맞았을 때 다른 효과가 일어나게
        
    }
}
