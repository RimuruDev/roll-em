using UnityEngine;

public class MBWithAudio : MonoBehaviour
{
    [Header("Audio settings")]
    [SerializeField] protected AudioClip[] SoundOneshots;
    [SerializeField] protected float _minPitch = 1, _maxPitch = 1;
}
