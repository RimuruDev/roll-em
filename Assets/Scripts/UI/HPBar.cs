using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Damageable checkingObject;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        Initialize();
    }

    public void Initialize()
    {
        if (checkingObject != null)
        {
            checkingObject.OnHPChanged += UpdateBar;

            _slider.maxValue = checkingObject.maxHP;
            _slider.value = checkingObject.hp;
        }
    }

    private void UpdateBar()
    {
        _slider.value = checkingObject.hp;
    }

    private void OnDestroy()
    {
        if (checkingObject != null)
        {
            checkingObject.OnHPChanged -= UpdateBar;
        }
    }
}
