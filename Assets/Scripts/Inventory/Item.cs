using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemScriptable ItemScript;
    public Inventory Inventory;
    public Image ImageItem;
    public Transform ParentEnd;
    public bool IsEquip = false;

    public void Start()
    {
        ImageItem.sprite = ItemScript.ItemSprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentEnd = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        ImageItem.raycastTarget = false;

        Inventory.ActualItem = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(ParentEnd);
        ImageItem.raycastTarget = true;
        Inventory.ActualItem = null;

        if (transform.parent != ParentEnd)
            Inventory.SaveInventory();
    }
}
