using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public int amountToPool;

    public GameObject EnemieToPoolBarbarian;
    private List<GameObject> pooledEnemieBarbarian;
    
    public GameObject LootToPoolAxe;
    private List<GameObject> pooledAxe;
    public GameObject LootToPoolSword;
    private List<GameObject> pooledSword;
    public GameObject LootToPoolHead;
    private List<GameObject> pooledHead;
    public GameObject LootToPoolBreastplate;
    private List<GameObject> pooledBreastplate;
    public GameObject LootToPoolPants;
    private List<GameObject> pooledPants;
    public GameObject LootToPoolBoots;
    private List<GameObject> pooledBoots;
    public GameObject LootToPoolMace;
    private List<GameObject> pooledMace;

    private void instantiateLootObjetcts(GameObject objectToPool, ref List<GameObject> pooledObjects)
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            if (objectToPool == LootToPoolSword)
            {
                tmp.GetComponent<WorldItem>().setItem(new Item { itemType = Item.ItemType.Sword});
            }
            else if (objectToPool == LootToPoolAxe)
            {
                tmp.GetComponent<WorldItem>().setItem(new Item { itemType = Item.ItemType.Axe});
            }
            else if (objectToPool ==LootToPoolHead)
            {
                tmp.GetComponent<WorldItem>().setItem(new Item { itemType = Item.ItemType.Head});
            }
            else if (objectToPool ==LootToPoolBreastplate)
            {
                tmp.GetComponent<WorldItem>().setItem(new Item { itemType = Item.ItemType.Breastplate});
            }
            else if (objectToPool ==LootToPoolPants)
            {
                tmp.GetComponent<WorldItem>().setItem(new Item { itemType = Item.ItemType.Pants});
            }
            else if(objectToPool ==LootToPoolBoots)
            {
                tmp.GetComponent<WorldItem>().setItem(new Item { itemType = Item.ItemType.Boots});
            }
            else if (objectToPool ==LootToPoolMace)
            {
                tmp.GetComponent<WorldItem>().setItem(new Item { itemType = Item.ItemType.Mace});
            }
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
    
    private void instantiateEnemyObjetcts(GameObject objectToPool, ref List<GameObject> pooledObjects)
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    public GameObject GetPooledEnemy(EnemyScript.EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyScript.EnemyType.Barbarian:
                return GetPooledObject(pooledEnemieBarbarian);
            default:
                return null;
        }
    }
    private void Awake()
    {
        SharedInstance = this;
        //instantiate Enemies
        instantiateEnemyObjetcts(EnemieToPoolBarbarian, ref pooledEnemieBarbarian);

        //instantiate Loot
        instantiateLootObjetcts(LootToPoolSword, ref pooledSword);
        instantiateLootObjetcts(LootToPoolAxe, ref pooledAxe);
        instantiateLootObjetcts(LootToPoolBoots, ref pooledBoots);
        instantiateLootObjetcts(LootToPoolBreastplate, ref pooledBreastplate);
        instantiateLootObjetcts(LootToPoolHead, ref pooledHead);
        instantiateLootObjetcts(LootToPoolPants, ref pooledPants);
        instantiateLootObjetcts(LootToPoolMace, ref pooledMace);
    }

    public GameObject GetRandomPooledLoot()
    {
        return GetPooledLoot((Item.ItemType)Random.Range(0, Enum.GetNames(typeof(Item.ItemType)).Length));
    }

    public GameObject GetPooledLoot(Item.ItemType itemType)
    {
        switch (itemType)
        {
            case Item.ItemType.Sword:
                return GetPooledObject(pooledSword);
            case Item.ItemType.Axe:
                return GetPooledObject(pooledAxe);
            case Item.ItemType.Boots:
                return GetPooledObject(pooledBoots);
            case Item.ItemType.Breastplate:
                return GetPooledObject(pooledBreastplate);
            case Item.ItemType.Head:
                return GetPooledObject(pooledHead);
            case Item.ItemType.Pants:
                return GetPooledObject(pooledPants);
            case Item.ItemType.Mace:
                return GetPooledObject(pooledMace);
            default:
                return null;
        }
    }
    
    public GameObject GetPooledObject(List<GameObject> pooledObjects)
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}