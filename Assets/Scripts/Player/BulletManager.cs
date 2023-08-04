using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float bulletSpeed = 0.02f;
    public float bulletDelay = 0.1f;
    public int damage = 3;
    private Vector3 direction;

    void Awake()
    {
        if (PlayerManager.Instance.transform.localScale.x < 0)
        {
            direction = new Vector3(-1, 0, 0);
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        else direction = new Vector3(1, 0, 0);
    }


    void Update()
    {

        transform.Translate(direction * bulletSpeed);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<CharacterManager>().Damaged(damage);
            Destroy(this.gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
