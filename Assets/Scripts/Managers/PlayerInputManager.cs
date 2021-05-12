using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public Vector3 MovenemtInputDirection;
    public bool IsPlayerHoldingMovementButton;

    public Vector3 FirstSkillDirection;
    public bool IsPlayerHoldingFirstSkillButton;

    public Vector3 SecondSkillDirection;
    public bool IsPlayerHoldingSecondSkillButton;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
    }
}
