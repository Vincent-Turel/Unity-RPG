using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LootDrop
{
    public static void dropRandomLoot(Vector3 position, float level)
    {
        GameObject lootToDrop = ObjectPool.SharedInstance.GetRandomPooledLoot();
        if (lootToDrop != null)
        {
            setItemStatByLevel(lootToDrop.GetComponent<WorldItem>().getItem(), level);
            lootToDrop.transform.position = position;
            lootToDrop.SetActive(true);
        }
    }

    private static void setItemStatByLevel(Item item, float level)
    {
        switch(item.itemType)
        {
            case Item.ItemType.Axe:
            case  Item.ItemType.Sword:
            case Item.ItemType.Mace:
                float damage = Random.Range(level*7f, level*8f);
                item.setDamageBonus((int)damage);
                break;
            default:
                float defence = Random.Range(level*2f, level*2f);
                item.setDefenceBonus((int)defence);
                break;
        }
    }
}
