using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiseleCroundCanvas : GroundCanvas
{
    [SerializeField] private float _secondSkillRadius;
   
    void Update()
    {
        ForwardAimingFirstSkill();
        TargetAimingSecondSkill(_secondSkillRadius);
        AutoattackCircle();
        ChangePointersColor();
    }
}
