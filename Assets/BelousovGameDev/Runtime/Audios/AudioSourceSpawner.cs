using BelousovGameDev.Spawn;

namespace BelousovGameDev.Audios
{
    public class AudioSourceSpawner : SiblingsSpawner
    {
        public void Prewarm(int count)
        {
            PooledAudioSource[] sources = new PooledAudioSource[count];
            
            for (int i = 0; i < count; i++)
            {
                sources[i] = Spawn();
            }
            
            foreach (PooledAudioSource source in sources)
            {
                source.Release();
            }
        }

        public new PooledAudioSource Spawn() =>
            base.Spawn().GetComponent<PooledAudioSource>();
    }
}
