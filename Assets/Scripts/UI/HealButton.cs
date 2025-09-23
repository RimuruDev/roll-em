using UnityEngine;

public class HealButton : AbilityButton
{
    public Damageable checkingObject;
    [SerializeField] private int _HPPerCoin = 10;

    private void Awake()
    {
        Initialize();

        checkingObject.OnHPChanged += ShowHideBtn;
    }

    public void HealCheckingObject()
    {
        _currentCost = Mathf.Min(CalculateHealCost(), Wallet.balance);
        if (PayAbility())
        {
            checkingObject.Heal(_HPPerCoin * _currentCost);
        }
    }

    private void ShowHideBtn()
    {
        _icon.enabled = checkingObject.hp < checkingObject.maxHP;
    }

    private int CalculateHealCost()
    {
        float hpDelta = checkingObject.maxHP - checkingObject.hp;
        return 1 + (int)(hpDelta / _HPPerCoin);
    }

    private void OnDestroy()
    {
        checkingObject.OnHPChanged -= ShowHideBtn;
        UnsubscribeEvents();
    }
}
