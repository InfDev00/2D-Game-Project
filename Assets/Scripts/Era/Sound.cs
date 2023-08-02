using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAttack : MonoBehaviour
{
    private Transform[] sounds;
    public float soundDelay = 0.2f;
    private int damage;

    void Awake()
    {
        this.sounds = GetComponentsInChildren<Transform>();

        for (int i = 1; i < sounds.Length; ++i) sounds[i].gameObject.SetActive(false);

        StartCoroutine(Sound());
    }

    IEnumerator Sound()
    {
        for (int i = 1; i < sounds.Length; ++i)
        {
            sounds[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(soundDelay);
        }

        yield return new WaitForSeconds(soundDelay);

        for (int i = 1; i < sounds.Length; ++i)
        {

            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterManager>().Damaged(this.damage);
        }
    }

    public void SetDamage(int damage) { this.damage = damage; }
}