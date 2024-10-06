using Common;
using UnityEngine;

public class SFX_Kill : CMSEntity
{
    public SFX_Kill()
    {
        Define<SFXTag>();
        Define<SFXArray>().volume = 0.5f;
        Define<SFXArray>().files.Add("sfx/kill".Load<AudioClip>());
    }
}