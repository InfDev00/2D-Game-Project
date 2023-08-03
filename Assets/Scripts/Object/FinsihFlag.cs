using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishFlag : MonoBehaviour,IInteractable
{
    public void Detect(Transform target)
    {
       
    }

    public void Interact(Transform target)
    {
        Debug.Log("Finished. Go to next level");
    }
}
