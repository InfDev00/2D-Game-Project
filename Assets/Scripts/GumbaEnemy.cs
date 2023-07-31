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

    //coroutine ���ػ��
    //�Ѿ˸¾��� �� ȿ��
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
    
    //�׾��� �� ȿ��
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
            transform.position += /*���⿡ ������� �ٲٴ� bool ���� �߰��ؼ� �� ���� �����ϸ� �ڵ����� �ݴ�������� ���� ����*/
                direction * speed * Time.deltaTime;

            yield return null;

            if (CheckGround() == false)
            {
                animator.SetBool("isWalk", false);
                yield return new WaitForSeconds(3f);
                TurnAround();
                animator.SetBool("isWalk", true);
            }

            //���⼭ checkplayer �ϰ� �� ������ ���� �ٸ� �������� ����?
            if (DistanceToPlayer()<range)
            {
                //�÷��̾� �������� �� �ִϸ��̼� ȣ���ϴ� ���� �Լ� �ʿ��ҵ�
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
        //�÷��̾ �ٶ���� �ϴ��� �׳� ���ֵ� �Ǵ��� ���ǹ����� Ȯ��
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
        //���� �������� bullet�� �¾��� �� �ٸ� ȿ���� �Ͼ��
    }

    public override void DamageToWeakPoint()
    {
        //���⿡ �ִϸ��̼��̳� ȿ�� �߰�
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
