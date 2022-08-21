using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public bool isShowing = false;
    public GameObject InventoryMenu;
    private Inventory Inventory;
    public Transform inventoryContainer;
    public Transform inventoryTemplate;
    public Transform equippedItems;
    public Transform selectedItemSprite;
    public Transform trash;
    public Transform itemInfo;

    private Item selectedItem;
    private Inventory.EquipSlotType selectedItemEquipSlot;

    public void setInventory(Inventory inventory)
    {
        this.Inventory = inventory;
        RefreshInventoryItems();
    }

    private void Start()
    {
        InventoryMenu.SetActive(false);
        inventoryTemplate.gameObject.SetActive(false);
        trash.gameObject.SetActive(false);
        inventoryContainer.GetComponent<Button>().enabled = false;
        selectedItemSprite.gameObject.SetActive(false);
        InitializeEquippedItems();
    }

    private void Update()
    {
        selectedItemSprite.position = Input.mousePosition;
    }

    private void InitializeEquippedItems()
    {
        foreach (Transform child in equippedItems)
        {
            switch (child.name)
            {
                case "Head":
                    child.GetComponent<equippedItemSlots>().setEquipSlotType(Inventory.EquipSlotType.Head);
                    break;
                case "Breastplate":
                    child.GetComponent<equippedItemSlots>().setEquipSlotType(Inventory.EquipSlotType.Breastplate);
                    break;
                case "Pants":
                    child.GetComponent<equippedItemSlots>().setEquipSlotType(Inventory.EquipSlotType.Pants);
                    break;
                case "Boots":
                    child.GetComponent<equippedItemSlots>().setEquipSlotType(Inventory.EquipSlotType.Boots);
                    break;
                case "Weapon":
                    child.GetComponent<equippedItemSlots>().setEquipSlotType(Inventory.EquipSlotType.Weapon);
                    break;
                default:
                    continue;
            }
            child.GetComponent<Button>().onClick.AddListener(() => clickEquippedItem(child));
        }
    }
    
    private void RefreshInventoryItems()
    {
        RefreshLootItems();
        RefreshEquippedItems();
    }

    private void resetInventoryUImodifications()
    {
        selectedItem = null;
        selectedItemEquipSlot = Inventory.EquipSlotType.None;
        selectedItemSprite.gameObject.SetActive(false);
        unshowTrash();
        RefreshInventoryItems();
        disenableLootContatinerButton();
    }

    private void RefreshEquippedItems()
    {
        foreach (Transform child in equippedItems)
        {
            if (child.name != "Body")
            {
                Inventory.EquipSlotType equipSlotType = child.GetComponent<equippedItemSlots>().GetEquipSlotType();
                Item item = Inventory.getEquppedItem(equipSlotType);
                Image image = child.GetComponentInChildren<Image>();
                if (item != null)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                    child.GetComponentInChildren<Image>().sprite = Inventory.getEquppedItem(equipSlotType).getSprite();
                    if(equipSlotType == Inventory.EquipSlotType.Weapon) child.GetComponentInChildren<Text>().text = ""+item.getDamageBonus();
                    else child.GetComponentInChildren<Text>().text = ""+item.getDefenceBonus();
                }
                else
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
                    child.GetComponentInChildren<Text>().text = "";
                }
            }
        }
    }
    
    private void RefreshLootItems()
    {
        foreach (Transform child in inventoryContainer)
        {
            if(child == inventoryTemplate || child == trash || child.name == "itemContainerBackground") continue;
            Destroy(child.gameObject);
        }
        int x = 0;
        float itemSize =  1.3f*inventoryTemplate.GetComponent<RectTransform>().rect.width;
        float itemSizeHeight =  0.9f*itemSize;
        foreach (Item item in Inventory.getLootList())
        {
            RectTransform itemSlotRectTransform =
                Instantiate(inventoryTemplate, inventoryContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.GetComponent<Button>().onClick.AddListener(() => clickItem(item));
            itemSlotRectTransform.anchoredPosition = new Vector2((x%6)*itemSize , -(x /6) * itemSizeHeight);
            itemSlotRectTransform.gameObject.GetComponentsInChildren<Image>()[0].sprite = item.getSprite();
            if (item.itemType == Item.ItemType.Axe || item.itemType == Item.ItemType.Sword ||
                item.itemType == Item.ItemType.Mace)
                itemSlotRectTransform.GetComponentInChildren<Text>().text = ""+item.getDamageBonus();
            else itemSlotRectTransform.GetComponentInChildren<Text>().text = ""+item.getDamageBonus();
                x++;
        }
    }
    
    private void clickItem(Item item)
    {
        if (selectedItem != null)
        {
            if (selectedItemEquipSlot == Inventory.EquipSlotType.None)
            {
                Inventory.switchItems(item, selectedItem);
                resetInventoryUImodifications();
            }
            else
            {
                Inventory.switchEquippedItems(selectedItemEquipSlot, item, selectedItem);
                resetInventoryUImodifications();
            }
        }
        else
        {
            selectedItemSprite.gameObject.SetActive(true);
            selectedItemSprite.GetComponent<Image>().sprite = item.getSprite();
            selectedItem = item;
            showTrash();
        }
    }

    private void clickEquippedItem(Transform child)
    {
        Inventory.EquipSlotType equipSlotType = child.GetComponent<equippedItemSlots>().GetEquipSlotType();
        Item item = Inventory.getEquppedItem(equipSlotType);
        if (selectedItem != null)
        {
            Inventory.switchEquippedItems(equipSlotType,selectedItem, item);
            resetInventoryUImodifications();
        }
        else if(item != null)
        {
            selectedItemSprite.gameObject.SetActive(true);
            selectedItemSprite.GetComponent<Image>().sprite = item.getSprite();
            selectedItem = item;
            selectedItemEquipSlot = equipSlotType;
            enableLootContatinerButton();
            showTrash();
        }
    }
    
    public void retrunToInventory()
    {
        Inventory.addItemToLootList(selectedItem);
        removeItem();
        resetInventoryUImodifications();
    }
    
    public void removeItem()
    {
        if (selectedItemEquipSlot == Inventory.EquipSlotType.None)
        {
            Inventory.removeItemFromLootList(selectedItem);
        }
        else
        {
            Inventory.removeItemFromequipSlot(selectedItemEquipSlot);
        }
        resetInventoryUImodifications();
    }

    private void showTrash()
    {
        trash.gameObject.SetActive(true);
    }

    private void unshowTrash()
    {
        trash.gameObject.SetActive(false);
    }
    
    public void InventoryShow()
    {
        isShowing = true;
        InventoryMenu.SetActive(true);
        resetInventoryUImodifications();
    }
    
    public void InventoryUnshow()
    {
        isShowing = false;
        InventoryMenu.SetActive(false);
    }

    private void enableLootContatinerButton()
    {
        inventoryContainer.GetComponent<Button>().enabled = true;
    }
    private void disenableLootContatinerButton()
    {
        inventoryContainer.GetComponent<Button>().enabled = false;
    }
}
