using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundCanvas : MonoBehaviour
{
    [SerializeField] private Transform _firstSkillPointer;
    [SerializeField] private Transform _secondSkillPointer;

    private PlayerInputManager _inputManager;

    private Vector3 _firstSkillDirection = Vector3.zero;
    private Vector3 _secondSkillDirection = Vector3.zero;

    private void Start()
    {
        _inputManager = ServiceLocator.Resolve<PlayerInputManager>();
       
    }

    private void Update()
    {
        if (_inputManager.IsPlayerHoldingFirstSkillButton)
        {
            _firstSkillPointer.gameObject.SetActive(true);
            var angle = Mathf.Atan2(_inputManager.FirstSkillDirection.x, _inputManager.FirstSkillDirection.y) * Mathf.Rad2Deg;
            _firstSkillPointer.transform.rotation = Quaternion.Euler(0, angle,0 );
        }
        else
        {
            _firstSkillPointer.gameObject.SetActive(false);
        }

        if (_inputManager.IsPlayerHoldingSecondSkillButton)
        {
            _secondSkillPointer.gameObject.SetActive(true);
            var angle = Mathf.Atan2(_inputManager.SecondSkillDirection.x, _inputManager.SecondSkillDirection.y) * Mathf.Rad2Deg;
            _secondSkillPointer.transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        else
        {
            _secondSkillPointer.gameObject.SetActive(false);
        }

    }
}
