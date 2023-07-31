using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDetect : MonoBehaviour
{
    private Transform target;
    private bool isMove = false;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            this.target = collision.transform;
            this.isMove = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            this.target = null;
            this.isMove = false;
        }
    }

    public Transform GetTarget() { return target; }
    public bool GetIsMove() { return isMove; }
}
