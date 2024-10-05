using Engine.Math;
using UnityEngine;

public static class TextStuff
{
    public static string Pinkish = "#d879e0";
    public static string Wildcard = "Wilcard".Color(Pinkish);
    public static string Fudge = "Fudge".Color(Pinkish);
    public static string Front = "Front".Color(Color.green);
    public static string Spectral = "Spectral".Color(Pinkish);
}

public class WildcardDice : BasicDice
{
    public WildcardDice()
    {
        Define<TagTint>().color = Color.magenta;
        Define<TagWildcard>();
        Define<TagDescription>().loc = $"{TextStuff.Wildcard}: Can go into any goal!";
    }
}

public class TagWildcard : EntityComponentDefinition
{
}