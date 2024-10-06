using Common;
using UnityEngine;

public class SFX_Animal : CMSEntity
{
    public SFX_Animal()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("sfx/animal_1".Load<AudioClip>());
        Define<SFXArray>().files.Add("sfx/animal_2".Load<AudioClip>());
        Define<SFXArray>().files.Add("sfx/animal_3".Load<AudioClip>());
        Define<SFXArray>().files.Add("sfx/animal_4".Load<AudioClip>());
    }
}