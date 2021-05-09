using System.Collections;
using UnityEngine;

public class AISharedContent
{
    public readonly CharacterIdentifier Identifier;
    public readonly CharacterCombat Combat;
    public readonly CharacterMovement Movement;
    public readonly CharacterHealth Health;
    public readonly MapHelper MapHelper;

    public AISharedContent(CharacterIdentifier identifier, CharacterCombat combat, CharacterMovement movement, CharacterHealth health, MapHelper mapHelper)
    {
        Identifier = identifier;
        Combat = combat;
        Movement = movement;
        Health = health;
        MapHelper = mapHelper;
    }
}
