using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerContol : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform target ;
    public bool targetIsEnemy=false;
    private bool attacked = false;

    public MoveController moveController;
    public AnimatorOverrider animatorOverrider;
    public AnimatorOverrideController[] animatorOverrideControllers;
    public GameObject healthBar;
    private bool godMode= false;

    public GameObject Axe;
    public GameObject Sword;
    public GameObject Mace;
    private GameObject currentWeapon;

    public float maxHealth = 100;
    public float health;
    public static bool playerDead = false;
    public Inventory inventory;
    public InventoryUI uiInventory;

    public int strength;
    public int agility;
    private void Start()
    {
        moveController = GetComponent<MoveController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        Mace.SetActive(false);
        Axe.SetActive(false);
        Sword.SetActive(true);
        currentWeapon = Sword;

        health = maxHealth;
        inventory = new Inventory();
        uiInventory.setInventory(inventory);

        Item initWeapon = new Item();
        initWeapon.itemType = Item.ItemType.Sword;
        initWeapon.setDamageBonus(3);
        inventory.switchEquippedItems(Inventory.EquipSlotType.Weapon, initWeapon, null);

        Item axe = new Item() {itemType = Item.ItemType.Axe};
        axe.setDamageBonus(4);
        inventory.addItemToLootList(axe);
        
        Item mace = new Item() {itemType = Item.ItemType.Mace};
        mace.setDamageBonus(4);
        inventory.addItemToLootList(mace);
    }

    void Update()
    {
        testAlive();
        if (!playerDead)
        {
            updateHealthBar();
            changeWeapon(inventory.getCurrentItemType());
            if (Input.GetKeyDown(KeyCode.G))
            {
                godMode = !godMode;
            }

            if (target && targetIsEnemy)
            {
                if (!attacked)
                {
                    float distanceToTarget = Vector3.Distance(this.transform.position, target.position);
                    agent.destination = target.transform.position;
                    if ((target.GetComponent<EnemyScript>().getType() != EnemyScript.EnemyType.Dragon &&
                        distanceToTarget < 3.0f) ||
                        (target.GetComponent<EnemyScript>().getType() == EnemyScript.EnemyType.Dragon &&
                        distanceToTarget < 4.0f))
                    {
                        attacked = true;
                        agent.destination = this.transform.position;
                        StartCoroutine("Attack");
                    }
                }
                else
                {
                    transform.LookAt(new Vector3(target.transform.position.x, transform.position.y,
                        target.transform.position.z));
                }
            }
            else if (target && !targetIsEnemy)
            {
                float distanceToTarget = Vector3.Distance(this.transform.position, target.position);
                agent.destination = target.transform.position;
                if (distanceToTarget < 1f)
                {
                    inventory.addItemToLootList(target.GetComponent<WorldItem>().getItem());
                    target.GetComponent<WorldItem>().DestroySelf();
                    target = null;
                }
            }

            if (Input.GetMouseButton(0))
            {
                RaycastHit Hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out Hit);
                if (Hit.transform.CompareTag("Floor"))
                {
                    attacked = false;
                    target = null;
                    targetIsEnemy = false;
                    agent.destination = Hit.point;
                }
                else if (Hit.transform.CompareTag("Enemy"))
                {
                    target = Hit.transform;
                    targetIsEnemy = true;
                }
                else if (Hit.transform.CompareTag("Item"))
                {
                    attacked = false;
                    target = Hit.transform;
                    targetIsEnemy = false;
                }
            }

            moveController.Move(agent.remainingDistance > agent.stoppingDistance
                ? agent.desiredVelocity
                : Vector3.zero);
        }
    }

    private IEnumerator Attack()
    {
        if (currentWeapon == Sword)
        {
            animatorOverrider.setAnimations(animatorOverrideControllers[0]);
        }
        else if (currentWeapon == Axe)
        {
            animatorOverrider.setAnimations(animatorOverrideControllers[1]);
        }
        else if (currentWeapon == Mace)
        {
            animatorOverrider.setAnimations(animatorOverrideControllers[2]);
        }
        moveController.Attack();
        yield return new WaitForSeconds(.4f); 
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1.8f);
        moveController.EndOfAttack();
        attacked = false;
    }

    public void updateHealthBar()
    {
        Slider healthSlider = healthBar.GetComponentInChildren<Slider>();
        healthSlider.value = health / maxHealth;
        TextMeshProUGUI healthText = healthBar.GetComponentInChildren<TextMeshProUGUI>();
        healthText.text = "" + health + " / " + maxHealth;
    }

    public void changeWeapon(Item.ItemType weapon)
    {
        if (weapon == Item.ItemType.Axe)
        {
            Axe.SetActive(true);
            Sword.SetActive(false);
            Mace.SetActive(false);
            currentWeapon = Axe;
        }
        else if(weapon == Item.ItemType.Sword)
        {
            Axe.SetActive(false);
            Sword.SetActive(true);
            Mace.SetActive(false);
            currentWeapon = Sword;
        }
        else if (weapon == Item.ItemType.Mace)
        {
            Axe.SetActive(false);
            Sword.SetActive(false);
            Mace.SetActive(true);
            currentWeapon = Mace;
        }
        else
        {
            Axe.SetActive(false);
            Sword.SetActive(false);
            Mace.SetActive(false);
            currentWeapon = null;
        }
    }

    void testAlive()
    {
        if (health <= 0) 
        {
            playerDead = true;
            gameObject.SetActive(false);
        }
        else
        {
            playerDead = false;
        }
    }
    
    public void takeDamage(int damage)
    {
        if (!playerDead && !godMode)
        {
            health -= Mathf.Max(damage-strength-inventory.getDefenceBonus(),0);
        }
    }
    
    public void regenHealth()
    {
        if (health < 100)
        {
            health++;
        }
    }
    
    public void DealMeleeDamage(GameObject enemy)
    {
        if (attacked)
        {
            int damageToDeal = inventory.getDamageBonus() + strength;
            enemy.transform.GetComponent<EnemyScript>().takeDamage(damageToDeal);
        }
    }
    
    public void DealRangedDamage(int weaponDamage, GameObject enemy)
    {
        int damageToDeal = weaponDamage+agility;
        enemy.transform.parent.GetComponent<EnemyScript>().takeDamage(damageToDeal);
    }

    public void playerRespawn()
    {
        gameObject.SetActive(true);
        transform.position = new Vector3(258,0,196);
        health = maxHealth;
    }

    public Item sheathWeapon()
    {
        Item weapon = inventory.getEquppedItem(Inventory.EquipSlotType.Weapon);
        inventory.removeItemFromequipSlot(Inventory.EquipSlotType.Weapon);
        return weapon;
    }

    public void equipWeapon(Item weapon)
    {
        inventory.switchEquippedItems(Inventory.EquipSlotType.Weapon, weapon, null);
    }

    public bool isAttacking()
    {
        return attacked;
    }
}
