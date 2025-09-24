using System;

public static class Wallet
{
    public static int balance { get => _balance;}


    private static int _balance = 0;

    public static Action OnBalanceChanged;

    public static void AddCoins(int count)
    {
        if (count > 0)
        {
            _balance += count;
            OnBalanceChanged?.Invoke();
        }
    }

    public static bool BringCoins(int count)
    {
        if (count > 0 && count <= _balance)
        {
            _balance -= count;
            OnBalanceChanged?.Invoke();
            return true;
        }
        return false;
    }

    public static void ClearBalance() => _balance = 0;
}
