using Common;
using UnityEngine;

public class SFX_TypeChar : CMSEntity
{
    public SFX_TypeChar()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("sfx/HighVoice".Load<AudioClip>());
    }
}