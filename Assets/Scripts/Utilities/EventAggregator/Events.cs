using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region InputEvents

public class AutoattackEvent { }
public class FirstSkillEvent { public Vector3 Direction; }
public class SecondSkillEvent { public Vector3 Direction; }

public class CharacterHit
{
    public CharacterHealth Character;
    public int Amount;
    public CharacterIdentifier Hiter;
}

public class CharacterDeath
{
    public CharacterHealth Character;
    public CharacterIdentifier Killer;
}

#endregion
