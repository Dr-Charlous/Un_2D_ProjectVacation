using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTransition : MonoBehaviour
{
    [SerializeField] FileData _data;
    [SerializeField]GameObject _transitionIn;
    [SerializeField]GameObject _transitionOut;
    [SerializeField] string _sceneName;

    Coroutine _coroutine;

    private void Start()
    {
        _transitionIn.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CharacterUi>() != null)
        {
            if (_coroutine == null)
            {
                _data.Characters.Clear();
                _data.Characters.Add(transform.GetComponent<CharacterUi>().CharacterStats);
                _data.Characters.Add(collision.GetComponent<CharacterUi>().CharacterStats);

                _coroutine = StartCoroutine(TransitionBattle());
            }
        }
    }

    IEnumerator TransitionBattle()
    {
        GetComponent<PlayerMovement>().IsParalysed = true;
        _transitionOut.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(_sceneName);
    }
}