using Common;
using UnityEngine;

public class SFX_Woosh : CMSEntity
{
    public SFX_Woosh()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("sfx/sfx_woosh".Load<AudioClip>());
    }
}