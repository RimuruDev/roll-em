using UnityEngine;

public class AmbientMusic : MonoBehaviour
{
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        _source.volume = GameSettings.soundVolume;
    }
}
