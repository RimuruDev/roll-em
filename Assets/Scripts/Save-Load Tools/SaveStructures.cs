using UnityEngine;

[System.Serializable]
public struct EnemySLData
{
    public Vector3 position;
    public float rotation;
    public float hp;
    public int typeIndex;

    public EnemySLData(Vector3 _position, float _rotation, float _hp, int _type)
    {
        position = _position;
        rotation = _rotation;
        hp = _hp;
        typeIndex = _type;
    }
}
