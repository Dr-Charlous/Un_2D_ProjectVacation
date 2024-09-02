using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField] List<Character> _characters;
    [SerializeField] List<Character> _charactersInSpeedOrder;
    [SerializeField] List<Character> _charactersInOrder;
    [SerializeField] List<Image> _iconesTurn;
    [SerializeField] TextMeshProUGUI _combatText;

    [Header("")]
    [SerializeField] FileData _data;
    [SerializeField] FileData _dataKill;
    [SerializeField] Button[] _buttons;
    [SerializeField] OpenSpaceTransition _transition;

    [Header("")]
    [SerializeField] GameObject _playerAttackDisplay;
    [SerializeField] GameObject _ennemyAttackDisplay;
    [SerializeField] GameObject _DefenseDisplay;
    [SerializeField] TextMeshProUGUI _playerAttackText;
    [SerializeField] TextMeshProUGUI _ennemyAttackText;

    [Header("")]
    [SerializeField] float _timerBetweenActions;
    [SerializeField] bool _isBattleFinish = false;
    [SerializeField] Button[] _AttackButtons;

    [Header("")]
    [SerializeField] bool _isBattleTestScene = false;

    Coroutine _coroutine = null;


    private void Start()
    {
        BeginBattle();
    }

    void BeginBattle()
    {
        if (_isBattleTestScene)
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

        if (_charactersInOrder[0].IsPlayable)
            ActualiseAttacksNameButtons();

        if (!_isBattleTestScene)
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
                ActualiseAttacksNameButtons();
                _combatText.text = $"{_charactersInOrder[0].CharacterStats.Name} Turn";
            }

            StartBot();
        }
    }

    void StartBot()
    {
        if (_charactersInOrder[0].IsPlayable != true)
        {
            ChangeButtonsActivation(false);

            int number = Random.Range(0, 10 + _charactersInOrder[0].CharacterStats.ActionFleeRatio);
            if (number <= _charactersInOrder[0].CharacterStats.AttackDefenseRatio && _coroutine == null)
                StartCoroutine(Attack(_charactersInOrder[0].CharacterStats.CharaAbilities[Random.Range(0, _charactersInOrder[0].CharacterStats.CharaAbilities.Length)]));
            else if (number > _charactersInOrder[0].CharacterStats.AttackDefenseRatio && number <= 10 && _coroutine == null)
                StartCoroutine(Defense());
            else if (_coroutine == null)
                StartCoroutine(Flee());
        }
        else
            ChangeButtonsActivation(true);
    }

    #region Buttons
    public void ButtonAttack(ScriptableAttack attack)
    {
        if (_charactersInOrder[0].IsPlayable)
        {
            _coroutine = StartCoroutine(Attack(attack));
        }

        ChangeButtonsActivation(false);
    }

    public void ButtonDefense()
    {
        if (_charactersInOrder[0].IsPlayable && _coroutine == null)
        {
            _coroutine = StartCoroutine(Defense());
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

    void ActualiseAttacksNameButtons()
    {
        for (int i = 0; i < _AttackButtons.Length; i++)
        {
            string text = $"{_charactersInOrder[0].CharacterStats.CharaAbilities[i].AttackName}\n{_charactersInOrder[0].CharacterStats.CharaAbilities[i].AttackText}";
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

    public IEnumerator Defense()
    {
        _combatText.text = $@"{_charactersInOrder[0].name} defend !";
        Defense(_charactersInOrder[0]);

        _DefenseDisplay.SetActive(true);

        yield return new WaitForSeconds(_timerBetweenActions / 4 * 3);

        _DefenseDisplay.SetActive(false);

        yield return new WaitForSeconds(_timerBetweenActions / 4);

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
        int lifePointsLose = 0;
        int attackValue = charaAttack.CharacterStats.AttackStat + attack.AttackDamage;

        if (attack.TargetAttack == ScriptableAttack.Target.Self)
        {
            charaDefense = charaAttack;
            charaDefense.LifePoints += attack.AttackDamage + charaDefense.CharacterStats.DefenseStat;
            lifePointsLose = attack.AttackDamage + charaDefense.CharacterStats.DefenseStat;
            lifePointsLose *= -1;
        }
        else
        {
            if (charaDefense.isDefending)
            {
                if (attackValue - charaDefense.CharacterStats.DefenseStat > 0)
                    lifePointsLose += attackValue - charaDefense.CharacterStats.DefenseStat;
                else
                    _combatText.text = @$"{charaDefense.CharacterStats.Name} block the attack";
            }
            else
                lifePointsLose += attackValue;

            charaDefense.LifePoints -= lifePointsLose;
        }

        Defeat(charaAttack, charaDefense);
        AttackText(lifePointsLose);
        charaDefense.ActualiseLifeDisplay();
    }

    void Defense(Character charaDefense)
    {
        charaDefense.isDefending = true;
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
            Exp(charaAttack, charaDefense);
            OutOfBattle(charaDefense);
        }
    }

    void Exp(Character charaAttack, Character charaDefense)
    {
        int expGiven = charaDefense.CharacterStats.LifeStat + charaDefense.CharacterStats.AttackStat + charaDefense.CharacterStats.DefenseStat + charaDefense.CharacterStats.SpeedStat;
        int exp = charaAttack.CharacterStats.Exp + expGiven;
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
                charaAttack.CharacterStats.ExpNextLevel = (charaAttack.CharacterStats.Level + levelUp) * 13;
            }

            charaAttack.CharacterStats.Level += levelUp;

            for (int i = 0; i < 3 * levelUp; i++)
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

            charaAttack.CharacterStats.LifeStat += life;
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

    public void OutOfBattle(Character charaDefense)
    {
        _isBattleFinish = true;

        if (charaDefense != null && !charaDefense.IsPlayable)
            _dataKill.Characters.Add(charaDefense.CharacterStats);

        StartCoroutine(_transition.TransitionOpenSpace());
    }
    #endregion EndBattle
}
