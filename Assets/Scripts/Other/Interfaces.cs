interface IDamageable
{
    public void TakeDamage(float count);

    public void Broken();

    public void Heal(int hpCount = int.MaxValue);
}