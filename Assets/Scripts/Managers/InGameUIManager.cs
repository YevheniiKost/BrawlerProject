using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] private Canvas _inGameCanvas;
    [SerializeField] private Healthbar _healthBarPrefab;
    [SerializeField] private DamagePopup _damagePopupPrefab;

    private Dictionary<CharacterHealth, Healthbar> _healthBars = new Dictionary<CharacterHealth, Healthbar>();

    private void Awake()
    {
        ServiceLocator.Register(this);
        EventAggregator.Subscribe<AddHealthBar>(CreateHealthBar);
        EventAggregator.Subscribe<CharacterDeath>(RemoveHealthBar);
        EventAggregator.Subscribe<CharacterHit>(CreateDamagePopup);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
    }

    private void CreateDamagePopup(object arg1, CharacterHit data)
    {
        if (data.Character.GetComponent<CharacterIdentifier>().IsControlledByThePlayer || data.Hiter.IsControlledByThePlayer)
        {
            var damagePopup = Instantiate(_damagePopupPrefab, _inGameCanvas.transform);
            damagePopup.Setup(data.Amount, data.Character, data.Hiter);
        } else { return; }
    }

    #region Manage healthbars
    private void CreateHealthBar(object arg1, AddHealthBar param)
    {
        if(_healthBars.ContainsKey(param.Character) == false)
        {
            var healthBar = Instantiate(_healthBarPrefab, _inGameCanvas.transform);
            _healthBars.Add(param.Character, healthBar);
            healthBar.SetHealth(param.Character);
        }
    }

    private void RemoveHealthBar(object arg1, CharacterDeath param)
    {
        if (_healthBars.ContainsKey(param.Character))
        {
            Destroy(_healthBars[param.Character].gameObject);
            _healthBars.Remove(param.Character);
        }
    }
    #endregion
}
