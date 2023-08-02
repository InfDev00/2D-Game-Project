using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{

    public void Interact(Transform target);

    public void Detect(Transform target);

    public void Stay(Transform target);
}
