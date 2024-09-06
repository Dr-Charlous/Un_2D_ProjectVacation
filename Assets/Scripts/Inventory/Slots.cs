using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slots : MonoBehaviour, IDropHandler
{
    public enum TypeItem
    {
        Helmet,
        Chest,
        Legs,
        Shoes,
        Weapon,
        Other
    }

    public TypeItem TypeSlot;
    public Inventory Inventory;
    public int ValueInArray;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0 && (TypeSlot.ToString() == Inventory.ActualItem.ItemScript.Type.ToString() || TypeSlot == TypeItem.Other))
        {
            GameObject dropped = eventData.pointerDrag;
            Item itemDrag = dropped.GetComponent<Item>();

            itemDrag.ParentEnd = transform;
            itemDrag.transform.parent = itemDrag.ParentEnd;

            if (TypeSlot.ToString() == Inventory.ActualItem.ItemScript.Type.ToString() && TypeSlot != TypeItem.Other && !itemDrag.IsEquip)
            {
                Inventory.InventoryScript.Owner.AttackEquip += itemDrag.ItemScript.AttakBoost;
                Inventory.InventoryScript.Owner.DefenseEquip += itemDrag.ItemScript.DefenseBoost;
                Inventory.InventoryScript.Owner.SpeedEquip += itemDrag.ItemScript.SpeedBoost;
                itemDrag.IsEquip = true;
            }
            else if(itemDrag.IsEquip)
            {
                Inventory.InventoryScript.Owner.AttackEquip -= itemDrag.ItemScript.AttakBoost;
                Inventory.InventoryScript.Owner.DefenseEquip -= itemDrag.ItemScript.DefenseBoost;
                Inventory.InventoryScript.Owner.SpeedEquip -= itemDrag.ItemScript.SpeedBoost;
                itemDrag.IsEquip = false;
            }
            Inventory.CharaUi.UpdateStatText();

            Inventory.ActualItem = null;
            Inventory.SaveInventory();
        }
    }
}
