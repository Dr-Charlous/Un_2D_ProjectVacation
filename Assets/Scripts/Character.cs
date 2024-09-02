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
            InitCharaStatsByLevel(CharacterStats.Level);

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
        stats += $"Vitality : {CharacterStats.LifeStat} + {CharacterStats.LifeBoost}\n";
        stats += $"Attack   : {CharacterStats.AttackStat} + {CharacterStats.AttackBoost}\n";
        stats += $"Defense  : {CharacterStats.DefenseStat} + {CharacterStats.DefenseBoost}\n";
        stats += $"Speed    : {CharacterStats.SpeedStat} + {CharacterStats.SpeedBoost}\n";
        stats += $"\n";
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

    void InitCharaStatsByLevel(int level)
    {
        int life = 3;
        int attack = 0;
        int defense = 0;
        int speed = 0;

        for (int j = 0; j < level; j++)
        {
            for (int i = 0; i < 3; i++)
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

        CharacterStats.LifeStat = life;
        CharacterStats.AttackStat = attack;
        CharacterStats.DefenseStat = defense;
        CharacterStats.SpeedStat = speed;
        CharacterStats.ExpNextLevel = CharacterStats.Level * 13;
    }
}
