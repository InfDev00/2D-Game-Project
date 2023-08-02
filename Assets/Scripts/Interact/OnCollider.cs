using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollider: MonoBehaviour
{
    public string tag;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag==this.tag)
        {
            this.transform.parent.gameObject.TryGetComponent(out IInteractable interactable);
            interactable.Interact(collision.transform);
            //this.gameObject.SetActive(false);
        }
    }

}
