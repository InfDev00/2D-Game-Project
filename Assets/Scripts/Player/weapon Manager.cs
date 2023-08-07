using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int damage = 3;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<CharacterManager>().Damaged(damage);
        }

        if(collision.tag == "Player")
        {
            this.transform.parent = collision.transform;
            this.transform.rotation = Quaternion.Euler(0, 0, 20);
            this.transform.position = collision.transform.position + new Vector3(0.8f, 0, 0);

            collision.gameObject.GetComponent<PlayerManager>().SetWeapon("sword");
        }
    }
}
