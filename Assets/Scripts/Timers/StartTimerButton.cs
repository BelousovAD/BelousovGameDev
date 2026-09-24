using System;
using BelousovGameDev.Timers.Runtime;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Timers
{
    [RequireComponent(typeof(Button))]
    internal class StartTimerButton : MonoBehaviour
    {
        private Button _button;
        private Timer _timer;

        [Inject]
        private void Initialize(Timer timer) =>
            _timer = timer;

        private void Awake() =>
            _button = GetComponent<Button>();

        private void OnEnable() =>
            _button.onClick.AddListener(HandleClick);
        
        private void OnDisable() =>
            _button.onClick.RemoveListener(HandleClick);

        private void HandleClick() =>
            _timer.Start();
    }
}