using BelousovGameDev.Timers.Runtime;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Timers
{
    [RequireComponent(typeof(Text))]
    internal class TimerTextView : MonoBehaviour
    {
        [SerializeField] private string _format = "{0}";
        
        private Text _textField;
        private Timer _timer;
        
        [Inject]
        private void Initialize(Timer timer) =>
            _timer = timer;

        private void Awake() =>
            _textField = GetComponent<Text>();

        private void OnEnable()
        {
            _timer.Changed += UpdateView;
            UpdateView();
        }

        private void OnDisable() =>
            _timer.Changed -= UpdateView;

        private void UpdateView() =>
            _textField.text = string.Format(_format, _timer.Time);
    }
}