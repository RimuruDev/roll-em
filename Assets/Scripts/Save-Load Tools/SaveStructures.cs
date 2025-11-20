using UnityEngine;

[System.Serializable]
public struct EnemySLData
{
    public Vector3 position;
    public float rotation;
    public float hp;
    public int typeIndex;
    public float remainAttackCooldown;
    public EnemySLData(Vector3 _position, float _rotation, float _hp, int _type)
    {
        position = _position;
        rotation = _rotation;
        hp = _hp;
        typeIndex = _type;
        remainAttackCooldown = 0;
    }

    public EnemySLData(Vector3 _position, float _rotation, float _hp, int _type, float _remainAttackCooldown)
    {
        position = _position;
        rotation = _rotation;
        hp = _hp;
        typeIndex = _type;
        remainAttackCooldown = _remainAttackCooldown;
    }
}

[System.Serializable]
public struct PotionSLData
{
    public Vector3 position;
    public Vector3 targetPoint;

    public PotionSLData(Vector3 _position, Vector3 _targetPoint)
    {
        position = _position;
        targetPoint = _targetPoint;
    }
}

[System.Serializable]
public struct PotionZoneSLData
{
    public Vector3 position;
    public float passedTime;

    public PotionZoneSLData(Vector3 _position, float _passedTime)
    {
        position = _position;
        passedTime = _passedTime;
    }
}

[System.Serializable]
public struct DroppedCoinsSLData
{
    public Vector3 position;
    public int coinsCount;

    public DroppedCoinsSLData(Vector3 _position, int _coinsCount)
    {
        position = _position;
        coinsCount = _coinsCount;
    }
}