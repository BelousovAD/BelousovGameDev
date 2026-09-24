using System.Collections.Generic;
using BelousovGameDev.Bootstrap;
using Reflex.Attributes;
using UnityEngine;

namespace BelousovGameDev.Audios
{
    internal class AudioLoader : MonoBehaviour, ILoadable
    {
        private List<Audio> _audios;

        [Inject]
        private void Initialize(IEnumerable<Audio> audios) =>
            _audios = new List<Audio>(audios);

        public void Load() =>
            _audios.ForEach(audioObject => audioObject.Load());
    }
}