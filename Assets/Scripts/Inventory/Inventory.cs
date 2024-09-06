using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventoryScriptable InventoryScript;
    public GameObject ItemPrefab;
    public CharacterUi CharaUi;
    public Item ActualItem;

    public Slots[] SlotsInventory;
    public List<GameObject> Obj;

    private void Start()
    {
        LoadInventory();
    }

    public void SaveInventory()
    {
        for (int i = 0; i < SlotsInventory.Length; i++)
        {
            if (SlotsInventory[i].transform.childCount > 0)
            {
                InventoryScript.Items[i] = SlotsInventory[i].GetComponentInChildren<Item>().ItemScript;
                SlotsInventory[i].GetComponentInChildren<Item>().ItemScript.ValueInInventory = i;
            }
            else
            {
                InventoryScript.Items[i] = null;
            }
        }
    }

    public void LoadInventory()
    {
        InventoryScript.Owner.AttackEquip = 0;
        InventoryScript.Owner.DefenseEquip = 0;
        InventoryScript.Owner.SpeedEquip = 0;

        for (int i = 0; i < SlotsInventory.Length; i++)
        {
            SlotsInventory[i].ValueInArray = i;

            if (InventoryScript.Items[i] != null)
            {
                GameObject obj = Instantiate(ItemPrefab, SlotsInventory[i].transform.position, Quaternion.identity, SlotsInventory[i].transform);
                Item item = obj.GetComponent<Item>();

                item.ItemScript = InventoryScript.Items[i];
                item.Start();
                item.Inventory = this;
                Obj.Add(obj);

                if (SlotsInventory[i].TypeSlot.ToString() == item.ItemScript.Type.ToString() && SlotsInventory[i].TypeSlot != Slots.TypeItem.Other && !item.IsEquip)
                {
                    InventoryScript.Owner.AttackEquip += item.ItemScript.AttakBoost;
                    InventoryScript.Owner.DefenseEquip += item.ItemScript.DefenseBoost;
                    InventoryScript.Owner.SpeedEquip += item.ItemScript.SpeedBoost;
                    item.IsEquip = true;
                }
                else if (item.IsEquip)
                {
                    item.IsEquip = false;
                }
            }
        }

        CharaUi.UpdateStatText();
    }
}
