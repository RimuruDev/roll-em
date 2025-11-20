using System;
using UnityEngine;

public static class PlayerData
{
    public static int kills => _kills;

    private static int _kills;
    public static float currentGameSpeed;
    public static bool showLearning = true;
    public static bool bloodMode = false;

    public static Action OnKillsCountChanged;


    public static void ClearKills()
    {
        _kills = 0;
        OnKillsCountChanged?.Invoke();
    }

    public static void EncountKills(int count = 1)
    {
        _kills += count;
        OnKillsCountChanged?.Invoke();
    }

    public static void ResetGameSpeed()
    {
        currentGameSpeed = 1;
    }

    public static void BloodModeOff() => bloodMode = false;
}
