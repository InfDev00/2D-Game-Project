using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public float eggSpeed = 0.01f;
    public int damage;

    void Update()
    {
        transform.Translate(Vector3.down * eggSpeed);
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.Damaged(damage);
            Destroy(this.gameObject);
        }
    }
}
