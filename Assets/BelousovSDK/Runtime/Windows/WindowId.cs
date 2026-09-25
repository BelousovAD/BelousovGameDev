using UnityEngine;

namespace BelousovSDK.Windows
{
    [CreateAssetMenu(fileName = nameof(WindowId), menuName = nameof(Windows) + "/" + nameof(WindowId))]
    internal class WindowId : ScriptableObject
    {
        [SerializeField] private string _value;

        public string Value => _value;
    }
}