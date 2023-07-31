using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBlock : MonoBehaviour, IInteractable
{
    [SerializeField] int coinCount;
    [SerializeField] Animator animator;
    [SerializeField] Sprite spawnCoin;

    // Start is called before the first frame update
    void Start()
    {
        //일단 임시로 해놓음 좋지못한 방법인듯
        GetComponentsInChildren<SpriteRenderer>()[1].sprite = spawnCoin;
    }

    public void Interact()
    {
        if (coinCount > 0)
        {
            animator.SetTrigger("isHit");
            coinCount--;
            Debug.Log("BlockHit");
            //sStartCoroutine(SpawnCoin());
            //코인 isactive
        }
        
        
    }

    //IEnumerator SpawnCoin()
    //{
    //    spawnCoin.SetActive(true);
    //    yield return new WaitForEndOfFrame();
    //    spawnCoin.SetActive(false);
    //}

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
