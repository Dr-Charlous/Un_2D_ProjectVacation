using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenSpaceTransition : MonoBehaviour
{
    [SerializeField] BattleManager _battleManager;
    [SerializeField] GameObject _transitionIn;
    [SerializeField] GameObject _transitionOut;
    [SerializeField] string _sceneName;

    private void Start()
    {
        _transitionIn.SetActive(true);
    }

    public IEnumerator TransitionOpenSpace()
    {
        yield return new WaitForSeconds(3f);

        _transitionOut.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        if (!_battleManager.IsBattleTestScene)
            SceneManager.LoadScene(_sceneName);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
