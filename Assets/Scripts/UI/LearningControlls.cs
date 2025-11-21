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

    [SerializeField] private bool _mirrorFirstHint = true;

    private int _hint;

    private void Awake()
    {
        _hint = 0;
        
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

            // NOTE: зеркалим только первый слайд, если включено
            // Так как по умолчанию в обучении раскладка отрисована не корректно, сбивает с толку. 
            // Возможно это конечно задумка автора. Но пох, это же форк, можно и пошалить.
            var scale = _demoImage.rectTransform.localScale;
            if (_hint == 0 && _mirrorFirstHint)
                scale.x = -Mathf.Abs(scale.x);
            else
                scale.x = Mathf.Abs(scale.x);
            _demoImage.rectTransform.localScale = scale;

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