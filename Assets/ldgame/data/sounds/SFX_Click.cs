using Common;
using UnityEngine;

public class SFX_Click : CMSEntity
{
    public SFX_Click()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("sfx/menu_click".Load<AudioClip>());
    }
}