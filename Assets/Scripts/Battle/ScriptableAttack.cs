using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable/Attack")]
public class ScriptableAttack : ScriptableObject
{
    public enum Target
    {
        Self,
        Other
    }

    public Target TargetAttack;
    public string AttackName;
    public string AttackText;
    [Range(0,2)]
    public int AttackRank;
    [Range(-5,25)]
    public int AttackDamage;
}
