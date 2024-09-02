using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Stats bonus :")]
    public int LifeBoost;
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
}
