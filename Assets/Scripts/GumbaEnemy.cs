using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GumbaEnemy : Character, IInteractable
{
    enum GroundEnemy { Default, Dash, Turret }
    private Vector3 direction = Vector3.left;
    private int xScale = 1;
    [SerializeField] private int range;
    [SerializeField] private int dashSpd;
    //[SerializeField] private int runSpd;
    [SerializeField] GroundEnemy enemyType;
    Vector3 hitDir;
    Vector3 dashDir;
    
    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(Move());
        //Debug.Log(playerManager.name);
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

            //여기서 checkplayer 하고 적 종류에 따라 다른 공격패턴 구현?
            if (DistanceToPlayer()<range)
            {
                //플레이어 인지했을 시 애니메이션 호출하는 등의 함수 필요할듯
                SeePlayer();
                yield return new WaitForSeconds(1.5f);
                switch (enemyType)
                {
                    case GroundEnemy.Default:
                        //??
                        break;
                    case GroundEnemy.Dash:
                        yield return Dash();
                        break;
                    case GroundEnemy.Turret:
                        yield return Turret();
                        break;
                    default:
                        break;
                }

            }
        }
    }

    IEnumerator Dash()
    {
        //yield return new WaitForSeconds(2f);
        Debug.Log("Do Dash");
        rb.AddForce(dashDir.normalized*dashSpd, ForceMode2D.Impulse);
        yield return new WaitForSeconds(2f);
        //yield return Move();
    }

    IEnumerator Turret()
    {
        //Dosomething
        //yield return new WaitForSeconds(2f);
        Debug.Log("Do Turret Attack");
        //turretsth launch missile?
        yield return new WaitForSeconds(1f);
        //turretsth launch missile?
        yield return new WaitForSeconds(1f);
        //turretsth launch missile?
        yield return new WaitForSeconds(1f);
        //yield return Move();
    }
    
    
    void SeePlayer()
    {
        dashDir = playerManager.transform.position - this.transform.position;
        //플레이어를 바라봐야 하는지 그냥 냅둬도 되는지 조건문으로 확인
        if (dashDir.x * direction.x < 0)
        {
            TurnAround();
        }
        rb.AddForce(Vector3.up * 3, ForceMode2D.Impulse);
        Debug.Log("See Player");
    }

    void TurnAround()
    {
        direction = -direction;
        xScale = -xScale;

        this.transform.localScale = new Vector3(xScale, 1, 1);
    }

    void TurnUpsideDown()
    {
        Vector3 currentScale = transform.localScale;
        currentScale = new Vector3(1, -0.1f, 1);
        transform.localScale = currentScale;
    }

    void LaunchMissile()
    {

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
    public void Interact()
    {
        Debug.Log("stepon");
        StopAllCoroutines();
        TurnUpsideDown();
        GetComponent<BoxCollider2D>().enabled = false;
        rb.AddForce(Vector3.up * 2, ForceMode2D.Impulse);
        rb.AddTorque(1, ForceMode2D.Impulse);
        StartCoroutine(DieCo());
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

    protected override IEnumerator Idle()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator Chase()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator Attack()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator Killed()
    {
        throw new System.NotImplementedException();
    }
}
