using System;
using BelousovGameDev.Spawn;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace BelousovGameDev.Audios
{
    [RequireComponent(typeof(AudioSource))]
    public class PooledAudioSource : PooledComponent
    {
        private AudioSource _audioSource;

        private void Awake() =>
            _audioSource = GetComponent<AudioSource>();

        public void Initialize(AudioMixerGroup group, AudioClip clip)
        {
            _audioSource.outputAudioMixerGroup = group;
            _audioSource.clip = clip;
        }

        public async void Play()
        {
            try
            {
                _audioSource.Play();
                await UniTask.WaitUntil(() => _audioSource.isPlaying);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                Release();
            }
        }

        private void OnDisable()
        {
            if (_audioSource != null)
            {
                _audioSource.Stop();
            }
        }
    }
}
