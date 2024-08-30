using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWolrdManager : MonoBehaviour
{
    [SerializeField] FileData _data;
    [SerializeField] GameObject[] _chara;

    private void Start()
    {
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
}
