using System;
using UnityEngine.AddressableAssets;

namespace BelousovGameDev.ContentLoading
{
    [Serializable]
    public struct ContentEntry
    {
        public ContentStage Stage;

        public ContentEntryId Id;
        
        public AssetReferenceGameObject Prefab;
    }
}