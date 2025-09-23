using System;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    public float hp => _HP;
    public float maxHP => _maxHP;

    [Header("Damageable settings")]
    [SerializeField] protected float _maxHP;
    protected float _HP;

    public Action OnHPChanged;
    public Action OnDamaged;
    public Action OnBroken;

    protected void InitializeHP()
    {
        _HP = _maxHP;
        OnHPChanged?.Invoke();
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

    public void Broken()
    {
        OnBroken?.Invoke();
    }

    public void Heal(int hpCount = int.MaxValue)
    {
        _HP += hpCount;
        _HP = Mathf.Clamp(_HP, 1, _maxHP);
        OnHPChanged?.Invoke();
    }
}
