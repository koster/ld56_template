using System;
using Engine.Math;
using UnityEngine;

public static class TextStuff
{
    public static string Pinkish = "#75428f";
    public static string Greenish = "#32a852";
    public static string Orange = "#FF7700";

    public static string Common = "#999999";
    public static string Uncommon = "#6b6fe3";
    public static string Rare = "#d64dc2";

    public static string Wildcard = "Wilcard".Color(Pinkish);
    public static string Fudge = "Fudge".Color(Pinkish);
    public static string Front = "Front".Color(Color.green);
    public static string Spectral = "Spectral".Color(Pinkish);
    public static string Block = "Block".Color(Color.gray);
    public static string Clone = "Clone".Color(Greenish);
    public static string ENERGY = "ENERGY".Color(Orange);
    public static string END_TURN = "End Turn".Color(Pinkish);

    public static string RarityToString(this DiceRarity r)
    {
        switch (r)
        {
            case DiceRarity.COMMON:
                return "Common".Color(Common);
            case DiceRarity.UNCOMMON:
                return "Uncommon".Color(Uncommon);
            case DiceRarity.RARE:
                return "Rare".Color(Rare);
            default:
                throw new ArgumentOutOfRangeException(nameof(r), r, null);
        }
    }
}