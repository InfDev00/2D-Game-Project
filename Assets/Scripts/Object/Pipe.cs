using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour, IInteractable
{
    bool isUsable = false;
    [SerializeField] string sceneName;
    public void Interact(Transform target)
    {
        if (target == null)
        {
            isUsable = false;
        }
        else { isUsable = true; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsable)
        {
            //???
        }
    }
}
