using UnityEngine;

namespace BelousovGameDev.ContentLoading
{
    [CreateAssetMenu(fileName = nameof(ContentEntryId),
        menuName = nameof(ContentLoading) + "/" + nameof(ContentEntryId))]
    public class ContentEntryId : ScriptableObject
    {
        [SerializeField] private string _value;

        public string Value => _value;
    }
}