using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [Header("Characters :")]
    [SerializeField] List<Character> _characters;
    [SerializeField] List<Character> _charactersInSpeedOrder;
    [SerializeField] List<Character> _charactersInOrder;

    [Header("Data :")]
    [SerializeField] FileData _data;
    [SerializeField] FileData _dataKill;
    [SerializeField] Button[] _buttons;
    [SerializeField] OpenSpaceTransition _transition;

    [Header("Display :")]
    [SerializeField] List<Image> _iconesTurn;
    [SerializeField] TextMeshProUGUI _combatText;
    [SerializeField] GameObject _playerAttackDisplay;
    [SerializeField] GameObject _ennemyAttackDisplay;
    [SerializeField] GameObject _DefenseDisplay;
    [SerializeField] TextMeshProUGUI _playerAttackText;
    [SerializeField] TextMeshProUGUI _ennemyAttackText;

    [Header("Values :")]
    [SerializeField] Button[] _AttackButtons;
    [SerializeField] bool _isBattleFinish = false;
    [SerializeField] float _timerBetweenActions;
    [SerializeField] int _pointsPerLevels = 3;
    [SerializeField] int _expPerMob = 5;
    [SerializeField] int _expMultiplier = 1;

    [Header("Debug :")]
    public bool IsBattleTestScene = false;

    Coroutine _coroutine = null;

    #region BeginBattle
    private void Start()
    {
        BeginBattle();
    }

    void BeginBattle()
    {
        if (IsBattleTestScene)
        {
            for (int i = 0; i < _characters.Count; i++)
            {
                _characters[i].StartCharacter();
            }
        }
        else
        {
            for (int i = 0; i < _data.Characters.Count; i++)
            {
                _characters[i].CharacterStats = _data.Characters[i];
                _characters[i].StartCharacter();
            }
        }

        int count = _characters.Count;

        for (int i = 0; i < count; i++)
        {
            Character chara = SortSpeed(_characters);

            _charactersInSpeedOrder.Add(chara);
            _charactersInOrder.Add(chara);

            _iconesTurn[i].sprite = chara.CharacterStats.CharaSprite;
        }

        //Init Turns
        int charaNumber = 0;

        for (int i = 0; i < _iconesTurn.Count; i++)
        {
            if (_charactersInOrder.Count <= charaNumber)
                charaNumber -= _charactersInOrder.Count;

            _iconesTurn[i].sprite = _charactersInOrder[charaNumber].CharacterStats.CharaSprite;
            charaNumber++;
        }

        if (_charactersInOrder[0].IsPlayable)
            ActualiseAttacksNameButtons(0);

        if (!IsBattleTestScene)
            _data.Characters.Clear();

        StartBot();
    }

    Character SortSpeed(List<Character> Characters)
    {
        int maxSpeed = -1;
        int num = -1;

        for (int i = 0; i < Characters.Count; i++)
        {
            if (maxSpeed < Characters[i].CharacterStats.SpeedStat)
            {
                maxSpeed = Characters[i].CharacterStats.SpeedStat;
                num = i;
            }
        }

        Character chara = Characters[num];
        Characters.RemoveAt(num);
        return chara;
    }

    void ChangeTurn()
    {
        if (!_isBattleFinish)
        {
            Character chara = _charactersInOrder[0];
            _charactersInOrder.RemoveAt(0);
            _charactersInOrder.Add(chara);
            int charaNumber = 0;

            for (int i = 0; i < _iconesTurn.Count; i++)
            {
                if (_charactersInOrder.Count <= charaNumber)
                    charaNumber -= _charactersInOrder.Count;

                _iconesTurn[i].sprite = _charactersInOrder[charaNumber].CharacterStats.CharaSprite;
                charaNumber++;
            }

            _charactersInOrder[0].isDefending = false;
            _coroutine = null;

            if (_charactersInOrder[0].IsPlayable)
            {
                ActualiseAttacksNameButtons(0);
                _combatText.text = $"{_charactersInOrder[0].CharacterStats.Name} Turn";
            }

            StartBot();
        }
    }

    void StartBot()
    {
        if (_coroutine != null)
            return;

        if (!_charactersInOrder[0].IsPlayable)
        {
            ChangeButtonsActivation(false);

            if (_coroutine == null)
            {
                int number = Random.Range(0, _charactersInOrder[0].CharacterStats.AttackFleeRatio);

                if (number <= _charactersInOrder[0].CharacterStats.AttackFleeRatio)
                    StartCoroutine(Attack(_charactersInOrder[0].CharacterStats.CharaAbilities[Random.Range(0, _charactersInOrder[0].CharacterStats.CharaAbilities.Length)]));
                else
                    StartCoroutine(Flee());
            }
        }
        else
            ChangeButtonsActivation(true);
    }
    #endregion BeginBattle

    #region Buttons
    public void ButtonAttack(ScriptableAttack attack)
    {
        if (_charactersInOrder[0].IsPlayable)
        {
            _coroutine = StartCoroutine(Attack(attack));
        }

        ChangeButtonsActivation(false);
    }

    public void ButtonFlee()
    {
        if (_charactersInOrder[0].IsPlayable && _coroutine == null)
        {
            _coroutine = StartCoroutine(Flee());
        }

        ChangeButtonsActivation(false);
    }

    void ChangeButtonsActivation(bool activated)
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (_buttons[i].interactable != activated)
                _buttons[i].interactable = activated;
        }
    }

    void ActualiseAttacksNameButtons(int number)
    {
        for (int i = 0; i < _AttackButtons.Length; i++)
        {
            string text = $"{_charactersInOrder[number].CharacterStats.CharaAbilities[i].AttackName}\n{_charactersInOrder[number].CharacterStats.CharaAbilities[i].AttackText}";
            _AttackButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = text;
        }
    }
    #endregion Buttons

    #region Actions
    public IEnumerator Attack(ScriptableAttack attack)
    {
        _combatText.text = $@"{_charactersInOrder[0].name} use {attack.AttackName}";
        Attack(_charactersInOrder[0], _charactersInOrder[1], attack);

        yield return new WaitForSeconds(_timerBetweenActions / 4 * 3);

        _playerAttackDisplay.SetActive(false);
        _ennemyAttackDisplay.SetActive(false);

        yield return new WaitForSeconds(_timerBetweenActions / 4);

        if (!_isBattleFinish)
            ChangeTurn();
    }

    public IEnumerator Flee()
    {
        Flee(_charactersInOrder[0], _charactersInOrder[1]);

        yield return new WaitForSeconds(_timerBetweenActions);

        if (!_isBattleFinish)
            ChangeTurn();
    }

    void Attack(Character charaAttack, Character charaDefense, ScriptableAttack attack)
    {
        int attSpeed = charaAttack.CharacterStats.SpeedStat + charaAttack.CharacterStats.SpeedBoost;
        int defSpeed = charaDefense.CharacterStats.SpeedStat + charaDefense.CharacterStats.SpeedBoost;

        // %de l'arme
        int hit = 75;

        int precision = 100 - (100 - hit) * (((1 + defSpeed) / (attSpeed - 1)) / 2);
        //Debug.Log(precision);

        int lifePointsLose = 0;
        if (Random.Range(0, 101) <= precision)
            lifePointsLose = attack.Action(charaAttack, charaDefense);
        else
            Debug.Log($"{charaAttack.CharacterStats.Name} miss");

        Defeat(charaAttack, charaDefense);
        AttackText(lifePointsLose);
        charaDefense.ActualiseLifeDisplay();
        charaAttack.ActualiseLifeDisplay();
    }

    void Flee(Character charaAttack, Character charaDefense)
    {
        int number = Random.Range(0, 10);

        if (number == 0)
        {
            _combatText.text = $@"{_charactersInOrder[0].name} fall during flee...";
            charaDefense.LifePoints--;
            Defeat(charaAttack, charaDefense);
            charaDefense.ActualiseLifeDisplay();
        }
        else
        {
            _combatText.text = $@"{_charactersInOrder[0].name} flee...";
            //OutOfBattle(charaDefense);
            ResetBoost(charaAttack);
            ResetBoost(charaDefense);
            OutOfBattle(null);
        }
    }

    public void LaunchAttack(int i)
    {
        StartCoroutine(Attack(_charactersInOrder[0].CharacterStats.CharaAbilities[i]));
    }

    void AttackText(int lifeLose)
    {
        string text = "";

        if (lifeLose <= 0)
        {
            _playerAttackText.color = Color.green;
            _ennemyAttackText.color = Color.green;
            text += "+";
        }
        else
        {
            _playerAttackText.color = Color.red;
            _ennemyAttackText.color = Color.red;
        }

        if (_charactersInOrder[0].IsPlayable)
        {
            _playerAttackDisplay.SetActive(true);
            text += -lifeLose;
            _playerAttackText.text = text;
        }
        else
        {
            _ennemyAttackDisplay.SetActive(true);
            text += -lifeLose;
            _ennemyAttackText.text = text;
        }
    }
    #endregion Actions

    #region EndBattle
    void Defeat(Character charaAttack, Character charaDefense)
    {
        if (charaDefense.LifePoints > charaDefense.CharacterStats.LifeStat)
            charaDefense.LifePoints = charaDefense.CharacterStats.LifeStat;

        if (charaDefense.LifePoints <= 0)
        {
            charaDefense.LifePoints = 0;
            ResetBoost(charaAttack);
            ResetBoost(charaDefense);
            Exp(charaAttack, charaDefense);
            OutOfBattle(charaDefense);
        }
    }

    void Exp(Character charaAttack, Character charaDefense)
    {
        //Exp optain
        float expGiven = ((_expMultiplier * _expPerMob * charaAttack.CharacterStats.Level) / 5) * ((2 * charaDefense.CharacterStats.Level + 10) / (charaAttack.CharacterStats.Level + charaDefense.CharacterStats.Level + 10)) * 2.5f;
        //float expGiven = _expPerMob * charaDefense.CharacterStats.Level * _expMultiplier;

        Debug.Log(expGiven);

        int exp = charaAttack.CharacterStats.Exp + Mathf.RoundToInt(expGiven);
        _combatText.text = $"{charaAttack.CharacterStats.Name} win the battle ! +{expGiven} Exp -> {exp}/{charaAttack.CharacterStats.ExpNextLevel}\n";

        if (exp >= charaAttack.CharacterStats.ExpNextLevel)
        {
            int life = 0;
            int attack = 0;
            int defense = 0;
            int speed = 0;
            int levelUp = 0;

            while (exp >= charaAttack.CharacterStats.ExpNextLevel)
            {
                exp -= charaAttack.CharacterStats.ExpNextLevel;
                levelUp++;

                //Level up
                //charaAttack.CharacterStats.ExpNextLevel = charaDefense.CharacterStats.Level * 15;
                charaAttack.CharacterStats.ExpNextLevel = Mathf.RoundToInt(Mathf.Pow(Mathf.Log(charaAttack.CharacterStats.Level * 2 + 1), 2) * 100);
            }

            charaAttack.CharacterStats.Level += levelUp;

            for (int i = 0; i < _pointsPerLevels * levelUp; i++)
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

            charaAttack.CharacterStats.LifeStat += life * 2;
            charaAttack.CharacterStats.AttackStat += attack;
            charaAttack.CharacterStats.DefenseStat += defense;
            charaAttack.CharacterStats.SpeedStat += speed;

            string text = $"{charaAttack.CharacterStats.Name} gain : +{levelUp} Levels";
            if (life > 0)
                text += $"\n +{life} Life points";
            if (attack > 0)
                text += $"\n +{attack} Attack points";
            if (defense > 0)
                text += $"\n +{defense} Defense points";
            if (speed > 0)
                text += $"\n +{speed} Speed points";

            _combatText.text += text;
        }

        charaAttack.CharacterStats.Exp = exp;
    }

    void ResetBoost(Character chara)
    {
        chara.CharacterStats.AttackBoost = 0;
        chara.CharacterStats.DefenseBoost = 0;
        chara.CharacterStats.SpeedBoost = 0;
    }

    public void OutOfBattle(Character charaDefense)
    {
        _isBattleFinish = true;

        if (charaDefense != null && !charaDefense.IsPlayable)
            _dataKill.Characters.Add(charaDefense.CharacterStats);

        StartCoroutine(_transition.TransitionOpenSpace());
    }
    #endregion EndBattle
}
