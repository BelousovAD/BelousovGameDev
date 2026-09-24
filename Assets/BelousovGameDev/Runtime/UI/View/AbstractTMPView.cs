using TMPro;
using UnityEngine;

namespace BelousovGameDev.UI.View
{
    [RequireComponent(typeof(TMP_Text))]
    public abstract class AbstractTMPView : MonoBehaviour, IView
    {
        [SerializeField] private string _format;
        
        private TMP_Text _textField;

        protected TMP_Text TextField => _textField ??= GetComponent<TMP_Text>();

        protected string Format => _format;

        public abstract void UpdateView();
    }
}