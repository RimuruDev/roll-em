using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : DamageableWithAudio
{
    public int damage { get => _damage; }

    [Header("Enemy base settings")]
    [SerializeField] protected Targets _targetType;
    [SerializeField] protected int _damage = 1;
    [SerializeField] protected int _reward = 1;

    protected Action OnDeath;
    public static Action OnAnyDeath;
}

public enum Targets
{
    tower,
    mainShield,
}