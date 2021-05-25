using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterIdentifier _characterIdentifier;
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private CharacterHealth _characterHealth;
    [SerializeField] private CharacterPlayerInput _characterInput;
    [SerializeField] private CharacterAnimation _characteAnimation;
    [SerializeField] private CharacterCombat _characterCombat;

    public CharacterIdentifier CharID => _characterIdentifier;
    public CharacterMovement CharMovement => _characterMovement;
    public CharacterHealth CharHealth => _characterHealth;
    public CharacterPlayerInput CharInput => _characterInput;
    public CharacterAnimation CharAnimation => _characteAnimation;
    public CharacterCombat CharCombat => _characterCombat;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;

    private NavMeshAgent _navMeshAgent;

    private void Awake() => _navMeshAgent = GetComponent<NavMeshAgent>();
}
