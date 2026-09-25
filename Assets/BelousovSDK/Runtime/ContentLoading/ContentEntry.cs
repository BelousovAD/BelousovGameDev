using System;
using UnityEngine.AddressableAssets;

namespace BelousovSDK.ContentLoading
{
    [Serializable]
    public struct ContentEntry
    {
        public ContentStage Stage;

        public ContentEntryId Id;
        
        public AssetReferenceGameObject Prefab;
    }
}