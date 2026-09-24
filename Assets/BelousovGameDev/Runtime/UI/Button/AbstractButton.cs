using UnityEngine;

namespace BelousovGameDev.UI.Button
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public abstract class AbstractButton : MonoBehaviour
    {
        private UnityEngine.UI.Button _button;

        protected UnityEngine.UI.Button Button => _button ??= GetComponent<UnityEngine.UI.Button>();

        protected virtual void OnEnable() =>
            Button.onClick.AddListener(HandleClick);

        protected virtual void OnDisable() =>
            Button.onClick.RemoveListener(HandleClick);

        protected abstract void HandleClick();
    }
}