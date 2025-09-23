using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LearningControlls : MonoBehaviour
{
    [SerializeField] private Image _demoImage;
    [SerializeField] private TMP_Text _hintText;
    [SerializeField] private Sprite[] Sprites;
    [SerializeField, Multiline] private string[] Hints;

    [SerializeField] private UnityEvent OnLearningEnded;

    private int _hint = 0;

    private void Awake()
    {
        if (PlayerData.showLearning)
        {
            Links.gameManager.PauseTime();
            ShowNextHint();
        }
        else
        {
            OnLearningEnded?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void ShowNextHint()
    {
        if (_hint < Sprites.Length)
        {
            _demoImage.sprite = Sprites[_hint];
            _hintText.text = Hints[_hint];
            _hint++;
        }
        else
        {
            OnLearningEnded?.Invoke();
            PlayerData.showLearning = false;
            Links.gameManager.CancelPauseTime();
            gameObject.SetActive(false);
        }
    }
}
