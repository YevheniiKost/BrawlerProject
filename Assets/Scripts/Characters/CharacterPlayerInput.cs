using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayerInput : MonoBehaviour
{
    public bool ControlledByKeyboard;
    public Vector3 MovementDirection => _movementDirection;

    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _movementDirection;

    private PlayerInputManager _inputManager;

    private void Start()
    {
        _inputManager = ServiceLocator.Resolve<PlayerInputManager>();
    }

    void Update()
    {
        if (ControlledByKeyboard)
            TemoraryKeyboardInput();
        else
        {
            _movementDirection.x = _inputManager.MovenemtInputDirection.x;
            _movementDirection.z = _inputManager.MovenemtInputDirection.y;
        }
        
    }

    private void TemoraryKeyboardInput()
    {
        ClearInput();

        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _movementDirection = new Vector3(_horizontalInput, 0, _verticalInput).normalized;
    }

    private void ClearInput()
    {
        _verticalInput = 0f;
        _horizontalInput = 0f;
    }
}
