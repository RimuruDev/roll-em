using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    protected int _currentCost;

    protected Button _btn;
    protected Image _icon;

    private Coroutine _currentCostUpdater;


    protected void Initialize()
    {
        _btn = GetComponent<Button>();
        _icon = GetComponent<Image>();

        Wallet.OnBalanceChanged += SetInteractableState;
    }

    protected void SetInteractableState()
    {
        _btn.interactable = Wallet.balance > 0;
    }

    public void OnMouse_Enter()
    {
        _currentCostUpdater = StartCoroutine(ShowCostUpdater());
    }

    public void OnMouse_Exit()
    {
        StopCoroutine(_currentCostUpdater);
        Links.costText.HideCost();
    }

    private IEnumerator ShowCostUpdater()
    {
        while (true)
        {
            Links.costText.ShowCost(_currentCost);
            yield return null;
        }
    }

    protected bool PayAbility()
    {
        return Wallet.BringCoins(_currentCost);
    }

    protected void UnsubscribeEvents()
    {
        Wallet.OnBalanceChanged -= SetInteractableState;
    }
}
