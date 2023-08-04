using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStay : MonoBehaviour
{
    public string targetTag;
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == this.targetTag)
        {
            this.transform.parent.gameObject.TryGetComponent(out IInteractable interactable);
            interactable.Stay(collision.transform);
            //this.gameObject.SetActive(false);
        }
    }

}
