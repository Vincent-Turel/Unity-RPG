using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    
    public float maxHealh = 100f;
    public GameObject HealthBarUI;
    public Slider slider;
    public float agroRange = 5f;
    public Vector3 spawnPos;
    public AudioSource hitSound;
    private EnemySpawner EnemySpawner;

    private float health;
    private bool attacked = false;
    private Camera camera;
    public EnemyType enemyType;

    private MoveController moveController;
    private float damage = 10f;
    private float defence = 0f;
    private float level;
    private int hit = 0;

    private void Start()
    {
        HealthBarUI.SetActive(false);
        moveController = GetComponent<MoveController>();
        camera = Camera.main;
        resetHelth();
        slider.value = CalculateHealth();
    }

    public void setEnemySpawner(EnemySpawner enemySpawner)
    {
        this.EnemySpawner = enemySpawner;
    }

    public void setLevel(float level)
    {
        this.level = level;
    }

    public void setDamage(float damage)
    {
        this.damage = damage;
    }

    public void setDefence(float defence)
    {
        this.defence = defence;
    }
    
    void Update()
    {
        if (health > 0)
        {
            if (HealthBarUI.activeInHierarchy)
            {
                transform.LookAt(new Vector3(camera.transform.position.x, transform.position.y, camera.transform.position.z));
            }
            slider.value = CalculateHealth();

            if (health < maxHealh)
            {
                HealthBarUI.SetActive(true);
            }
            if (health > maxHealh)
            {
                health = maxHealh;
            }
            
            if (!PlayerContol.playerDead)
            {
                float distanceToPlayer = Vector3.Distance(this.transform.position, player.position);
                if (distanceToPlayer < agroRange)
                {
                    if (getType() != EnemyType.Dragon && distanceToPlayer < 3f ||
                        getType() == EnemyType.Dragon && distanceToPlayer < 4f)
                    {
                        if (!attacked)
                        {
                            attacked = true;
                            StartCoroutine(Attack());
                        }
                    }
                    else
                    {
                        agent.destination = player.position;
                    }
                }
                else
                {
                    agent.destination = spawnPos;
                }
            }
            
            moveController.Move(agent.remainingDistance > agent.stoppingDistance ? agent.desiredVelocity : Vector3.zero);
        }
        else 
        {
            moveController.Die();
            StartCoroutine(Dead());
            player.GetComponent<PlayerContol>().target = null;
        }
    }

    public void resetHelth()
    {
        health = maxHealh;
    }
    private float CalculateHealth()
    {
        return health / maxHealh;
    }

    public void takeDamage(int damange)
    {
        if (hit % 2 == 0)
            health -= Mathf.Max(damange-defence,0);
    }
    
    IEnumerator Attack()
    {
        moveController.Attack();
        player.GetComponent<PlayerContol>().takeDamage((int)damage);
        yield return new WaitForSeconds(2f);
        moveController.EndOfAttack();
        attacked = false;
    }
    
    IEnumerator Dead()
    {
        if(getType() == EnemyType.Barbarian) GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(moveController.getDurationClip());
        this.gameObject.SetActive(false);
        EnemySpawner.removeDead(this.gameObject);
        LootDrop.dropRandomLoot(this.transform.position, level);
    }
    public void Hit()
    {
        if (hit % 2 == 0)
        {
            StartCoroutine(getHit());
            hitSound.Play();
        }
        hit++;
    }
    public IEnumerator getHit()
    {
        moveController.Hit();
        yield return new WaitForSeconds(moveController.getDurationClip());
        moveController.ResetHit();
    }
    
    public EnemyType getType()
    {
        return enemyType;
    }
    public enum EnemyType
    {
        Barbarian,
        Dragon,
        Archer,
    }
}
