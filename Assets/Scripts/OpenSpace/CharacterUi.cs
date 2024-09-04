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
        stats += $"Lvl: {CharacterStats.Level} : {CharacterStats.Exp}/{CharacterStats.ExpNextLevel} Exp\n";
        stats += $"\n";
        stats += $"Vitality : {CharacterStats.LifeStat}";
        stats += $"\nAttack   : {CharacterStats.AttackStat}";

        if (CharacterStats.AttackBoost > 0)
            stats += $" +{CharacterStats.AttackBoost}";
        else if (CharacterStats.AttackBoost < 0)
            stats += $" {CharacterStats.AttackBoost}";

        stats += $"\nDefense  : {CharacterStats.DefenseStat}";

        if (CharacterStats.DefenseBoost > 0)
            stats += $" +{CharacterStats.DefenseBoost}";
        else if (CharacterStats.DefenseBoost < 0)
            stats += $" {CharacterStats.DefenseBoost}";

        stats += $"\nSpeed    : {CharacterStats.SpeedStat}";

        if (CharacterStats.SpeedBoost > 0)
            stats += $" +{CharacterStats.SpeedBoost}";
        else if (CharacterStats.SpeedBoost < 0)
            stats += $" {CharacterStats.SpeedBoost}";

        stats += $"\n";
        stats += $"Abilities : \n";

        for (int i = 0; i < CharacterStats.CharaAbilities.Length; i++)
        {
            stats += $" - {CharacterStats.CharaAbilities[i].AttackName}\n";
        }

        _textStats.text = stats;
    }
}
