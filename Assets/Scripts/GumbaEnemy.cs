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

    //coroutine ���ػ��
    //�Ѿ˸¾��� �� ȿ��
    IEnumerator HitCo()
    {
        StopCoroutine(Move());
        yield return null;
    }
    
    //�׾��� �� ȿ��
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
}
