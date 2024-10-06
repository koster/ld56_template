using Common;
using UnityEngine;

public class SFX_Roll : CMSEntity
{
    public SFX_Roll()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("sfx/roll".Load<AudioClip>());
    }
}