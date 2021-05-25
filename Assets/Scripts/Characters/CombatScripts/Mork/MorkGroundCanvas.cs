using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorkGroundCanvas : GroundCanvas
{
    [SerializeField] float _secondSkillRadius;

    private void Update()
    {
        if (_inputManager.IsPlayerHoldingFirstSkillButton)
        {
            _firstSkillPointer.gameObject.SetActive(true);
        }
        else
        {
            _firstSkillPointer.gameObject.SetActive(false);
        }
        ChangePointersColor();
        TargetAimingSecondSkill(_secondSkillRadius);
    }
}
