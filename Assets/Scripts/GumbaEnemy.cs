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
        //���� �������� bullet�� �¾��� �� �ٸ� ȿ���� �Ͼ��
    }

    public override void DamageToWeakPoint()
    {
        //���⿡ �ִϸ��̼��̳� ȿ�� �߰�
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
