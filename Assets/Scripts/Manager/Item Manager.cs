using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ItemManager : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rigid;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            this.Action(collision.gameObject);
        }
    }

    protected abstract void Action(GameObject player);
}
