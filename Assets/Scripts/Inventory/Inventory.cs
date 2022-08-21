using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item> lootList;
    private Dictionary<EquipSlotType, Item> equippedList;

    private int maxSize = 12;

    private int damageBonus;
    private int defenceBonus;

    public Inventory()
    {
        lootList = new List<Item>();
        equippedList = new Dictionary<EquipSlotType, Item>()
        {
            {EquipSlotType.Head , null},
            {EquipSlotType.Breastplate , null},
            {EquipSlotType.Pants , null},
            {EquipSlotType.Boots , null},
            {EquipSlotType.Weapon , null},
        };
        addItemToLootList(new Item(){ itemType = Item.ItemType.Head});
        addItemToLootList(new Item(){ itemType = Item.ItemType.Breastplate});
    }

    public void addItemToLootList(Item item)
    {
        if (lootList.Count <= maxSize)
        {
            lootList.Add(item);
        }
    }

    public void removeItemFromLootList(Item item)
    {
        lootList.Remove(item);
    }

    public void removeItemFromequipSlot(EquipSlotType equipSlot)
    {
        equippedList[equipSlot] = null;
        updateBonuses();
    }

    public List<Item> getLootList()
    {
        return lootList;
    }

    public Item getEquppedItem(EquipSlotType equipSlotType)
    {
        return equippedList[equipSlotType];
    }

    public void switchItems(Item item1, Item item2)
    {
        int indexItem1 = lootList.IndexOf(item1);
        int indexItem2 = lootList.IndexOf(item2);
        if (indexItem1 != -1 && indexItem2 != -1)
        {
            lootList[indexItem1] = item2;
            lootList[indexItem2] = item1;
        }
    }

    public void switchEquippedItems(EquipSlotType equipSlot,Item itemToEquip, Item itemToRemove)
    {
        if (itemToEquip == itemToRemove) return;
        int indexItemToEquip = lootList.IndexOf(itemToEquip);
        switch (equipSlot)
        {
            case EquipSlotType.Head:
                if (itemToEquip.itemType != Item.ItemType.Head) return;
                break;
            case EquipSlotType.Breastplate:
                if (itemToEquip.itemType != Item.ItemType.Breastplate) return;
                break;
            case EquipSlotType.Pants:
                if (itemToEquip.itemType != Item.ItemType.Pants) return;
                break;
            case EquipSlotType.Boots:
                if (itemToEquip.itemType != Item.ItemType.Boots) return;
                break;
            case EquipSlotType.Weapon:
                if (itemToEquip.itemType != Item.ItemType.Sword && itemToEquip.itemType != Item.ItemType.Axe && itemToEquip.itemType != Item.ItemType.Mace) return;
                break;
        }
        if (itemToRemove == null)
        {
            equippedList[equipSlot] = itemToEquip;
            removeItemFromLootList(itemToEquip);
            updateBonuses();
        }
        else
        {
            equippedList[equipSlot] = itemToEquip;
            lootList[indexItemToEquip] = itemToRemove;
            updateBonuses();
        }
    }

    private void updateBonuses()
    {
        damageBonus = 0;
        defenceBonus = 0;
        foreach (Item item in equippedList.Values)
        {
            if (item != null)
            {
                damageBonus += item.getDamageBonus();
                defenceBonus += item.getDefenceBonus();
            }
        }
    }

    public Item.ItemType getCurrentItemType()
    {
        Item item = equippedList[EquipSlotType.Weapon];
        if (item == null)
        {
            return Item.ItemType.None;
        }
        else
        {
            return item.itemType;
        }
    }
    public int getDamageBonus()
    {
        return damageBonus;
    }

    public int getDefenceBonus()
    {
        return defenceBonus;
    }
    
    public enum EquipSlotType
    {
        Head,
        Breastplate,
        Pants,
        Boots,
        Weapon,
        None
    }
}
