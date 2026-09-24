using System.Collections.Generic;
using System.Linq;
using BelousovGameDev.UI.Toggle;
using Reflex.Attributes;
using UnityEngine;
using AudioType = BelousovGameDev.Audios.AudioType;

namespace BelousovGameDev.Audios
{
    internal class AudioToggle : AbstractToggle
    {
        [SerializeField] private AudioType _type;

        private Audio _audio;

        [Inject]
        private void Initialize(IEnumerable<Audio> audios) =>
            _audio = audios.FirstOrDefault(audioObject => audioObject.Type == _type);

        private void Awake() =>
            Toggle.isOn = _audio.IsActive;

        protected override void HandleValue(bool value) =>
            _audio.SetActive(value);
    }
}
