using TMPro;
using UnityEngine;

public class MenuHPBar : HPBar
{
    [SerializeField] private TMP_Text _hpText;

    private new void Awake()
    {
        base.Awake();
        checkingObject.OnMaxHPChanged += UpdateHPText;
    }

    public void UpdateHPText()
    {
        _hpText.text = $"{checkingObject.hp}/{checkingObject.maxHP}";
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        checkingObject.OnMaxHPChanged -= UpdateHPText;
    }
}
