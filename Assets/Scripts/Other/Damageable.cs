using System;
using UnityEngine;

public class Damageable : MBWithAudio, IDamageable
{
    public float hp => _HP;
    public float maxHP
    {
        get => _maxHP;
        set
        {
            if (value > 0)
            {
                _maxHP = value;
                _HP = value;
                OnMaxHPChanged?.Invoke();
                OnHPChanged?.Invoke();
            }
        }
    }

    [Header("Damageable settings")]
    [SerializeField] protected float _maxHP;
    protected float _HP = -1;

    public Action OnHPChanged;
    public Action OnMaxHPChanged;
    public Action OnDamaged;
    public Action OnBroken;

    protected void Awake()
    {
        if (_HP == -1)
        {
            _HP = _maxHP;
            OnHPChanged?.Invoke();
        }
    }

    public void TakeDamage(float count)
    {
        _HP -= count;
        OnHPChanged?.Invoke();
        OnDamaged?.Invoke();

        if (_HP <= 0)
        {
            //_HP = 0;
            Broken();
        }
    }

    public void Heal(int hpCount = int.MaxValue)
    {
        _HP += hpCount;
        _HP = Mathf.Clamp(_HP, 1, _maxHP);
        OnHPChanged?.Invoke();
    }

    public void SetHP(float count)
    {
        _HP = Mathf.Clamp(count, 0.01f, _maxHP);
        OnHPChanged?.Invoke();
    }

    public void Broken()
    {
        OnBroken?.Invoke();
    }
}
