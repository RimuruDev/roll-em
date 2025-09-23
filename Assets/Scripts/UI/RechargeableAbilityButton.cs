using System;
using System.Collections;
using UnityEngine;

public class RechargeableAbilityButton : AbilityButton
{
    [SerializeField] private int _usageCost = 1;
    [SerializeField] private float _rechargeTime = 10;

    protected Action OnAbilityUsed;

    private void Awake()
    {
        Initialize();
        _currentCost = _usageCost;
        StartCoroutine(OnRecharge());
    }

    public void AbilityBtn_Click()
    {
        if (PayAbility())
        {
            OnAbilityUsed?.Invoke();
            StartCoroutine(OnRecharge());
        }
    }

    private IEnumerator OnRecharge()
    {
        _icon.fillAmount = 0;
        while (_icon.fillAmount < 1)
        {
            _btn.interactable = false;
            _icon.fillAmount += Time.deltaTime / _rechargeTime;
            yield return null;
        }
        _btn.interactable = true;
    }
}
