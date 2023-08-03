using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDetect : MonoBehaviour
{
    public string targetTag;
    [SerializeField] bool enterOnly;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == targetTag)
        {
            this.transform.parent.gameObject.TryGetComponent(out IInteractable detect);
            detect.Detect(collision.transform);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (enterOnly) return;
        if (collision.tag == targetTag)
        {
            this.transform.parent.gameObject.TryGetComponent(out IInteractable detect);
            detect.Detect(null);
        }
    }
}
