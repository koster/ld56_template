using Common;
using UnityEngine;

public class SFX_DiceDraw : CMSEntity
{
    public SFX_DiceDraw()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("sfx/give_out_dice".Load<AudioClip>());
    }
}