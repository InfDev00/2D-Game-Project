using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollider: MonoBehaviour
{
    public string targetTag;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag==this.targetTag)
        {
            this.transform.parent.gameObject.TryGetComponent(out IInteractable interactable);
            interactable.Interact(collision.transform);
            //this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == this.targetTag)
        {
            this.transform.parent.gameObject.TryGetComponent(out IInteractable interactable);
            interactable.Interact(null);
            //this.gameObject.SetActive(false);
        }
    }
}
