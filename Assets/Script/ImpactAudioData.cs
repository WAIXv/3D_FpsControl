using System.Collections.Generic;
using UnityEngine;

namespace Script.Weapon
{
    [CreateAssetMenu(menuName = "FPS/Impact Audio Data")]
    public class ImpactAudioData: ScriptableObject
    {
        public List<ImpactTagWithAudio> ImpactTagWithAudios;
    }

    [System.Serializable]
    public class ImpactTagWithAudio
    {
        public string Tag;
        public List<AudioClip> ImpactAudioClips;
    }
}