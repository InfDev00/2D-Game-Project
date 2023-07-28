using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraAttack : MonoBehaviour
{
    public bool isAttack = false;

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            this.isAttack = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            this.isAttack = false;
        }
    }
}
