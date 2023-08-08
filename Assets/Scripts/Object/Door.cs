using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable, IDetectable
{
    private Animator animator;
    public string SceneName;

    void Start()
    {
        animator = this.GetComponent<Animator>();
    }
    public void Interact(Transform target)
    {
        Debug.Log("GameEnd");
        GameManager.Instance.SceneChange(SceneName);

    }

    public void Detect(Transform target)
    {
        if(target == null)
        {
            animator.Play("DoorClose");
        }
        else
        {
            animator.Play("DoorOpen");
        }
    }
}
