using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable/Character")]
public class CharacterData : ScriptableObject
{
    public Sprite CharaSprite;
    public string Name;
    public int Level;
    public int Exp;
    public int ExpNextLevel;

    [Header("Stats :")]
    public int LifeStat;
    public int AttackStat;
    public int DefenseStat;
    public int SpeedStat;

    [Header("Stats equipement :")]
    public int LifeEquip;
    public int AttackEquip;
    public int DefenseEquip;
    public int SpeedEquip;

    [Header("Stats bonus :")]
    public int AttackBoost;
    public int DefenseBoost;
    public int SpeedBoost;

    [Header("")]
    [Range(0, 10)]
    public int AttackFleeRatio;
    //[Range(0, 10)]
    //public int ActionFleeRatio;

    public Vector2 CharaPosition;
    public ScriptableAttack[] CharaAbilities;
    [HideInInspector] public bool _isPressed = false;

#if UNITY_EDITOR
    [CustomEditor(typeof(CharacterData))]
    public class CharacterDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            CharacterData chara = (CharacterData)target;
            chara._isPressed = EditorGUILayout.Toggle("Init stats", chara._isPressed);

            if (chara._isPressed)
            {
                chara.InitCharaStatsByLevel(chara.Level);
                chara._isPressed = false;
            }
        }
    }
#endif

    public void InitCharaStatsByLevel(int level)
    {
        int life = 3;
        int attack = 0;
        int defense = 0;
        int speed = 0;

        for (int j = 0; j < level; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                int random = Random.Range(0, 4);

                if (random == 0)
                    life++;
                else if (random == 1)
                    attack++;
                else if (random == 2)
                    defense++;
                else
                    speed++;
            }
        }

        AttackBoost = 0;
        DefenseBoost = 0;
        SpeedBoost = 0;

        LifeStat = life;
        AttackStat = attack;
        DefenseStat = defense;
        SpeedStat = speed;
        ExpNextLevel = Mathf.RoundToInt(Mathf.Pow(Mathf.Log(Level * 2 + 1), 2) * 20);
    }

    public string ActualiseStats()
    {
        string stats = $"--- {Name} ---\n";
        stats += $"Lvl: {Level} : {Exp}/{ExpNextLevel} Exp\n";
        stats += $"\n";
        stats += $"Vitality : {LifeStat}";

        if (LifeEquip > 0)
            stats += $" +{LifeEquip}";
        else if (LifeEquip < 0)
            stats += $" {LifeEquip}";

        stats += $"\nAttack   : {AttackStat}";

        if (AttackEquip > 0)
            stats += $" +{AttackEquip}";
        else if (AttackEquip < 0)
            stats += $" {AttackEquip}";

        if (AttackBoost > 0)
            stats += $" +{AttackBoost}";
        else if (AttackBoost < 0)
            stats += $" {AttackBoost}";

        stats += $"\nDefense  : {DefenseStat}";

        if (DefenseEquip > 0)
            stats += $" +{DefenseEquip}";
        else if (DefenseEquip < 0)
            stats += $" {DefenseEquip}";

        if (DefenseBoost > 0)
            stats += $" +{DefenseBoost}";
        else if (DefenseBoost < 0)
            stats += $" {DefenseBoost}";

        stats += $"\nSpeed    : {SpeedStat}";

        if (SpeedEquip > 0)
            stats += $" +{SpeedEquip}";
        else if (SpeedEquip < 0)
            stats += $" {SpeedEquip}";

        if (SpeedBoost > 0)
            stats += $" +{SpeedBoost}";
        else if (SpeedBoost < 0)
            stats += $" {SpeedBoost}";

        stats += $"\n\n";
        stats += $"Abilities : \n";

        for (int i = 0; i < CharaAbilities.Length; i++)
        {
            stats += $" - {CharaAbilities[i].AttackName}\n";
        }

        return stats;
    }
}
