using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable/Attack")]
public class ScriptableAttack : ScriptableObject
{
    #region UnityEditor
#if UNITY_EDITOR
    [CustomEditor(typeof(ScriptableAttack))]
    public class ScriptableAttackEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ScriptableAttack ability = (ScriptableAttack)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Ability values :", GUILayout.MaxWidth(175));
            EditorGUI.indentLevel++;

            if (ability.AbilityType == Ability.Attack)
            {
                EditorGUILayout.LabelField("Damages :", GUILayout.MaxWidth(150));
                ability.AttackDamage = EditorGUILayout.IntField(ability.AttackDamage, GUILayout.MaxWidth(100));
            }
            else if (ability.AbilityType == Ability.Defense)
            {
                EditorGUILayout.LabelField("Damages blocked :", GUILayout.MaxWidth(150));
                ability.DamageBlock = EditorGUILayout.IntField(ability.DamageBlock, GUILayout.MaxWidth(100));
            }
            else if (ability.AbilityType == Ability.Heal)
            {
                EditorGUILayout.LabelField("Life restore :", GUILayout.MaxWidth(150));
                ability.Heal = EditorGUILayout.IntField(ability.Heal, GUILayout.MaxWidth(100));
            }
            else if (ability.AbilityType == Ability.Buff)
            {
                //EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Life boost :", GUILayout.MaxWidth(150));
                ability.LifeBoost = EditorGUILayout.IntField(ability.LifeBoost, GUILayout.MaxWidth(100));

                EditorGUILayout.LabelField("Attack boost :", GUILayout.MaxWidth(150));
                ability.AttackBoost = EditorGUILayout.IntField(ability.AttackBoost, GUILayout.MaxWidth(100));

                EditorGUILayout.LabelField("Defense boost :", GUILayout.MaxWidth(150));
                ability.DefenseBoost = EditorGUILayout.IntField(ability.DefenseBoost, GUILayout.MaxWidth(100));

                EditorGUILayout.LabelField("Speed boost :", GUILayout.MaxWidth(150));
                ability.SpeedBoost = EditorGUILayout.IntField(ability.SpeedBoost, GUILayout.MaxWidth(100));

                //EditorGUILayout.EndHorizontal();
            }

            EditorGUI.indentLevel--;
        }
    }
#endif
    #endregion UnityEditor

    public enum Target
    {
        Self,
        Other
    }

    public enum Ability
    {
        Attack,
        Defense,
        Heal,
        Buff
    }

    public string AttackName;
    public string AttackText;
    public Target TargetAttack;
    public Ability AbilityType;

    //Attack type :
    [HideInInspector] public int AttackDamage;

    //Defense type :
    [HideInInspector] public int DamageBlock;

    //Heal type :
    [HideInInspector] public int Heal;

    //Stats type :
    [HideInInspector] public int LifeBoost;
    [HideInInspector] public int AttackBoost;
    [HideInInspector] public int DefenseBoost;
    [HideInInspector] public int SpeedBoost;

    public void Attack(Character charaAttack, Character charaDefense)
    {
        int damage = charaAttack.CharacterStats.AttackStat + AttackDamage;
        int defense = charaDefense.CharacterStats.DefenseStat;
        int blockedDamages = defense - damage;
        int lifeLose = damage - blockedDamages;

        charaDefense.LifePoints -= lifeLose;
    }
}
