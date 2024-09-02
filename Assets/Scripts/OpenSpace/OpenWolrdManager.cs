using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWolrdManager : MonoBehaviour
{
    [SerializeField] FileData _data;
    [SerializeField] GameObject[] _chara;

    void Start()
    {
        LoadPositions();

        if (_data.Characters.Count > 0)
        {
            for (int i = 0; i < _data.Characters.Count; i++)
            {
                for (int j = 0; j < _chara.Length; j++)
                {
                    if (_data.Characters[i] == _chara[j].GetComponent<CharacterUi>().CharacterStats)
                    {
                        Destroy(_chara[j]);
                    }
                }
            }

            _data.Characters.Clear();
        }
    }

    public void SavePositions()
    {
        for (int i = 0; i < _chara.Length; i++)
        {
            _chara[i].GetComponent<CharacterUi>().CharacterStats.CharaPosition = _chara[i].transform.position;
        }
    }

    public void LoadPositions()
    {
        for (int i = 0; i < _chara.Length; i++)
        {
            _chara[i].transform.position = _chara[i].GetComponent<CharacterUi>().CharacterStats.CharaPosition;
        }
    }
}
