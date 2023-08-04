using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDetect : MonoBehaviour
{
    public string targetTag;

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == targetTag)
        {
            this.transform.parent.gameObject.TryGetComponent(out IDetectable detect);
            detect.Detect(null);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == targetTag)
        {
            this.transform.parent.gameObject.TryGetComponent(out IDetectable detect);
            detect.Detect(collision.transform);
        }

        if(collision.tag == "Stealth" && targetTag == "Player")
        {
            this.transform.parent.gameObject.TryGetComponent(out IDetectable detect);
            detect.Detect(null);
        }
    }
}
