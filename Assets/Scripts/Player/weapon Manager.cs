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
            switch (collision.transform.localScale.x)
            {
                case 1 :
                    this.transform.localRotation = Quaternion.Euler(0, 0, 20);
                    this.transform.position = collision.transform.position + new Vector3(0.3f, -0.2f, 0);
                    break;
                case -1 :
                    this.transform.localRotation = Quaternion.Euler(0, 0, 200);
                    this.transform.position = collision.transform.position + new Vector3(-0.3f, -0.2f, 0);
                    break;
            }

            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<PlayerManager>().SetWeapon("sword");
        }
    }
}
