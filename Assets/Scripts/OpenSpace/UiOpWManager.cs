using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiOpWManager : MonoBehaviour
{
    [SerializeField] Button[] _buttons;
    [SerializeField] GameObject[] _objOnButtons;

    private void Start()
    {
        for (int i = 0; i < _objOnButtons.Length; i++)
        {
            _objOnButtons[i].SetActive(false);
        }
    }

    public void ActivateButton(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
}
