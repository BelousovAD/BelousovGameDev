using UnityEngine;

namespace BelousovGameDev.UI.Toggle
{
    [RequireComponent(typeof(UnityEngine.UI.Toggle))]
    public abstract class AbstractToggle : MonoBehaviour
    {
        private UnityEngine.UI.Toggle _toggle;
        
        protected UnityEngine.UI.Toggle Toggle => _toggle ??= GetComponent<UnityEngine.UI.Toggle>();

        protected virtual void OnEnable() =>
            Toggle.onValueChanged.AddListener(HandleValue);

        protected virtual void OnDisable() =>
            Toggle.onValueChanged.RemoveListener(HandleValue);

        protected abstract void HandleValue(bool value);
    }
}