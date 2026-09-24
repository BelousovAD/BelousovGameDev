using Reflex.Core;
using UnityEngine;

namespace BelousovGameDev.Bootstrap
{
    internal class BootstrapInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder) =>
            builder.RegisterValue(new SavvyServicesProvider());
    }
}
