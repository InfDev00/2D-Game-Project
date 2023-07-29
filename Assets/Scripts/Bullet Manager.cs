using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float bulletSpeed = 0.02f;
    public float bulletDelay = 0.1f;
    public int bulletDamage = 3;
    private Vector3 direction;
    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerManager.Instance.transform.localScale.x < 0)
        {
            direction = new Vector3(-1, 0, 0);
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        else direction = new Vector3(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(direction * bulletSpeed);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("aa");
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }

    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
