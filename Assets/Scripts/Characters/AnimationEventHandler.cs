using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public event Action OnAutoattackHit;
    public event Action OnFirstSkillHit;
    public event Action OnSecondSkillHit;

    public void AutoattackHit()
    {
        OnAutoattackHit?.Invoke();
    }

    public void FistSkillHit()
    {
        OnFirstSkillHit?.Invoke();
    }

    public void SecondSkillHit()
    {
        OnSecondSkillHit?.Invoke();
    }
}
