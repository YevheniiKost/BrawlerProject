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
public class CharacterWakeUp { public CharacterHealth Character; }
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
public class OnLeftArrowHeroSelectClick { }
public class OnRightArrowHeroSelectClick { }
public class OnActivateCancleButton { public bool IsOn; }
public class OnSelectHeroButtonClick { }
public class SetWindow { public IWindow Window; }

#endregion

#region Gamplay enetns
public class RequestForTeamMatchData { }
public class TakeTeamFightData { public TeamMatchSetup MatchData; }
public class SelectAndSaveCurrentHero { public int HeroIndex; }
public class TransferCurrentCharacterData { public HeroSelecterData Data; }
public class FirstAbilityWasUsed { public float Cooldown; }
public class SecondAbilityWasUsed { public float Cooldown; }
public class OnGameStart { }
public class OnGetPoint { public Team CharacterTeam; }
public class UpdateCrystalCounter { public Team CurrentTeam; public int CurrentCrystalCount; }
#endregion

