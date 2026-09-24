using Reflex.Core;
using UnityEngine;

namespace BelousovGameDev.Windows
{
    internal class WindowServiceInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private MonoBehaviour _spawner;

        public void InstallBindings(ContainerBuilder builder) =>
            builder.RegisterValue(
                new WindowService(_spawner as IWindowSpawner),
                new[] { typeof(IWindowService) });

        private void OnValidate()
        {
            if (_spawner is not null and not IWindowSpawner)
            {
                Debug.LogError($"{nameof(_spawner)} must inherited {nameof(IWindowSpawner)}");
                _spawner = null;
            }
        }
    }
}