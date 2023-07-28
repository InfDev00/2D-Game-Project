using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GumbaEnemy : Enemy,IInteractable
{
    private Vector3 direction = Vector3.left;
    private int xScale = 1;
    Vector3 hitDir;
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
        if (hp<=0)
        {
            StartCoroutine(DieCo());
            yield break;
        }
        rb.AddForce(hitDir,ForceMode2D.Impulse);
        Debug.Log("Hit by bullet");
        hp -= 1;
        yield return new WaitForSeconds(2f);
        StartCoroutine(Move());
    }
    
    //죽었을 때 효과
    IEnumerator DieCo()
    {
        Debug.Log(this.gameObject.name+" died");
        animator.SetBool("isDie", true);
        yield return new WaitForSeconds(1.3f);
        Destroy(gameObject);
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

    public override void DamageToWeakPoint()
    {
        //여기에 애니메이션이나 효과 추가
        Debug.Log($"{this.name} Died.");
    }

    public void StepOn()
    {
        StopAllCoroutines();
        //animator.SetBool("isSteppedOn", true);
        Vector3 currentScale = transform.localScale;
        currentScale = new Vector3(1,-0.1f,1);
        transform.localScale = currentScale;    
        GetComponent<BoxCollider2D>().enabled = false;
        rb.AddForce(Vector3.up*2,ForceMode2D.Impulse);
        rb.AddTorque(1,ForceMode2D.Impulse);
        StartCoroutine(DieCo());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hitDir = this.transform.position - collision.transform.position;

        if (collision.gameObject.tag=="Bullet")
        {
            StopAllCoroutines();
            StartCoroutine(HitCo());
        }
    }

   
}
