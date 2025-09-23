using TMPro;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    [SerializeField] private TMP_Text _windowLabel;

    private void Awake()
    {
        if (PlayerData.bloodMode) Activate();
    }

    public void Activate()
    {
        _windowLabel.text = "roll'em.BLOOD";
        _windowLabel.color = Color.crimson;
        PlayerData.bloodMode = true;
    }
}
