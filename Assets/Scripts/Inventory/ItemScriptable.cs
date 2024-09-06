using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable/Item")]
public class ItemScriptable : ScriptableObject
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ItemScriptable))]
    public class ItemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ItemScriptable itemScript = (ItemScriptable)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Item values :", GUILayout.MaxWidth(175));
            EditorGUI.indentLevel++;

            if (itemScript.Type == ItemScriptable.ItemType.Helmet || itemScript.Type == ItemScriptable.ItemType.Chest || itemScript.Type == ItemScriptable.ItemType.Legs || itemScript.Type == ItemScriptable.ItemType.Shoes || itemScript.Type == ItemScriptable.ItemType.Weapon)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Attack boost :", GUILayout.MaxWidth(175));
                itemScript.AttakBoost = EditorGUILayout.IntField(itemScript.AttakBoost);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Defense boost :", GUILayout.MaxWidth(175));
                itemScript.DefenseBoost = EditorGUILayout.IntField(itemScript.DefenseBoost);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Speed boost :", GUILayout.MaxWidth(175));
                itemScript.SpeedBoost = EditorGUILayout.IntField(itemScript.SpeedBoost);
                EditorGUILayout.EndHorizontal();

                if (itemScript.Type == ItemScriptable.ItemType.Weapon)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Hit chance :", GUILayout.MaxWidth(175));
                    itemScript.HitChance = EditorGUILayout.IntSlider(itemScript.HitChance, 0, 100);
                    EditorGUILayout.EndHorizontal();
                }
            }
            else if(itemScript.Type == ItemScriptable.ItemType.Consomable)
            {

            }
        }
    }
#endif

    public enum ItemType
    {
        Helmet,
        Chest,
        Legs,
        Shoes,
        Weapon,
        Consomable,
        Other
    }

    public ItemType Type;
    [HideInInspector] public int AttakBoost;
    [HideInInspector] public int DefenseBoost;
    [HideInInspector] public int SpeedBoost;
    [HideInInspector] public int HitChance;
    [HideInInspector] public int ValueInInventory;

    public string Name;
    public string Description;
    public Sprite ItemSprite;
}
