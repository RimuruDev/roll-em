using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealButton : MonoBehaviour
{
    [SerializeField] private Damageable _relatedObject;
    [SerializeField] private int _HPPerCoin = 10;

    private Image _icon;
    private Button _btn;

    private Coroutine _showCostCoroutine;

    private void Awake()
    {
        _icon = GetComponent<Image>();
        _btn = GetComponent<Button>();
        Wallet.OnBalanceChanged += SetInteractableState;
        _relatedObject.OnHPChanged += ShowHideBtn;
    }

    public void HealRelatedObject()
    {
        int bringCoins = Mathf.Min(CalculateHealCost(), Wallet.balance);
        if (Wallet.BringCoins(bringCoins))
        {
            _relatedObject.Heal(bringCoins * _HPPerCoin);
        }
    }

    public void OnMouse_Enter()
    {
        _showCostCoroutine = StartCoroutine(ShowCost());
    }

    private IEnumerator ShowCost()
    {
        while (true)
        {
            Links.costText.ShowCost(CalculateHealCost());
            yield return null;
        }
    }

    public void OnMouse_Exit()
    {
        StopCoroutine(_showCostCoroutine);
        Links.costText.HideCost();
    }

    private void SetInteractableState()
    {
        _btn.interactable = Wallet.balance > 0;
    }

    private void ShowHideBtn()
    {
        _icon.enabled = _relatedObject.hp < _relatedObject.maxHP;
    }

    private int CalculateHealCost()
    {
        float hpDelta = _relatedObject.maxHP - _relatedObject.hp;
        return 1 + (int)(hpDelta / _HPPerCoin);
    }


    private void OnDestroy()
    {
        Wallet.OnBalanceChanged -= SetInteractableState;
        _relatedObject.OnHPChanged += ShowHideBtn;
    }
}
