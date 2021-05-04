using System.Collections;
using UnityEngine;

public class AISharedContent : MonoBehaviour
{
    public readonly CharacterIdentifier Identifier;
    public readonly CharacterCombat Combat;
    public readonly CharacterMovement Movement;
    public readonly CharacterHealth Health;
    public readonly AIMapHelper MapHelper;

    public AISharedContent(CharacterIdentifier identifier, CharacterCombat combat, CharacterMovement movement, CharacterHealth health, AIMapHelper mapHelper)
    {
        Identifier = identifier;
        Combat = combat;
        Movement = movement;
        Health = health;
        MapHelper = mapHelper;
    }
}
