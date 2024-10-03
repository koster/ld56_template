using System.Collections.Generic;
using UnityEngine;

public class SFXDefinition : CMSEntity
{
    
}

public class SFXArray : EntityComponentDefinition
{
    public List<AudioClip> files = new List<AudioClip>();
    public float volume = 1f;
}

public class SFXVolume : EntityComponentDefinition
{
}