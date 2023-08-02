using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public float eggSpeed = 0.01f;

    void Update()
    {
        transform.Translate(Vector3.down * eggSpeed);
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
