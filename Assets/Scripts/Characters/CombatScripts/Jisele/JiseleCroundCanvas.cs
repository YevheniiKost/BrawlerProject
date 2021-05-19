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
        TatgetAimingSecondSkill(_secondSkillRadius);
        AutoattackCircle();
        ChagePointersColor();
    }
}
