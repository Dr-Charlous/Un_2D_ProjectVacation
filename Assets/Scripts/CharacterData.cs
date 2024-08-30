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

    [Header("")]
    public int LifeStat;
    public int AttackStat;
    public int DefenseStat;
    public int SpeedStat;

    [Header("")]
    [Range(0, 10)]
    public int AttackDefenseRatio;
    [Range(0, 10)]
    public int ActionFleeRatio;

    public ScriptableAttack[] CharaAbilities;
}
