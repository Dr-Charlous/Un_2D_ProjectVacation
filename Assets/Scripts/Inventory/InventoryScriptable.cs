using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable/Inventory")]
public class InventoryScriptable : ScriptableObject
{
    public CharacterData Owner;

    public ItemScriptable[] Items = new ItemScriptable[26];
}
