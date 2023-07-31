using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDetect : MonoBehaviour
{
    public string targetTag;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.tag == targetTag)
        {
            this.transform.parent.gameObject.TryGetComponent(out IInteractable detect);
            detect.Detect(collision.transform);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == targetTag)
        {
            this.transform.parent.gameObject.TryGetComponent(out IInteractable detect);
            detect.Detect(null);
        }
    }
}
