using UnityEngine;

public class Tower : DamageableWithAudio
{
    private void Awake()
    {
        InitializeHP();
        OnDamaged += PlayDamagedSound;
    }

    private void PlayDamagedSound()
    {
        Links.soundManager.PlayOneshotClip(SoundOneshots[0], GameSettings.soundVolume, Random.Range(_minPitch, _maxPitch));
    }

    private void OnDestroy()
    {
        OnDamaged -= PlayDamagedSound;
    }
}
