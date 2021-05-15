using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeorGroundCanvas : GroundCanvas
{
    
    void Update()
    {
        ForwardAimingFirstSkill();
        ForwardAimingSecondSkill();
        AutoattackCircle();
    }
}
