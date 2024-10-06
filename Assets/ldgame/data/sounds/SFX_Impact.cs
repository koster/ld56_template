using Common;
using UnityEngine;

public class SFX_Impact : CMSEntity
{
    public SFX_Impact()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("sfx/sfx_impact".Load<AudioClip>());
    }
}