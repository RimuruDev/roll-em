using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// NOTE: Не совсем понимаю где утилиты Фокси хранит, по этому просто оставлю тут.
// Очень больно не использовать var... но нужно придерживаться кодстайла фокса, раз проект так написан, держим стиль... 
public static class CanvasGroupFadeUtility
{
    public static IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
    {
        if (canvasGroup == null)
            yield break;

        if (duration <= 0f)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            
            yield break;
        }

        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            
            float t = elapsed / duration;
            
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, t);
            
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}

public class LearningControlls : MonoBehaviour
{
    [SerializeField] private Image _demoImage;
    [SerializeField] private TMP_Text _hintText;
    [SerializeField] private Sprite[] Sprites;
    [SerializeField, Multiline] private string[] Hints;

    [SerializeField] private UnityEvent OnLearningEnded;

    [SerializeField] private bool _mirrorFirstHint = true;

    [SerializeField] private bool _useFadeForUiGroups = true;
    [SerializeField] private CanvasGroup[] _uiGroupsForFade;
    [SerializeField] private float _fadeDurationSeconds = 1.5f;

    private int _hint;

    private void Awake()
    {
        _hint = 0;
        
        if (PlayerData.showLearning)
        {
            if (_useFadeForUiGroups)
                InitializeUiGroupsForLearning();
            
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
            Vector3 scale = _demoImage.rectTransform.localScale;
            if (_hint == 0 && _mirrorFirstHint)
                scale.x = -Mathf.Abs(scale.x);
            else
                scale.x = Mathf.Abs(scale.x);
            _demoImage.rectTransform.localScale = scale;

            UiImageScaleAnimator animator = _demoImage.GetComponent<UiImageScaleAnimator>();
            if (animator != null)
                animator.Play();

            _hint++;
        }
        else
        {
            if (_useFadeForUiGroups)
                StartFadeInUiGroups();

            OnLearningEnded?.Invoke();
            PlayerData.showLearning = false;
            Links.gameManager.CancelPauseTime();
            gameObject.SetActive(false);
        }
    }

    private void InitializeUiGroupsForLearning()
    {
        if (_uiGroupsForFade == null)
            return;

        for (int i = 0; i < _uiGroupsForFade.Length; i++)
        {
            CanvasGroup group = _uiGroupsForFade[i];
            if (group == null)
                continue;

            group.alpha = 0f;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }

    private void StartFadeInUiGroups()
    {
        if (_uiGroupsForFade == null)
            return;

        for (int i = 0; i < _uiGroupsForFade.Length; i++)
        {
            CanvasGroup group = _uiGroupsForFade[i];
            if (group == null)
                continue;

            Links.gameManager.StartCoroutine(CanvasGroupFadeUtility.FadeIn(group, _fadeDurationSeconds));
        }
    }
}