using Common;
using UnityEngine;

public class SFX_GetDamage : CMSEntity
{
    public SFX_GetDamage()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("sfx/get_damage".Load<AudioClip>());
    }
}