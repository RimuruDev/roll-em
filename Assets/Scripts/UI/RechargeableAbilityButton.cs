using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RechargeableAbilityButton : MonoBehaviour
{
    public int usageCost { get => _usageCost; set => _usageCost = value; }
    public int rechargeTime { get => _rechargeTime; set => _rechargeTime = value; }
    public float currentChargePercent { get => _icon.fillAmount; set => _currentChargePercent = value; }

    [SerializeField] private int _usageCost = 1;
    [SerializeField] private int _rechargeTime = 10;

    private Image _icon;
    private Button _btn;

    private float _currentChargePercent = 0;
    private bool _isCharged = false;
    private Coroutine _showCostCoroutine;

    public UnityEvent OnAbilityUsed;

    private void Awake()
    {
        _icon = GetComponent<Image>();
        _btn = GetComponent<Button>();
        Wallet.OnBalanceChanged += SetInteractableState;
    }

    private void Start()
    {
        _btn.interactable = false;
        _icon.fillAmount = _currentChargePercent;
        StartCoroutine(Recharge());
    }

    private IEnumerator Recharge()
    {
        _isCharged = false;
        _btn.interactable = false;
        _icon.fillAmount = _currentChargePercent;

        while (_icon.fillAmount < 1)
        {
            _icon.fillAmount += Time.deltaTime / _rechargeTime;
            _currentChargePercent = _icon.fillAmount;
            yield return null;
        }
        _isCharged = true;
        SetInteractableState();
    }

    public void UseAbility()
    {
        if (Wallet.BringCoins(_usageCost))
        {
            _currentChargePercent = 0;
            OnAbilityUsed?.Invoke();
            StartCoroutine(Recharge());
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
            Links.costText.ShowCost(_usageCost);
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
        _btn.interactable = _isCharged && Wallet.balance >= _usageCost;
    }

    private void OnDestroy()
    {
        Wallet.OnBalanceChanged -= SetInteractableState;
    }
}
