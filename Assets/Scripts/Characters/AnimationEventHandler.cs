using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public event Action OnAutoattackHit; 

    public void AutoattackHit()
    {
        OnAutoattackHit?.Invoke();
        Debug.Log("Hit");
    }
}
