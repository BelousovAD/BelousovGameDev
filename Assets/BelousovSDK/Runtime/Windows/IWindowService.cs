using UnityEngine;

namespace BelousovSDK.Windows
{
    public interface IWindowService
    {
        protected const int MinCountToClose = 0;
        
        public void CloseCurrent();

        public void Open(string id, int countToClose = MinCountToClose);

        public T Open<T>(string id, int countToClose = MinCountToClose) where T : Component;
        
        public void CloseCurrentIf(string id);
    }
}