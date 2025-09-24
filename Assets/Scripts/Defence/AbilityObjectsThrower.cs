using System.Collections;
using UnityEngine;

public class AbilityObjectsThrower : MBWithAudio
{
    [SerializeField] private GameObject _potionPrefab;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ThrowPotion();
        }
    }

    private void ThrowPotion()
    {
        Links.soundManager.PlayOneshotClip(SoundOneshots[0], GameSettings.soundVolume, Random.Range(_minPitch, _maxPitch));
        Potion potion = Instantiate(_potionPrefab, transform.position, Quaternion.identity).GetComponent<Potion>();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        potion.targetPoint = mousePos;
    }

    public void AbilityActivated() => StartCoroutine(WaitPlayerInput());

    private IEnumerator WaitPlayerInput()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ThrowPotion();
                yield break;
            }

            yield return null;
        }
    }
}
