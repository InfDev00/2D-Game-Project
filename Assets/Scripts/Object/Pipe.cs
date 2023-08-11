using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour, IInteractable
{
    bool isUsable = false;
    [SerializeField] string sceneName;
    GameObject usingPlayer;
    public void Interact(Transform target)
    {
        if (target == null)
        {
            isUsable = false;
        }
        else 
        {
            usingPlayer = target.gameObject;
            isUsable = true; 
        }
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
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                StartCoroutine(PipeCo());
            }
        }
    }

    IEnumerator PipeCo()
    {
        Debug.Log("Pipe Activated");
        GameManager.Instance.soundControl.PlaySfx("pipe");
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        usingPlayer.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        while (true)
        {
            Debug.Log("pipe working");
            yield return null;
            
            Vector3 newPos = usingPlayer.transform.position - new Vector3(0, .01f,0);
            usingPlayer.transform.position = newPos;

            if (usingPlayer.transform.position.y < this.transform.position.y)
            {
                Debug.Log("nextScene");
                break;
            }
            
        }

        GameManager.Instance.SceneChange(sceneName);
    }
}
