using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [Header("Stats :")]
    public CharacterData CharacterStats;

    public bool IsPlayable = false;

    [Header("Components :")]
    public Image LifeDisplay;
    public Image CharacterDisplay;
    public Image StatsDisplay;
    public TextMeshProUGUI NameDisplay;
    public TextMeshProUGUI LifeDisplayText;
    public TextMeshProUGUI StatsDisplayText;

    [Header("In Battle :")]
    public bool isDefending = false;
    public int LifePoints;

    public void StartCharacter()
    {
        if (!IsPlayable)
            CharacterStats.InitCharaStatsByLevel(CharacterStats.Level);

        LifePoints = CharacterStats.LifeStat;
        ActualiseLifeDisplay();
        StatsDisplay.gameObject.SetActive(false);
        ActualiseStats();
        NameDisplay.text = $@"{CharacterStats.Name}    Lvl:{CharacterStats.Level}";
        CharacterDisplay.sprite = CharacterStats.CharaSprite;
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

        stats += $"\n\n";
        stats += $"Abilities : \n";

        for (int i = 0; i < CharacterStats.CharaAbilities.Length; i++)
        {
            stats += $" - {CharacterStats.CharaAbilities[i].AttackName}\n";
        }

        StatsDisplayText.text = stats;
    }

    public void ActualiseLifeDisplay()
    {
        LifeDisplay.fillAmount = (float)LifePoints / CharacterStats.LifeStat;
        LifeDisplayText.text = @$"{LifePoints} / {CharacterStats.LifeStat}";

        ActualiseStats();
    }

    public void ButtonShowStats()
    {
        StatsDisplay.gameObject.SetActive(!StatsDisplay.gameObject.activeInHierarchy);
    }
}
