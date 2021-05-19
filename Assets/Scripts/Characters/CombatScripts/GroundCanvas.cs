using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundCanvas : MonoBehaviour
{
    [SerializeField] protected Transform _autoattackRadius;
    [SerializeField] protected Transform _firstSkillPointer;
    [SerializeField] protected Transform _secondSkillPointer;

    [SerializeField] protected List<Image> _skillsImages = new List<Image>();

    protected PlayerInputManager _inputManager;

    protected Vector3 _firstSkillDirection = Vector3.zero;
    protected Vector3 _secondSkillDirection = Vector3.zero;

    private void Start()
    {
        _inputManager = ServiceLocator.Resolve<PlayerInputManager>();
    }


    protected void AutoattackCircle()
    {
        if (_inputManager.IsPlayerHoldingAutoattackButton)
        {
            _autoattackRadius.gameObject.SetActive(true);
        }
        else
        {
            _autoattackRadius.gameObject.SetActive(false);
        }
    }

    protected void ForwardAimingSecondSkill()
    {
        if (_inputManager.IsPlayerHoldingSecondSkillButton)
        {
            if (_inputManager.SecondSkillDirection != Vector3.zero)
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
        else
        {
            _secondSkillPointer.gameObject.SetActive(false);
        }
    }

    protected void ForwardAimingFirstSkill()
    {
        if (_inputManager.IsPlayerHoldingFirstSkillButton)
        {
            if (_inputManager.FirstSkillDirection != Vector3.zero)
            {
                _firstSkillPointer.gameObject.SetActive(true);
                var angle = Mathf.Atan2(_inputManager.FirstSkillDirection.x, _inputManager.FirstSkillDirection.y) * Mathf.Rad2Deg;
                _firstSkillPointer.transform.rotation = Quaternion.Euler(0, angle, 0);

            }
            else
            {
                _firstSkillPointer.gameObject.SetActive(false);
            }
        }
        else
        {
            _firstSkillPointer.gameObject.SetActive(false);
        }
    }
    protected void TatgetAimingSecondSkill(float skillRadius)
    {
        if (_inputManager.IsPlayerHoldingSecondSkillButton)
        {
            if (_inputManager.SecondSkillDirection != Vector3.zero)
            {
                _secondSkillPointer.gameObject.SetActive(true);
                _secondSkillPointer.position = new Vector3(_inputManager.SecondSkillDirection.x, 0, _inputManager.SecondSkillDirection.y) * skillRadius + transform.position;

            }
            else
            {
                _secondSkillPointer.gameObject.SetActive(false);
            }

        }
        else
        {
            _secondSkillPointer.gameObject.SetActive(false);
        }
    }

    protected void ChagePointersColor()
    {
        if (CancleButton.IsCancle)
        {
            for (int i = 0; i < _skillsImages.Count; i++)
            {
                _skillsImages[i].color = Color.red;
            }
        }
        else
        {
            for (int i = 0; i < _skillsImages.Count; i++)
            {
                _skillsImages[i].color = Color.white;
            }
        }
    }
}
