using UnityEngine;

public static class PlayerData
{
    public static int kills;
    public static float currentGameSpeed;
    public static bool showLearning = true;
    public static bool bloodMode = false;

    public static void ClearKills() => kills = 0;

    public static void ResetGameSpeed() => currentGameSpeed = 1;
    public static void BloodModeOff() => bloodMode = false;
}
