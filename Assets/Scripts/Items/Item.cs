using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemType itemType;
    private int damangeBonus=0;
    private int defenceBonus=0;
    
    public int getDamageBonus()
    {
        return damangeBonus;
    }

    public void setDamageBonus(int damageBonus)
    {
        this.damangeBonus = damageBonus;
    }
    
    public int getDefenceBonus()
    {
        return defenceBonus;
    }

    public void setDefenceBonus(int defenceBonus)
    {
        this.defenceBonus = defenceBonus;
    }

    public enum ItemType
    {
        Sword,
        Axe,
        Head,
        Breastplate,
        Pants,
        Boots,
        Mace,
        None
    }
    public Sprite getSprite()
    {
        switch (itemType)
        {
            case ItemType.Axe: return ItemAsset.Instance.AxeSprite;
            case ItemType.Head: return ItemAsset.Instance.HelmetSprite;
            case ItemType.Sword: return ItemAsset.Instance.SwordSprite;
            case ItemType.Breastplate: return ItemAsset.Instance.BreastplateSprite;
            case ItemType.Pants: return ItemAsset.Instance.PantsSprite;
            case ItemType.Boots: return ItemAsset.Instance.BootsSprite;
            case ItemType.Mace: return ItemAsset.Instance.MaceSprite;
        }
        return null;
    }
}
