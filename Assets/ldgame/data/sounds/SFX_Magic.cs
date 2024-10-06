using Common;
using UnityEngine;

public class SFX_Magic : CMSEntity
{
    public SFX_Magic()
    {
        Define<SFXTag>();
        Define<SFXArray>().volume = 0.5f;
        Define<SFXArray>().files.Add("sfx/win_magic".Load<AudioClip>());
    }
}