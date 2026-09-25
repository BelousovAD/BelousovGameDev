using System;
using BelousovSDK.ContentLoading;
using Reflex.Attributes;
using UnityEngine;

namespace BelousovSDK.Windows
{
    internal class WindowSpawner : MonoBehaviour, IWindowSpawner
    {
        private ContentLoadingService _content;

        [Inject]
        private void Initialize(ContentLoadingService content) =>
            _content = content;

        public Window Spawn(string id)
        {
            Window window = _content.GetPrefab(id).GetComponent<Window>();
            window = Instantiate(window ?? throw new InvalidOperationException($"Can't open window {id}"), transform);

            return window;
        }
    }
}