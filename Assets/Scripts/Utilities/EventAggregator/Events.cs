using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region InputEvents

public class AutoattackEvent { }
public class FirstSkillEvent { public Vector3 Direction; }
public class SecondSkillEvent { public Vector3 Direction; }
#endregion

#region Character interaction events
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

public class AddHealthBar
{
    public CharacterHealth Character;
}

#endregion

#region UI events

public class OnGameModClick { }
public class OnSelectHeroClick { }
public class OnReturnToMainMenuClick { }
public class OnPlayClick { }

public class SetWindow { public IWindow Window; }

#endregion

