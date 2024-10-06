using Common;
using UnityEngine;

public class SFX_Win : CMSEntity
{
    public SFX_Win()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("sfx/win_chime".Load<AudioClip>());
    }
}