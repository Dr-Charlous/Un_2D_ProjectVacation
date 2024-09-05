using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiOpWManager : MonoBehaviour
{
    [SerializeField] Button[] _buttons;

    public void ActivateButton(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
}
