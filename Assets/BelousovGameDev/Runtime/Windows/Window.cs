using System;
using UnityEngine;

namespace BelousovGameDev.Windows
{
    internal class Window : MonoBehaviour
    {
        [SerializeField] private WindowId _id;

        private bool _isActive;
        private bool _isInteractable;

        public event Action ActivityChanged;

        public event Action InteractableChanged;

        public string Id => _id.Value;

        public bool IsActive
        {
            get => _isActive;

            private set
            {
                if (value != _isActive)
                {
                    _isActive = value;
                    ActivityChanged?.Invoke();
                }
            }
        }

        public bool IsInteractable
        {
            get => _isInteractable;

            private set
            {
                if (value != _isInteractable)
                {
                    _isInteractable = value;
                    InteractableChanged?.Invoke();
                }
            }
        }

        public void SetActive(bool value) =>
            IsActive = value;

        public void SetInteractable(bool value) =>
            IsInteractable = value;
    }
}