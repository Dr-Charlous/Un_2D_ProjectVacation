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
            else if (ability.AbilityType == Ability.Heal)
            {
                EditorGUILayout.LabelField("Life restore :", GUILayout.MaxWidth(150));
                ability.HealPoints = EditorGUILayout.IntField(ability.HealPoints, GUILayout.MaxWidth(100));
            }
            else if (ability.AbilityType == Ability.Buff)
            {
                //EditorGUILayout.BeginHorizontal();

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
        Heal,
        Buff
    }

    public string AttackName;
    public string AttackText;
    public Target TargetAttack;
    public Ability AbilityType;

    //Attack type :
    [HideInInspector] public int AttackDamage = 0;

    //Heal type :
    [HideInInspector] public int HealPoints = 0;

    //Stats type :
    [HideInInspector] public int AttackBoost = 0;
    [HideInInspector] public int DefenseBoost = 0;
    [HideInInspector] public int SpeedBoost = 0;

    public int Action(Character charaAttack, Character charaDefense)
    {
        if (AbilityType == Ability.Attack)
        {
            return Attack(charaAttack, charaDefense);
        }
        else if (AbilityType == Ability.Heal)
        {
            return -Heal(charaAttack);
        }
        else if (AbilityType == Ability.Buff)
        {
            return Stat(charaAttack);
        }
        else return 0;
    }

    public int Attack(Character charaAttack, Character charaDefense)
    {
        int damage = charaAttack.CharacterStats.AttackStat + AttackDamage + charaAttack.CharacterStats.AttackBoost;
        int defense = charaDefense.CharacterStats.DefenseStat + charaDefense.CharacterStats.DefenseBoost;
        int damageTook = damage / 2 - defense / 4;

        if (damageTook <= 0)
        {
            return 0;
        }
        else
        {
            charaDefense.LifePoints -= damageTook;
            return damageTook;
        }
    }

    public int Heal(Character charaAttack)
    {
        int heal = charaAttack.CharacterStats.AttackStat + HealPoints;

        charaAttack.LifePoints += heal;
        if (charaAttack.LifePoints > charaAttack.CharacterStats.LifeStat)
        {
            charaAttack.LifePoints = charaAttack.CharacterStats.LifeStat;
        }
        return heal;
    }

    public int Stat(Character charaAttack)
    {
        charaAttack.CharacterStats.AttackBoost += AttackBoost;
        charaAttack.CharacterStats.DefenseBoost += DefenseBoost;
        charaAttack.CharacterStats.SpeedBoost += SpeedBoost;
        return 0;
    }
}
