using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBot : CharacterManager, IInteractable, IDetectable
{

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        state = State.Idle;
        StartCoroutine(StateMachine());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Damaged(int damage)
    {
        throw new System.NotImplementedException();
    }

    public void Detect(Transform target)
    {
        throw new System.NotImplementedException();
    }

    public void Interact(Transform target)
    {
        
    }

    protected override IEnumerator Attack()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator Chase()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator Idle()
    {
        yield return null;
    }

    protected override IEnumerator Killed()
    {
        throw new System.NotImplementedException();
    }

    
}
