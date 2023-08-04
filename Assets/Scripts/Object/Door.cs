using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene(SceneName);
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
