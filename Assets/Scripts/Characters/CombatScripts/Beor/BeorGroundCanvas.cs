using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeorGroundCanvas : GroundCanvas
{
    
    void Update()
    {
        ChangePointersColor();
        ForwardAimingFirstSkill();
        ForwardAimingSecondSkill();
        AutoattackCircle();
    }
}
