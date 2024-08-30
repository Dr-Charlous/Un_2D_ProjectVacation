using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterUi : MonoBehaviour
{
    public CharacterData CharacterStats;

    [SerializeField] TextMeshProUGUI _textUi;
    [SerializeField] TextMeshProUGUI _textStats;
    [SerializeField] SpriteRenderer _renderer;

    private void Start()
    {
        _textUi.text = $@"{CharacterStats.Name} | Lvl:{CharacterStats.Level}";
        _renderer.sprite = CharacterStats.CharaSprite;

        if (_textStats != null)
            ActualiseStats();
    }

    void ActualiseStats()
    {
        string stats = $"--- {CharacterStats.Name} ---\n";
        stats += $"Lvl{CharacterStats.Level} : {CharacterStats.Exp}/{CharacterStats.ExpNextLevel} Exp\n";
        stats += $"\n";
        stats += $"Vitality : {CharacterStats.LifeStat}\n";
        stats += $"Attack   : {CharacterStats.AttackStat}\n";
        stats += $"Defense  : {CharacterStats.DefenseStat}\n";
        stats += $"Speed    : {CharacterStats.SpeedStat}\n";
        stats += $"\n";
        stats += $"Abilities : \n";

        for (int i = 0; i < CharacterStats.CharaAbilities.Length; i++)
        {
            stats += $" - {CharacterStats.CharaAbilities[i].AttackName}\n";
        }

        _textStats.text = stats;
    }
}
