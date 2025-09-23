using TMPro;
using UnityEngine;

public class MenuHPBar : HPBar
{
    [SerializeField] private TMP_Text _hpText;

    public void UpdateHPText()
    {
        _hpText.text = $"{checkingObject.hp}/{checkingObject.maxHP}";
    }
}
