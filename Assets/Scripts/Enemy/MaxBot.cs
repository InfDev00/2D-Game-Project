using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxBot : CharacterManager, IInteractable, IDetectable
{
    enum GroundEnemy { Default, Dash, Turret }
    private Vector3 direction = Vector3.left;
    private int xScale = 1;
    [SerializeField] private int range;
    [SerializeField] private int dashSpd;
    [SerializeField] GroundEnemy enemyType;
    [SerializeField] Transform rayPoint;
    Vector3 hitDir;
    Vector3 dashDir;
    
    // Start is called before the first frame update
    void Start()
    {
        //playerManager = PlayerManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        state = State.Idle;
        StartCoroutine(StateMachine());
    }

    protected override IEnumerator Idle()
    {
        animator.SetBool("isWalk", true);

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

    protected override IEnumerator Chase()
    {
        Debug.Log("Chase");
        SeePlayer();
        yield return null;
        ChangeState(State.Attack);
    }

    protected override IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);

        switch (enemyType)
        {
            case GroundEnemy.Default:
                ChangeState(State.Idle);
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

    protected override IEnumerator Killed()
    {
        Debug.Log(this.gameObject.name + " died");
        animator.SetBool("isDie", true);
        yield return new WaitForSeconds(1.3f);
        Destroy(gameObject);
    }

    //coroutine 적극사용
    //총알맞았을 때 효과
    IEnumerator HitCo()
    {
        rb.AddForce(hitDir, ForceMode2D.Impulse);
        Debug.Log("Hit by bullet");
        hp -= 1;
        yield return new WaitForSeconds(1f);
        //StartCoroutine(Move());
    }

    //죽었을 때 효과
    IEnumerator DieCo()
    {
        Debug.Log(this.gameObject.name+" died");
        animator.SetBool("isDie", true);
        yield return new WaitForSeconds(1.3f);
        Destroy(gameObject);
    }

    

    IEnumerator Dash()
    {
        //yield return new WaitForSeconds(2f);
        Debug.Log("Do Dash");
        rb.AddForce(dashDir.normalized*dashSpd, ForceMode2D.Impulse);
        yield return new WaitForSeconds(2f);
        ChangeState(State.Idle);
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
        ChangeState (State.Idle);
    }


    void SeePlayer()
    {
        //dashDir = playerManager.transform.position - this.transform.position;
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

        if 
   

    (collision.gameObject.tag=="Bullet")
        {
            StartCoroutine(HitCo());
        }
    }
    public void Interact(Transform target)
    {
        Debug.Log("stepon");
        StopAllCoroutines();
        TurnUpsideDown();
        GetComponent<BoxCollider2D>().enabled = false;
        rb.AddForce(Vector3.up * 2, ForceMode2D.Impulse);
        rb.AddTorque(1, ForceMode2D.Impulse);
        StartCoroutine(DieCo());
    }

    public void Detect(Transform target)
    {
        if (target == null) return;
        Debug.Log("Detect");
        state = State.Chase;
    }


    public override void Damaged(int damage)
    {

    }


    bool CheckGround()
    {

        Debug.DrawRay(rayPoint.transform.position, Vector3.down, Color.blue);
        if (Physics2D.Raycast(rayPoint.transform.position, Vector3.down, 5))
        {
            return true;
        }
        else return false;

    }

    public void Stay(Transform target)
    {
        throw new System.NotImplementedException();
    }

}
