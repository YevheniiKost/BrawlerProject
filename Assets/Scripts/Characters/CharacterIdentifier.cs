using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIdentifier : MonoBehaviour
{
    [SerializeField] private Transform _aIBehaviuorComponent;
    [SerializeField] private Transform _groundCanvas;

    public int Team = 0;
    public bool IsControlledByThePlayer;
    public CharacterName Name;

    private void Start()
    {
        if (IsControlledByThePlayer)
        {
            _aIBehaviuorComponent.gameObject.SetActive(false);
        }
        else
        {
            _groundCanvas.gameObject.SetActive(false);
        }
    }
}

public enum CharacterName
{
    Beor
}

