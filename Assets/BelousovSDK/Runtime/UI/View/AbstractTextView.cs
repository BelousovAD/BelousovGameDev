using UnityEngine;
using UnityEngine.UI;

namespace BelousovSDK.UI.View
{
    [RequireComponent(typeof(Text))]
    public abstract class AbstractTextView : MonoBehaviour, IView
    {
        [SerializeField] private string _format;
        
        private Text _textField;

        protected Text TextField => _textField ??= GetComponent<Text>();

        protected string Format => _format;

        public abstract void UpdateView();
    }
}