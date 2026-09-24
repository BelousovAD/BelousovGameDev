using UnityEngine;

namespace BelousovGameDev.UI.Slider
{
    [RequireComponent(typeof(UnityEngine.UI.Slider))]
    public abstract class AbstractSlider : MonoBehaviour
    {
        private UnityEngine.UI.Slider _slider;
        
        protected UnityEngine.UI.Slider Slider => _slider ??= GetComponent<UnityEngine.UI.Slider>();

        protected virtual void OnEnable() =>
            Slider.onValueChanged.AddListener(HandleValue);

        protected virtual void OnDisable() =>
            Slider.onValueChanged.RemoveListener(HandleValue);

        protected abstract void HandleValue(float value);
    }
}