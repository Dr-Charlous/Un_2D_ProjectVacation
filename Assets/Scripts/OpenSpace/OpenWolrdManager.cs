using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWolrdManager : MonoBehaviour
{
    [SerializeField] FileData _data;
    [SerializeField] GameObject _charaParent;
    [SerializeField] CharacterUi[] _charaUi;

    private void Awake()
    {
        _charaUi = _charaParent.GetComponentsInChildren<CharacterUi>();
    }

    void Start()
    {
        LoadPositions();

        if (_data.Characters.Count > 0)
        {
            for (int i = 0; i < _data.Characters.Count; i++)
            {
                for (int j = 0; j < _charaUi.Length; j++)
                {
                    if (_data.Characters[i] == _charaUi[j].CharacterStats)
                    {
                        Destroy(_charaUi[j].gameObject);
                    }
                }
            }

            _data.Characters.Clear();
        }
    }

    public void SavePositions()
    {
        for (int i = 0; i < _charaUi.Length; i++)
        {
            _charaUi[i].CharacterStats.CharaPosition = _charaUi[i].transform.position;
        }
    }

    public void LoadPositions()
    {
        for (int i = 0; i < _charaUi.Length; i++)
        {
            _charaUi[i].transform.position = _charaUi[i].CharacterStats.CharaPosition;
        }
    }
}
