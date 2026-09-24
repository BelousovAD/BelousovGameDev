using System;
using UnityEngine;

namespace BelousovGameDev.Audios
{
    [Serializable]
    public struct Track
    {
        public AudioClipKey Key;
        public AudioClip Clip;
    }
}