using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayerInput : MonoBehaviour
{
    public float HorizontalInput => _horizontalInput;
    public float VerticalInput => _verticalInput;

    private float _horizontalInput;
    private float _verticalInput;

    void Update()
    {
        TemoraryKeyboardInput();
    }

    private void TemoraryKeyboardInput()
    {
        ClearInput();

        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
    }

    private void ClearInput()
    {
        _verticalInput = 0f;
        _horizontalInput = 0f;
    }
}
