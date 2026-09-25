using System;
using UnityEngine;

namespace BelousovSDK.Audios
{
    [Serializable]
    public struct Track
    {
        public AudioClipKey Key;
        public AudioClip Clip;
    }
}