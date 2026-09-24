using UnityEngine;
using UnityEngine.UI;

namespace BelousovGameDev.UI.View
{
    [RequireComponent(typeof(Image))]
    public abstract class AbstractImageView : MonoBehaviour, IView
    {
        [SerializeField] private Sprite _defaultSprite;
        
        private Image _image;

        protected Image Image => _image ??= GetComponent<Image>();

        protected Sprite DefaultSprite => _defaultSprite;

        public abstract void UpdateView();
    }
}
