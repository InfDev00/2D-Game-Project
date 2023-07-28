using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Enemy : MonoBehaviour
{
    int hp;
    int atk;
    int speed;

    public void DealDamage()
    {

    }

    public abstract void DamageByBulet();
}
