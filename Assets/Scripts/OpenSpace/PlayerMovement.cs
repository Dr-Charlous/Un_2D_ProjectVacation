using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Inputs
    [SerializeField] PlayerMovements _playerInputs;

    private void Awake()
    {
        _playerInputs = new PlayerMovements();
    }

    private void OnEnable()
    {
        _playerInputs.Enable();
        _playerInputs.OpenWorld.Walk.performed += WalkingInputFunction;
    }

    private void OnDisable()
    {
        _playerInputs.Disable();
        _playerInputs.OpenWorld.Walk.performed -= WalkingInputFunction;
    }

    private void Start()
    {

    }
    #endregion Inputs

    public bool IsParalysed = false;

    [SerializeField] OpenWolrdManager _openWorldManager;
    [SerializeField] CharacterUi _charaUi;
    [SerializeField] Vector2 _direction;
    [SerializeField] float _speed;

    float _timer = 0;

    private void Update()
    {
        _timer += Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (_direction != Vector2.zero)
            Move(new Vector2(_direction.x, _direction.y));

        //_openWorldManager.SavePositions();
        _charaUi.CharacterStats.CharaPosition = transform.position;
    }

    void WalkingInputFunction(InputAction.CallbackContext move)
    {
        _direction = move.ReadValue<Vector2>();
    }

    void Move(Vector2 axis)
    {
        if (!IsParalysed)
        {
            Vector3 direction = new Vector3(axis.x, axis.y);
            //transform.position += direction.normalized * _speed * Time.deltaTime;

            if (direction != Vector3.zero && _timer > _speed)
            {
                transform.position += direction;
                _timer = 0;
            }
        }
    }
}