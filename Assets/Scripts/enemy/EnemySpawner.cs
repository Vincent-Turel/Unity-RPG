using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public float enemyLevel = 1;
    public GameObject player;
    public float spawnRange = 10f;
    public GameObject enemieToSpawn;
    public int numberToSpawn = 4;
    private List<GameObject> enemyList = new List<GameObject>();

    private void Update()
    {
        if (!PlayerContol.playerDead)
        {
            float distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);
            if (distanceToPlayer > spawnRange + 10 && distanceToPlayer < spawnRange + 30)
            {
                spawnToMax();
            }
            else if (distanceToPlayer > spawnRange + 100)
            {
                foreach (var enemy in enemyList)
                {
                    enemy.SetActive(false);
                }

                enemyList.Clear();
            }
        }
    }

    private void spawnToMax()
    {
        LayerMask layerMask = LayerMask.GetMask("Ground");
        while (enemyList.Count < numberToSpawn)
        {
            Vector3 position = transform.position;
            float Xpos = Random.Range(position.x - spawnRange, position.x + spawnRange);
            float Zpos = Random.Range(position.z - spawnRange, position.z + spawnRange);
            const float Ypos = 500f;
            RaycastHit hit;
            
            if (Physics.Raycast(new Vector3(Xpos, Ypos, Zpos), Vector3.down, out hit,
                500, layerMask))
            {
                if (NavMesh.SamplePosition(hit.point, out var hitOnNavMesh, 2f, NavMesh.AllAreas))
                {
                    GameObject enemy = ObjectPool.SharedInstance.GetPooledEnemy(enemieToSpawn.GetComponent<EnemyScript>().enemyType);
                    Vector3 spawnPos = hitOnNavMesh.position;
                    enemy.transform.position = spawnPos;
                    EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
                    enemyScript.spawnPos = spawnPos;
                    enemyScript.resetHelth();
                    setStatByLevel(enemyScript);
                    enemyScript.setEnemySpawner(this);
                    enemyScript.player = player.transform;
                    enemy.SetActive(true);
                    enemyList.Add(enemy);
                }
            }
        } 
    }

    public void removeDead(GameObject enemy)
    {
        enemyList.Remove(enemy);
    }

    private void setStatByLevel(EnemyScript enemyScript)
    {
        enemyScript.setLevel(enemyLevel);
        float health = 15f * enemyLevel;
        enemyScript.maxHealh = health;
        float damage = Random.Range(2f * enemyLevel, 3f * enemyLevel);
        enemyScript.setDamage(damage);
        float defence = enemyLevel*2;
        enemyScript.setDefence(defence);
    }
}

