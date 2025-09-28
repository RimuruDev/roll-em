using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SegmentedBar _volumeBar;

    private void Awake()
    {
        _volumeBar.barValue = GameSettings.soundVolume;
        UpdateSoundVolume();
    }

    public void PlayOneshotClip(AudioClip clip, float volume = 1f, float pitch = 1f, bool is3DSound = false, Vector3 position = default)
    {
        GameObject obj = new GameObject();
        obj.transform.position = position;
        obj.transform.SetParent(transform, true);
        obj.name = $"{clip.name}-oneshot";
        AudioSource audioSource = obj.AddComponent<AudioSource>();
        audioSource.pitch = pitch;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = false;
        audioSource.spatialBlend = is3DSound ? 1 : 0;
        audioSource.Play();
        Destroy(obj, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    public void UpdateSoundVolume()
    {
        GameSettings.soundVolume = _volumeBar.barValue;
    }
}