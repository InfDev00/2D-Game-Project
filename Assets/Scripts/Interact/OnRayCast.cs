using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRayCast : MonoBehaviour
{
    [SerializeField] string tagName;

    public bool CheckWithRay(Vector2 dir, float distance)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, dir, distance);
        Debug.DrawRay(transform.position, dir, Color.blue);

        if (hit.collider != null && hit.collider.gameObject.CompareTag(tagName))
        {
            return true;
        }

        return false;
    }
}
