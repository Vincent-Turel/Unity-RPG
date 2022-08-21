using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Enemy") && player.GetComponent<PlayerContol>().isAttacking())
        {
            player.GetComponent<PlayerContol>().DealMeleeDamage(other.gameObject);
            other.gameObject.GetComponent<EnemyScript>().Hit();
           
        }
    }
}
