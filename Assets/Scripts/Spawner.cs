using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnPrefab;
    private GameObject spawnedObject;

    void Start()
    {
        Spawn();
    }

    void OnBecameVisible()
    {
        if (spawnedObject == null)
        {
            Spawn();
        }
    }
    void Spawn()
    {
        spawnedObject = Instantiate(spawnPrefab, this.transform.position, this.transform.rotation);
    }
}
