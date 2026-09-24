using BelousovGameDev.Timers.Runtime;
using Reflex.Core;
using UnityEngine;

namespace Timers
{
    internal class TimerInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(new Timer(10));
        }
    }
}