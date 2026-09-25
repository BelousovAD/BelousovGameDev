using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BelousovSDK.ContentLoading
{
    public class ContentLoadingService : IDisposable
    {
        private readonly CancellationTokenSource _lifetime = new();
        private readonly Dictionary<ContentStage, UniTaskCompletionSource> _stages = new();
        private readonly HashSet<ContentStage> _ready = new();
        private readonly Dictionary<string, AsyncOperationHandle<GameObject>> _prefabs = new();
        private bool _disposed;

        public ContentLoadingService(Catalog catalog) =>
            Catalog = catalog;

        public event Action StageReady;
        
        public Catalog Catalog { get; }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            
            _disposed = true;
            _lifetime.Cancel();
            
            foreach (AsyncOperationHandle<GameObject> handle in _prefabs.Values)
            {
                ReleaseWhenComplete(handle);
            }
            
            _prefabs.Clear();
            _lifetime.Dispose();
        }

        public bool IsReady(ContentStage stage) =>
            _ready.Contains(stage);

        public UniTask LoadStageAsync(ContentStage stage)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ContentLoadingService));
            }
            
            if (!_stages.TryGetValue(stage, out UniTaskCompletionSource completion))
            {
                completion = new UniTaskCompletionSource();
                _stages.Add(stage, completion);
                LoadStageWithRetryAsync(stage, completion).Forget();
            }
            
            return completion.Task;
        }

        public GameObject GetPrefab(string id)
        {
            ContentEntry entry = Catalog.Entries.FirstOrDefault(entry => entry.Id.Value == id);
            
            if (!_prefabs.TryGetValue(entry.Prefab.AssetGUID, out AsyncOperationHandle<GameObject> handle))
            {
                throw new InvalidOperationException(
                    $"Prefab '{id}' is not ready.");
            }
            
            return handle.Result;
        }

        private async UniTask LoadStageWithRetryAsync(ContentStage stage, UniTaskCompletionSource completion)
        {
            // CompletionSource supports concurrent callers; Preserve only memoizes a result
            // and cannot accept two continuations while the original UniTask is pending.
            try
            {
                await LoadStageCoreAsync(stage);
                completion.TrySetResult();
            }
            catch (OperationCanceledException exception)
            {
                _stages.Remove(stage);
                completion.TrySetCanceled(exception.CancellationToken);
            }
            catch (Exception exception)
            {
                _stages.Remove(stage);
                completion.TrySetException(exception);
            }
        }

        private async UniTask LoadStageCoreAsync(ContentStage stage)
        {
            if (Catalog == null)
            {
                throw new InvalidOperationException("Content catalog is not assigned to BootstrapInstaller.");
            }
            
            if (stage != ContentStage.Initial)
            {
                await LoadStageAsync(stage - 1);
            }
            
            Debug.Log($"[Content] {stage}: loading");
            
            if (stage == ContentStage.Initial)
            {
                AsyncOperationHandle<IResourceLocator> initialization = Addressables.InitializeAsync(false);
                
                try
                {
                    await WaitAsync(initialization);
                }
                finally
                {
                    ReleaseWhenComplete(initialization);
                }
            }

            // Download-only operations use a different provider cache key and do not retain
            // a loaded bundle. On WebGL that caused a second request when loading the asset.
            // Load actual prefabs once per stage; retain their dependency leases for this session.
            foreach (ContentEntry entry in Catalog.Entries.Where(entry => entry.Stage == stage))
            {
                if (_prefabs.ContainsKey(entry.Prefab.AssetGUID))
                {
                    continue;
                }

                AsyncOperationHandle<GameObject> handle =
                    Addressables.LoadAssetAsync<GameObject>(entry.Prefab.RuntimeKey);

                try
                {
                    await WaitAsync(handle);
                    _prefabs.Add(entry.Prefab.AssetGUID, handle);
                }
                catch
                {
                    ReleaseWhenComplete(handle);
                    throw;
                }
            }
            
            _ready.Add(stage);
            Debug.Log($"[Content] {stage}: ready");
            StageReady?.Invoke();
        }

        private async UniTask WaitAsync(AsyncOperationHandle handle)
        {
            await UniTask.WaitUntil(() => handle.IsDone, cancellationToken: _lifetime.Token);
            
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new InvalidOperationException("Addressables operation failed: " + handle.DebugName,
                    handle.OperationException);
            }
        }

        private static void ReleaseWhenComplete(AsyncOperationHandle handle)
        {
            if (!handle.IsValid())
            {
                return;
            }
            
            if (handle.IsDone)
            {
                Addressables.Release(handle);
            }
            else
            {
                handle.Completed += Addressables.Release;
            }
        }
    }
}
