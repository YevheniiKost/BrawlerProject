using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    private List<Effect> _sceneEffects = new List<Effect>();

    public void SnareEffect(CharacterIdentifier character, float speedReducerInPercent, float duration)
    {
        for (int i = 0; i < _sceneEffects.Count; i++)
        {
            if (_sceneEffects[i].CharID == character && _sceneEffects[i].EffectType == EffectType.Snare)
            {
                StopCoroutine(_sceneEffects[i].EffectCoroutine);
                character.GetComponent<CharacterMovement>()?.ReturnNormalSpeed();
                _sceneEffects.Remove(_sceneEffects[i]);
                break;
            }
            else { continue; }
        }

      var cor =  StartCoroutine(StartSnareCoroutine(character, speedReducerInPercent, duration));
        _sceneEffects.Add(new SnareEffect(duration, speedReducerInPercent, character, cor));
    }

    private IEnumerator StartSnareCoroutine(CharacterIdentifier character, float speedReducer, float duration)
    {
        character.GetComponent<CharacterMovement>()?.ModifyMovementSpeed(speedReducer);
        yield return new WaitForSeconds(duration);
        character.GetComponent<CharacterMovement>()?.ReturnNormalSpeed();
    }

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
    }
}

public class Effect
{
    public EffectType EffectType;
    public float Duration;
    public Coroutine EffectCoroutine;
    public CharacterIdentifier CharID;
}

public class SnareEffect : Effect
{
    public float SnareFactorInPercent;

    public SnareEffect(float duration, float factor, CharacterIdentifier charID, Coroutine effectCoroutine)
    {
        EffectType = EffectType.Snare;
        Duration = duration;
        SnareFactorInPercent = factor;
        CharID = charID;
        EffectCoroutine = effectCoroutine;
    }
}

public class StunEffect : Effect
{
    public StunEffect(float duration, CharacterIdentifier charID, Coroutine effectCoroutine)
    {
        EffectType = EffectType.Stun;
        Duration = duration;
        CharID = charID;
        EffectCoroutine = effectCoroutine;
    }
}

public enum EffectType
{
    Snare,
    Stun, 
    DamageOverTime
}
