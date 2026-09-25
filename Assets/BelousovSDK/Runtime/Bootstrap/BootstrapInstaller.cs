using Reflex.Core;
using UnityEngine;

namespace BelousovSDK.Bootstrap
{
    internal class BootstrapInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder) =>
            builder.RegisterValue(new SavvyServicesProvider());
    }
}
