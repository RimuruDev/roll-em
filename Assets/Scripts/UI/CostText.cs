using TMPro;
using UnityEngine;

public class CostText : MonoBehaviour
{
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _unsuccessColor;

    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void ShowCost(int cost)
    {
        gameObject.SetActive(true);
        _text.text = $"-{cost}";

        if (cost <= Wallet.balance)
        {
            _text.color = _successColor;
        }
        else
        {
            _text.color = _unsuccessColor;
        }
    }

    public void HideCost()
    {
        gameObject.SetActive(false);
    }
}
