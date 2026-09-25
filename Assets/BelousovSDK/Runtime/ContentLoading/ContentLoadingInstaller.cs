using Reflex.Core;
using UnityEngine;

namespace BelousovSDK.ContentLoading
{
    internal class ContentLoadingInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private Catalog _catalog;
        
        public void InstallBindings(ContainerBuilder builder) =>
            builder.RegisterValue(new ContentLoadingService(_catalog));
    }
}