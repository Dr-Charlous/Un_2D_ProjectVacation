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
            _textStats.text = CharacterStats.ActualiseStats();
    }
}
