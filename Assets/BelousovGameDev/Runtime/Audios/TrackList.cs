using System.Collections.Generic;
using UnityEngine;

namespace BelousovGameDev.Audios
{
    [CreateAssetMenu(fileName = nameof(TrackList), menuName = nameof(Audios) + "/" + nameof(TrackList))]
    internal class TrackList : ScriptableObject
    {
        [SerializeField] private List<Track> _tracks = new ();

        public IReadOnlyList<Track> Tracks => _tracks;
    }
}