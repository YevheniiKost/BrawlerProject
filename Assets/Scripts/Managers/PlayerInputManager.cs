using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [HideInInspector]
    public Vector3 MovenemtInputDirection;
    [HideInInspector]
    public bool IsPlayerHoldingMovementButton;

    [HideInInspector]
    public Vector3 FirstSkillDirection;
    [HideInInspector]
    public bool IsPlayerHoldingFirstSkillButton;

    [HideInInspector]
    public Vector3 SecondSkillDirection;
    [HideInInspector]
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
