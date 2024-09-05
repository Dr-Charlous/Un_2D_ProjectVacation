using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventoryScriptable InventoryScript;
    public GameObject ItemPrefab;
    public Item ActualItem;

    public Slots[] Slots;
    public List<GameObject> Obj;

    private void Start()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].ValueInArray = i;

            if (InventoryScript.Items[i] != null)
            {
                GameObject obj = Instantiate(ItemPrefab, Slots[i].transform.position, Quaternion.identity, Slots[i].transform);
                Item item = obj.GetComponent<Item>();

                item.Inventory = this;
                Obj.Add(obj);
            }
        }
    }
}
