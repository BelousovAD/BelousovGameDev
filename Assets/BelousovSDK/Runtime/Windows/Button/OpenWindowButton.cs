using BelousovSDK.UI.Button;
using Reflex.Attributes;
using UnityEngine;

namespace BelousovSDK.Windows.Button
{
    public class OpenWindowButton : AbstractButton
    {
        [SerializeField] private WindowId _windowId;
        [SerializeField][Min(0)] private int _countToClose;

        private IWindowService _windowService;

        [Inject]
        private void Initialize(IWindowService windowService) =>
            _windowService = windowService;

        protected override void HandleClick() =>
            _windowService.Open(_windowId.Value, _countToClose);
    }
}