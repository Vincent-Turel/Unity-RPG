using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{

    public GameObject player;

    private bool isRegen = false;

    void Update()
    {
        Vector3 playerPos = player.transform.position;
        if (!isRegen && Mathf.Sqrt(Mathf.Pow(playerPos.x - transform.position.x, 2f) + Mathf.Pow(playerPos.z - transform.position.z, 2f)) < 35)
        {
            StartCoroutine(Regen());
        }
    }

    IEnumerator Regen()
    {
        isRegen = true;
        player.GetComponent<PlayerContol>().regenHealth();
        yield return new WaitForSeconds(.5f);
        isRegen = false;
    }
}
