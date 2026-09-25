using Reflex.Attributes;
using UnityEngine;

namespace BelousovSDK.Windows
{
    internal class StartWindowOpener : MonoBehaviour
    {
        [SerializeField] private string _windowId;

        private IWindowService _windowService;

        [Inject]
        private void Initialize(IWindowService windowService) =>
            _windowService = windowService;

        private void Start() =>
            _windowService.Open(_windowId);
    }
}