using System;
using System.Collections.Generic;
using UnityEngine;

namespace BelousovGameDev.Windows
{
    internal class WindowService : IWindowService
    {
        private readonly IWindowSpawner _spawner;
        private readonly Stack<Window> _windowsHistory = new ();
        private readonly Dictionary<string, Window> _spawnedWindows = new ();

        public WindowService(IWindowSpawner spawner) =>
            _spawner = spawner;

        public void CloseCurrent()
        {
            if (_windowsHistory.Count <= 0)
            {
                return;
            }

            Window window = _windowsHistory.Pop();
            window.SetInteractable(false);
            window.SetActive(false);

            if (_windowsHistory.Count <= 0)
            {
                return;
            }

            window = _windowsHistory.Peek();
            window.SetActive(true);
            window.SetInteractable(true);
        }

        public void CloseCurrentIf(string id)
        {
            if (_windowsHistory.TryPeek(out Window window) && window.Id == id)
            {
                CloseCurrent();
            }
        }

        public void Open(string id, int countToClose = IWindowService.MinCountToClose) =>
            OpenWindowCore(id, countToClose);

        public T Open<T>(string id, int countToClose = IWindowService.MinCountToClose) where T : Component
        {
            Window window = OpenWindowCore(id, countToClose);
            T component = window.GetComponentInChildren<T>(true);

            return component != null
                ? component
                : throw new InvalidOperationException($"Window {id} does not contain {typeof(T).Name}");
        }

        private Window OpenWindowCore(string id, int countToClose = IWindowService.MinCountToClose)
        {
            if (countToClose < IWindowService.MinCountToClose)
            {
                throw new ArgumentOutOfRangeException(nameof(countToClose), countToClose, null);
            }

            if (_windowsHistory.TryPeek(out Window lastWindow))
            {
                lastWindow.SetInteractable(false);
            }
            
            for (int i = 0; i < countToClose; i++)
            {
                if (_windowsHistory.TryPop(out Window historyWindow))
                {
                    historyWindow.SetActive(false);
                }
            }
            
            if (!_spawnedWindows.TryGetValue(id, out Window window))
            {
                window = _spawner.Spawn(id);
                _spawnedWindows.Add(id, window);
            }

            _windowsHistory.Push(window);
            window.SetActive(true);
            window.SetInteractable(true);

            return window;
        }
    }
}
