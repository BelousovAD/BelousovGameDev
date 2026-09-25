using BelousovSDK.UI.Button;
using Reflex.Attributes;

namespace BelousovSDK.Windows.Button
{
    internal class CloseWindowButton : AbstractButton
    {
        private IWindowService _windowService;

        [Inject]
        private void Initialize(IWindowService windowService) =>
            _windowService = windowService;

        protected override void HandleClick() =>
            _windowService.CloseCurrent();
    }
}