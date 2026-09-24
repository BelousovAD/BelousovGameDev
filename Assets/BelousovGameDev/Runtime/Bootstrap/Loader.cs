using System;
using System.Collections.Generic;
using BelousovGameDev.ContentLoading;
using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using Savvy.Container;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BelousovGameDev.Bootstrap
{
    internal class Loader : MonoSavvy
    {
        [SerializeField] private string _sceneToLoad;
        [SerializeField] private ContentStage _stage = ContentStage.Initial;
        [SerializeField] private List<MonoBehaviour> _loaders = new ();
        
        private ContentLoadingService _content;

        [Inject]
        private void Initialize(ContentLoadingService content) =>
            _content = content;

        private void Start() =>
            LoadAndEnterAsync().Forget();

        private async UniTaskVoid LoadAndEnterAsync()
        {
            try
            {
                Load();
                await _content.LoadStageAsync(_stage);
                await SceneManager.LoadSceneAsync(_sceneToLoad);
            }
            catch (Exception exception)
            {
                Error("Loading failed.");
                UnityEngine.Debug.LogException(exception);
            }
        }

        private void Load()
        {
            foreach (MonoBehaviour monoBehaviour in _loaders)
            {
                if (monoBehaviour is ILoadable loader)
                {
                    loader.Load();
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Unexpected loader that is null or does not inherit {nameof(ILoadable)}");
                }
            }
        }

        private void OnValidate()
        {
            foreach (MonoBehaviour monoBehaviour in _loaders)
            {
                if (monoBehaviour is not null and not ILoadable)
                {
                    Error($"Elements in {nameof(_loaders)} must inherit {nameof(ILoadable)}");
                }
            }
        }
    }
}
