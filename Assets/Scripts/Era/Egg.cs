using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public float eggSpeed = 0.01f;
    private int damage = 0;

    void Update()
    {
        transform.Translate(Vector3.down * eggSpeed);
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterManager>().Damaged(this.damage);
            Destroy(this.gameObject);
        }
    }

    public void SetDamage(int damage) { this.damage = damage; }
}
