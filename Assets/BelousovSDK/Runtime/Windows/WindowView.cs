using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace BelousovSDK.Windows
{
    [RequireComponent(typeof(CanvasGroup))]
    internal class WindowView : MonoBehaviour
    {
        private static readonly Vector3 MaxScale = Vector3.one;
        
        [SerializeField] private Window _window;
        [SerializeField] private RectTransform _content;
        [Header("Animation")]
        [SerializeField][Min(0f)] private float _duration;
        [SerializeField] private Ease _ease = Ease.OutQuad;
        [SerializeField][Range(0f, 1f)] private float _startScale = 0.92f;
        
        private CanvasGroup _canvasGroup;
        private Tweener _tweener;
        private Coroutine _changeActivity;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _window.ActivityChanged += UpdateActivity;
            _window.InteractableChanged += UpdateInteractable;
        }
        
        private void OnDestroy()
        {
            _window.InteractableChanged -= UpdateInteractable;
            _window.ActivityChanged -= UpdateActivity;
            StopAnimation();
        }
        
        private void UpdateActivity()
        {
            StopAnimation();

            if (_window.IsActive)
            {
                transform.SetAsLastSibling();
                _content.localScale = _duration > 0f ? MaxScale * _startScale : MaxScale;
                _canvasGroup.interactable = false;
                gameObject.SetActive(true);
            }
        
            _changeActivity = StartCoroutine(ChangeActivityRoutine());
        }

        private void UpdateInteractable()
        {
            if (_changeActivity == null)
            {
                _canvasGroup.interactable = _window.IsInteractable;
            }
        }

        private void StopAnimation()
        {
            if (_changeActivity != null)
            {
                StopCoroutine(_changeActivity);
                _changeActivity = null;
            }

            _tweener.Kill();
        }

        private IEnumerator ChangeActivityRoutine()
        {
            _tweener.Kill();
        
            if (_window.IsActive)
            {
                _tweener = _content.DOScale(MaxScale, _duration)
                    .SetEase(_ease)
                    .SetUpdate(true)
                    .OnComplete(() => _canvasGroup.interactable = _window.IsInteractable);
            }
            else
            {
                _canvasGroup.interactable = false;
                _content.localScale = MaxScale;
                _changeActivity = null;
                gameObject.SetActive(false);
                
                yield break;
            }
        
            yield return _tweener.WaitForCompletion();
        
            _changeActivity = null;
        }
    }
}
