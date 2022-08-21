using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public static WorldItem spawnWorldItem(Vector3 position, Item item)
    {
        GameObject gameObject = ObjectPool.SharedInstance.GetPooledLoot(item.itemType);
        gameObject.transform.position = position;
        gameObject.SetActive(true);
        WorldItem worldItem = gameObject.GetComponent<WorldItem>();
        worldItem.setItem(item);
        return worldItem;
    }

    private float life=0;
    private Item item;
    
    private void Update()
    {
        if (life < 60)
        {
            life += Time.deltaTime;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        life=0;
    }

    public void setItem(Item item)
    {
        this.item = item;
    }

    public Item getItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        gameObject.SetActive(false);
    }
}
