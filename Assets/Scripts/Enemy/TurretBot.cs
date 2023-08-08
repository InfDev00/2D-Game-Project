using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretBot : CharacterManager, IInteractable, IDetectable
{
    Transform target;
    int xScale = 1;
    Vector3 direction = Vector3.left;
    Vector3 shootDir;
    bool isAngry = false;
    [SerializeField] OnRayCast rayCast;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] float shootSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        state = State.Idle;
        StartCoroutine(StateMachine());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Damaged(int damage)
    {
        isAngry = true;
        animator.SetBool("isAngry",true);
        hp -= damage;
    }

    public void Detect(Transform target)
    {
        this.target = target;

        if (this.target == null) return;

        Debug.Log("Detect");
        ChangeState(State.Chase);
    }

    public void Interact(Transform target)
    {
        Debug.Log("stepon");
        animator.SetBool("isAngry", true);
        isAngry = true;
    }

    protected override IEnumerator Attack()
    {
        float attackRate = isAngry ? .5f : 1.5f;
        yield return new WaitForSeconds(attackRate);
        animator.SetTrigger("isAttack");
        ShootBomb();
        
        //Shoot()

        ChangeState(State.Idle);
    }

    void ShootBomb()
    {
        if (target == null) return;
        shootDir = target.position - this.transform.position;
        rb.AddTorque(.2f, ForceMode2D.Impulse);
        GameObject bomb = Instantiate(bombPrefab, rayCast.transform);
        bomb.GetComponent<Rigidbody2D>().AddForce(shootDir.normalized * shootSpeed, ForceMode2D.Impulse);

    }
    protected override IEnumerator Chase()
    {
        //if (isAngry) yield break;

        Debug.Log("Chase");
        SeePlayer();
        yield return null;
        ChangeState(State.Attack);
    }

    protected override IEnumerator Idle()
    {
        transform.position += /*���⿡ ������� �ٲٴ� bool ���� �߰��ؼ� �� ���� �����ϸ� �ڵ����� �ݴ�������� ���� ����*/
            direction * speed * Time.deltaTime;

        yield return null;

        //��üũ or ���� ������ ������ ��
        if (!rayCast.CheckWithRay(Vector2.down, 5) || rayCast.CheckWithRay(direction, .5f))
        {
            yield return new WaitForSeconds(3f);
            TurnAround();
        }
       
    }

    protected override IEnumerator Killed()
    {
        throw new System.NotImplementedException();
    }

    void SeePlayer()
    {
        if (target == null) return;

        shootDir = target.position - this.transform.position;
        //�÷��̾ �ٶ���� �ϴ��� �׳� ���ֵ� �Ǵ��� ���ǹ����� Ȯ��
        if (shootDir.x * direction.x < 0)
        {
            TurnAround();
        }
        rb.AddForce(Vector3.up*2, ForceMode2D.Impulse);
        
        Debug.Log("See Player");
    }

    void TurnAround()
    {
        direction = -direction;
        xScale = -xScale;

        this.transform.localScale = new Vector3(xScale, 1, 1);
    }

}
