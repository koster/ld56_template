using System.Collections.Generic;
using UnityEngine;

public static class SpriteUtil
{
    public static Sprite Load(string imageName)
    {
        return imageName.Load<Sprite>();
    }
    
    public static Sprite Load(string imageName, string spriteName)
    {
        Sprite[] all = Resources.LoadAll<Sprite>(imageName);
 
        foreach( var s in all)
        {
            if (s.name == spriteName)
            {
                return s;
            }
        }
        return null;
    }

    public static Sprite[] LoadSpriteSheet(string glitchManager)
    {
        return Resources.LoadAll<Sprite>(glitchManager);
    }
}