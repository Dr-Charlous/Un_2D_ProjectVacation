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
        NameDisplay.text = $@"{CharacterStats.Name}    Lvl:{CharacterStats.Level}";
        CharacterDisplay.sprite = CharacterStats.CharaSprite;
    }

    public void ActualiseLifeDisplay()
    {
        LifeDisplay.fillAmount = (float)LifePoints / CharacterStats.LifeStat;
        LifeDisplayText.text = @$"{LifePoints} / {CharacterStats.LifeStat}";

        StatsDisplayText.text = CharacterStats.ActualiseStats();
    }

    public void ButtonShowStats()
    {
        StatsDisplay.gameObject.SetActive(!StatsDisplay.gameObject.activeInHierarchy);
    }
}
