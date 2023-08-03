using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    private GameObject monster;
    private GameManager gameManager;

    void Start()
    {
        gameManager = this.transform.parent.gameObject.GetComponent<GameManager>();
        Spawn();
    }

    void OnBecameVisible()
    {
        if (monster == null)
        {
            Spawn();
        }
    }
    void Spawn()
    {
        monster = Instantiate(monsterPrefab, this.transform.position, this.transform.rotation);
    }
}
