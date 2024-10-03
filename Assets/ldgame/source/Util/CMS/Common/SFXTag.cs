using UnityEngine;

namespace Common
{
    public class SFXTag : EntityComponentDefinition
    {
        public string sfx_id;
        public float Cooldown = 0.1f;
        public bool VaryPitch;
    }

    public class MusicTag : EntityComponentDefinition
    {
        public AudioClip clip;
    }

    public class AmbientTag : EntityComponentDefinition
    {
        public AudioClip clip;
    }
}