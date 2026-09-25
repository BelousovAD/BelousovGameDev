using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BelousovSDK.Timers
{
    public class Timer : IDisposable
    {
        private const int Min = 0;
        private const int Second = 1;
        
        private int _time;
        private CancellationTokenSource _cancellationTokenSource;

        public Timer(int max)
        {
            if (max <= Min)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "Must be positive");
            }
            
            Max = max;
        }

        public event Action Changed;
        
        public event Action Finished;
        
        public int Max { get; }

        public int Time
        {
            get => _time;
            
            private set
            {
                _time = Mathf.Clamp(value, Min, Max);
                Changed?.Invoke();
            }
        }

        public void Dispose() =>
            Stop();

        public void Start()
        {
            Stop();
            Time = Max;
            _cancellationTokenSource = new CancellationTokenSource();
            Countdown(_cancellationTokenSource.Token).Forget();
        }

        private void Stop()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private async UniTask Countdown(CancellationToken token)
        {
            while (!token.IsCancellationRequested && Time > Min)
            {
                if (await UniTask.Delay(TimeSpan.FromSeconds(Second), cancellationToken: token)
                        .SuppressCancellationThrow() == false)
                {
                    Time--;
                }
            }
            
            if (!token.IsCancellationRequested)
            {
                Finished?.Invoke();
            }
        }
    }
}