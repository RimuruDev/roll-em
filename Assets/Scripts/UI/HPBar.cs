using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Damageable checkingObject;

    private Slider _slider;

    protected void Awake()
    {
        _slider = GetComponent<Slider>();
        Initialize();
    }

    public void Initialize()
    {
        if (checkingObject != null)
        {
            checkingObject.OnHPChanged += UpdateBar;
            checkingObject.OnMaxHPChanged += UpdateBar;

            UpdateBar();
        }
    }

    private void UpdateBar()
    {
        _slider.maxValue = checkingObject.maxHP;
        _slider.value = checkingObject.hp;
    }

    protected void OnDestroy()
    {
        if (checkingObject != null)
        {
            checkingObject.OnHPChanged -= UpdateBar;
        }
    }
}
