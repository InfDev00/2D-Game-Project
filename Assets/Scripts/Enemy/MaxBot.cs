using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxBot : CharacterManager, IInteractable, IDetectable
{
    enum GroundEnemy { Default, Dash}
    private Vector3 direction = Vector3.left;
    private int xScale = 1;
    private Transform target;
    [SerializeField] private int dashSpd;
    [SerializeField] GroundEnemy enemyType;
    [SerializeField] OnRayCast rayCast;
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

        //땅체크 or 앞이 땅으로 막혔을 때
        if (!rayCast.CheckWithRay(Vector2.down,5) || rayCast.CheckWithRay(direction, .5f) )
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
            default:
                break;
        }
    }

    protected override IEnumerator Killed()
    {
        Debug.Log(this.gameObject.name + " died");
        animator.SetBool("isDie", true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
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
        if (target == null) return;

        dashDir = target.position - this.transform.position;
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
        this.target = target;

        if (this.target == null) return;

        Debug.Log("Detect");
        ChangeState(State.Chase);
    }


    public override void Damaged(int damage)
    {
        rb.AddForce(new Vector2(this.transform.localScale.x * (1f), 2f), ForceMode2D.Impulse);
        Debug.Log("Hit by bullet");
        hp -= damage;
        //StartCoroutine(Move());
    }


    

   

}
