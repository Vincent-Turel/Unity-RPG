using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equippedItemSlots : MonoBehaviour
{
    private Inventory.EquipSlotType equipSlotType;

    public Inventory.EquipSlotType GetEquipSlotType()
    {
        return this.equipSlotType;
    }

    public void setEquipSlotType(Inventory.EquipSlotType equipSlotType)
    {
        this.equipSlotType = equipSlotType;
    }
}
