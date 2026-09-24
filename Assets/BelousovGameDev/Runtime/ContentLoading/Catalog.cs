using System.Collections.Generic;
using UnityEngine;

namespace BelousovGameDev.ContentLoading
{
    [CreateAssetMenu(fileName = nameof(Catalog), menuName = nameof(ContentLoading) + "/" + nameof(Catalog))]
    public class Catalog : ScriptableObject
    {
        [SerializeField] private List<ContentEntry> _entries;
        
        public IReadOnlyList<ContentEntry> Entries => _entries;
    }
}
