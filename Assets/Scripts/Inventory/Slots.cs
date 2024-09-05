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

            Inventory.InventoryScript.Items[Inventory.ActualItem.ItemScript.ValueInInventory] = null;
            Inventory.InventoryScript.Items[ValueInArray] = Inventory.ActualItem.ItemScript;
            Inventory.ActualItem.ItemScript.ValueInInventory = ValueInArray;
        }
    }

    //public void Drop()
    //{
    //    if (Inventory.InventoryScript.Items[ValueInArray] == null && Inventory.ActualItem != null)
    //    {
    //        if (TypeSlot.ToString() == Inventory.ActualItem.ItemScript.Type.ToString() || TypeSlot == TypeItem.Other)
    //        {
    //            Inventory.ActualItem.Position = this.transform.position;

    //            Inventory.InventoryScript.Items[Inventory.ActualItem.ItemScript.ValueInInventory] = null;
    //            Inventory.InventoryScript.Items[ValueInArray] = Inventory.ActualItem.ItemScript;

    //            Inventory.ActualItem.ItemScript.ValueInInventory = ValueInArray;
    //            //Inventory.ActualItem.Drop();
    //            Inventory.ActualItem = null;
    //            Inventory.ActualItem.GetComponent<Image>().raycastTarget = true;
    //        }
    //    }
    //}
}
