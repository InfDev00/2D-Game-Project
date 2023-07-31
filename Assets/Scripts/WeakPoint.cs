using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            this.transform.parent.gameObject.TryGetComponent(out IInteractable interactable);
            interactable.Interact();
            //this.gameObject.SetActive(false);
        }
    }

}
