using Common;
using UnityEngine;

public class Sound_AmbienceForest : CMSEntity
{
    public Sound_AmbienceForest()
    {
        Define<AmbientTag>().clip = "ambience/forest_atmos_brighter".Load<AudioClip>();
    }
}